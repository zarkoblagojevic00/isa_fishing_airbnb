using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Domain.Entities;
using Domain.Mappings;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentAssertions;
using Infrastructure;
using IntegrationTests.Parameters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NHibernate.Util;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace IntegrationTests.Steps
{
    [Binding]
    public class CommonSteps
    {
        public static FeatureContext FeatureContext { get; private set; }
        public static ScenarioContext ScenarioContext { get; private set; }
        public static HttpClient Client { get; private set; }
        public static HttpClientHandler ClientHandler { get; private set; }
        public static CookieContainer CookieContainer { get; private set; }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext context)
        {
            FeatureContext = context;

            var builder = new ContainerBuilder();
            builder.RegisterModule(new NHibernateModule()
            {
                DbType = "SqlServer",
                ConnectionString = "Data Source=localhost;Initial Catalog=ISA;Integrated Security=True", //TODO: fix this so it reads from APP.config
                MappingAssemblies = new List<Assembly>()
                {
                    typeof(AccountDeletionRequestMap).Assembly
                }
            });

            builder.RegisterModule(new RepositoryModule()
            {
                RepositoryAssemblies = new System.Collections.Generic.List<Assembly>()
                {
                    typeof(AccountDeletionRequest).Assembly
                }
            });

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            var container = builder.Build();

            FeatureContext.Set<ILifetimeScope>(container);
            FeatureContext.Set(10, "PollingInterval");
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            FeatureContext.Get<ILifetimeScope>().Dispose();
        }

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext context)
        {
            ScenarioContext = context;
            context.Set(string.Empty,TestConstants.PathParam);
            context.Set(string.Empty, TestConstants.Content);
            CookieContainer = new CookieContainer();
            ClientHandler = new HttpClientHandler()
            {
                CookieContainer = CookieContainer
            };

            Client = new HttpClient(ClientHandler);
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Client.Dispose();
            ClientHandler.Dispose();
        }
        
        [Given(@"a path parameter with the key ""(.*)"" has the value ""(.*)""")]
        public void APathParameterWithKeyHasTheValue(string key, string value)
        {
            var str = ScenarioContext.Get<string>(TestConstants.PathParam);

            if (str == string.Empty)
                str = "?" + key + "=" + value;
            else
                str = "&" + key + "=" + value;

            ScenarioContext.Set(str, TestConstants.PathParam);
        }

        [Given(@"there was a test villa owner with mail ""(.*)"" in database")]
        public void ThereWasTestVillaOwnerWithMail(string mail)
        {
            var uow = FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var user = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Email == mail);

            if (user == null)
            {
                uow.BeginTransaction();

                user = new User()
                {
                    Email = mail,
                    UserType = UserType.VillaOwner
                };

                uow.GetRepository<IUserWriteRepository>().Add(user);

                uow.Commit();
            }
            else
            {
                if (user.UserType != UserType.VillaOwner)
                {
                    uow.BeginTransaction();
                    user.UserType = UserType.VillaOwner;

                    uow.GetRepository<IUserWriteRepository>().Update(user);
                    uow.Commit();
                }
            }
        }

        [Given(@"there was a villa in the database named ""(.*)"" linked with villa owner with email ""(.*)""")]
        public void ThereWasAVillaInTheDatabaseNamedLinkedWithTheOwner(string name, string email)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var testVillaOwner = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Email == email);

            uow.BeginTransaction();
            if (testVillaOwner == null)
            {
                testVillaOwner = new User()
                {
                    Email = TestConstants.TestVillaOwnerMail,
                    IsAccountVerified = true,
                    IsAccountActive = true
                };
                uow.GetRepository<IUserWriteRepository>().Add(testVillaOwner);
            }

            var service = uow.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == name && x.OwnerId == testVillaOwner.UserId);

            if (service == null)
            {
                service = new Domain.Entities.Service()
                {
                    Name = name,
                    Capacity = 1,
                    Longitude = 0,
                    Latitude = 0,
                    OwnerId = testVillaOwner.UserId
                };

                uow.GetRepository<IServiceWriteRepository>().Add(service);

                var adInfo = new AdditionalVillaServiceInfo()
                {
                    ServiceId = service.ServiceId
                };
                uow.GetRepository<IAdditionalVillaServiceInfoWriteRepository>().Add(adInfo);
            }

            uow.Commit();
        }

        [When(@"a request is sent to the API")]
        public async Task ARequestIsSentToTheAPI(Table table)
        {
            var parameters = table.CreateInstance<HttpRequestParameter>();

            Client.BaseAddress = new Uri(parameters.BaseUrl);

            if (!string.IsNullOrEmpty(parameters.CookieEmail))
            {
                CookieContainer.Add(new Cookie("email", parameters.CookieEmail) {Domain = TestConstants.Domain});

                var uow = FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();
                var user = uow.GetRepository<IUserReadRepository>()
                    .GetAll()
                    .FirstOrDefault(x => x.Email == parameters.CookieEmail);

                if (user != null)
                {
                    CookieContainer.Add(new Cookie("userId", user.UserId.ToString()) { Domain = TestConstants.Domain });
                }
            }

            await SendRequest(parameters.HttpMethod, parameters.RelativeResourceUrl);
        }

        [Then(@"a ""(.*)"" status code should be received")]
        public void AStatusCodeShouldBeReceivedWithData(HttpStatusCode statusCode)
        {
            var response = ScenarioContext.Get<HttpResponseMessage>();

            response.StatusCode.Should().Be(statusCode);
        }

        [Then(@"the response will come with following json object")]
        public async Task TheResponseWillComeWithFollowingJsonObject(Table table)
        {
            var response = ScenarioContext.Get<HttpResponseMessage>();
            var kvps = table.CreateSet<HttpResponseParameter>();
            var content = await response.Content.ReadAsStringAsync();

            var receivedKvp = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

            foreach (var kvp in kvps)
            {
                var found = receivedKvp[kvp.Key];
                found.Should().NotBeNullOrEmpty();

                found.Should().Be(kvp.Value);
            }
        }

        private async Task SendRequest(string method, string url)
        {
            var json = ScenarioContext.Get<string>(TestConstants.Content);
            var content = new StringContent(json, Encoding.Unicode, "application/json");

            var pathParams = ScenarioContext.Get<string>(TestConstants.PathParam);
            HttpResponseMessage response;

            switch (method.ToLower())
            {
                case "get":
                    response = await Client.GetAsync(url + pathParams);
                    break;
                case "post":
                    response = await Client.PostAsync(url + pathParams, content);
                    break;
                case "delete":
                    response = await Client.DeleteAsync(url + pathParams);
                    break;
                case "put":
                    response = await Client.PutAsync(url + pathParams, content);
                    break;
                default:
                    throw new Exception("Method not recognized");
            }

            ScenarioContext.Set<HttpResponseMessage>(response);
        }
    }
}
