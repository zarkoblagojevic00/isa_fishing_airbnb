using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Domain.Entities;
using Domain.Mappings;
using Domain.UnitOfWork;
using FluentAssertions;
using Infrastructure;
using IntegrationTests.Parameters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            context.Set(string.Empty,"pathParams");

            CookieContainer = new CookieContainer();
            ClientHandler = new HttpClientHandler()
            {
                CookieContainer = CookieContainer
            };

            Client = new HttpClient(ClientHandler);

            var jsonObject = new JsonObject();
            ScenarioContext.Set<JsonObject>(jsonObject);
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Client.Dispose();
            ClientHandler.Dispose();
        }

        [Given(@"a key ""(.*)"" had a value ""(.*)"" in the jsonObject")]
        public void AKeyHadAValueInJsonObject(string key, string value)
        {
            var jsonObject = ScenarioContext.Get<JsonObject>();

            jsonObject.Add(key, value);

            ScenarioContext.Set<JsonObject>(jsonObject);
        }

        [Given(@"a path parameter with the key ""(.*)"" has the value ""(.*)""")]
        public void APathParameterWithKeyHasTheValue(string key, string value)
        {
            var str = ScenarioContext.Get<string>("pathParams");

            if (str == string.Empty)
                str = "?" + key + "=" + value;
            else
                str = "&" + key + "=" + value;

            ScenarioContext.Set(str, "pathParams");
        }

        [When(@"a request is sent to the API")]
        public async Task ARequestIsSentToTheAPI(Table table)
        {
            var parameters = table.CreateInstance<HttpRequestParameter>();

            Client.BaseAddress = new Uri(parameters.BaseUrl);

            if (!string.IsNullOrEmpty(parameters.CookieEmail))
            {
                CookieContainer.Add(new Cookie("email", parameters.CookieEmail));
            }

            if (!string.IsNullOrEmpty(parameters.CookieUserId))
            {
                CookieContainer.Add(new Cookie("userId",parameters.CookieUserId));
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
            var jsonObject = ScenarioContext.Get<JsonObject>();
            var pathParams = ScenarioContext.Get<string>("pathParams");
            HttpResponseMessage response;

            switch (method.ToLower())
            {
                case "get":
                    response = await Client.GetAsync(url + pathParams);
                    break;
                default:
                    throw new Exception("Method not recognized");
            }

            ScenarioContext.Set<HttpResponseMessage>(response);
        }
    }
}
