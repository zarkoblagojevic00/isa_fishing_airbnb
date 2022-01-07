using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AcceptanceTests.Parameters;
using API.DTOs;
using Autofac;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentAssertions;
using IntegrationTests;
using IntegrationTests.Steps;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHibernate.Mapping;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Table = TechTalk.SpecFlow.Table;

namespace AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ConcurrencyChecking")]
    public class ConcurrencyCheckingSteps
    {
        [When(@"The user with mail ""(.*)"" sends multiple requests for service with name ""(.*)"" at the same time with the following properties")]
        public async Task TwoRequestsThatOverlapSentAtTheSameTime(string mail, string name, Table table)
        {
            var requests = table.CreateSet<ReservationDTOParameter>();

            var transformedRequests = new List<NewReservationDTO>();

            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();
            var owner = uow.GetRepository<IUserReadRepository>().GetAll().First(x => x.Email == mail);

            foreach (var request in requests)
            {
                var dto = new NewReservationDTO()
                {
                    UserMail = request.UserMail,
                    ServiceId = uow.GetRepository<IServiceReadRepository>().GetAll()
                        .First(x => x.OwnerId == owner.UserId && x.Name == name).ServiceId,
                    AdditionalEquipment = request.AdditionalEquipment,
                    StartDateTime = DateTime.Now.AddDays(request.From),
                    EndDateTime = DateTime.Now.AddDays(request.To),
                    Price = request.Price
                };

                transformedRequests.Add(dto);
            }

            var responses = new List<HttpResponseMessage>();
            var tasks = new List<Task>();
            var mutex = new Mutex();
            foreach (var request in transformedRequests)
            {
                var task = SendRequestsAndSerializeAnswers(mutex, owner, request, responses);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            CommonSteps.FeatureContext.Set(responses, "response");
        }

        [Then(@"only one request should succeed")]
        public async Task OnlyOneRequestShouldSucceed()
        {
            var responses = CommonSteps.FeatureContext.Get<List<HttpResponseMessage>>("response");

            var successCounter = 0;
            foreach (var response in responses)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    successCounter++;
                }
            }

            successCounter.Should().Be(1);
        }

        private async Task<int> SendRequestsAndSerializeAnswers(Mutex mutex, User owner, NewReservationDTO dto, List<HttpResponseMessage> results)
        {
            var cookieContainer = new CookieContainer();
            var clientHandler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            };
            var client = new HttpClient(clientHandler);

            cookieContainer.Add(new Cookie("userId", owner.UserId.ToString()){Domain = TestConstants.Domain});
            cookieContainer.Add(new Cookie("email", owner.Email){Domain = TestConstants.Domain});

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.Unicode, "application/json");

            var response = await client.PostAsync(TestConstants.Localhost + "api/GeneralService/CreateReservationForUser", content);

            mutex.WaitOne();

            results.Add(response);

            mutex.ReleaseMutex();

            return 1;
        }
    }
}
