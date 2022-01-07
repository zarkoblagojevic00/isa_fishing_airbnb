using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API.DTOs;
using Autofac;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentAssertions;
using FluentNHibernate.Conventions;
using IntegrationTests.Parameters;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Service = Domain.Entities.Service;

namespace IntegrationTests.Steps
{
    [Binding]
    [Scope(Feature = "GeneralService")]
    public sealed class GeneralServiceSteps
    {
        [Given(@"the ReportDTO was created and linked to previously created reservation as content with following properties")]
        public void ReportDTOWasCreatedAsContentWithFollowing(Table table)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();
            var previouslyCreatedReservation = CommonSteps.ScenarioContext.Get<Reservation>(TestConstants.TestReservation);
            var reportDto = table.CreateInstance<ReportDTO>();

            reportDto.ReservationId = previouslyCreatedReservation.ReservationId;

            var content = JsonConvert.SerializeObject(reportDto);

            CommonSteps.ScenarioContext.Set(content, TestConstants.Content);
        }

        [Given(@"a NewReservationParameter was created as content for the previously created user and service with name ""(.*)"" with the following properties")]
        public void NewReservationDTOWasCreatedForThePreviouslyCreatedUser(string name, Table table)
        {
            var user = CommonSteps.ScenarioContext.Get<User>(TestConstants.TestUser);
            var service = CommonSteps.ScenarioContext.Get<Service>(name);
            var parameter = table.CreateInstance<NewReservationParameter>();

            if (parameter.EndDaysAfterToday <= parameter.BeginDaysAfterToday)
            {
                throw new Exception($"Begin days '{parameter.BeginDaysAfterToday}' isn't less than End days '{parameter.EndDaysAfterToday}'");
            }

            var dto = new NewReservationDTO()
            {
                AdditionalEquipment = parameter.AdditionalEquipment,
                Price = parameter.PricePerDay,
                ServiceId = service.ServiceId,
                StartDateTime = DateTime.Now.AddDays(parameter.BeginDaysAfterToday),
                EndDateTime = DateTime.Now.AddDays(parameter.EndDaysAfterToday),
                UserMail = user.Email
            };

            var content = JsonConvert.SerializeObject(dto);
            CommonSteps.ScenarioContext.Set(content, TestConstants.Content);
        }

        [Then(@"a new report will be created for the service with name ""(.*)"" in ""(.*)"" seconds")]
        public void NewReportWillBeCreatedForServiceWithNameInSeconds(string name, int timeout)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();
            var service = CommonSteps.ScenarioContext.Get<Service>(name);
            var reservation = CommonSteps.ScenarioContext.Get<Reservation>(TestConstants.TestReservation);
            var content = CommonSteps.ScenarioContext.Get<string>(TestConstants.Content);

            var sentDto = JsonConvert.DeserializeObject<ReportDTO>(content);
            var retry = new Retry<IUnitOfWork>(uow);

            var pollingInterval = CommonSteps.FeatureContext.Get<int>(TestConstants.PollingInterval);

            try
            {
                var report = retry.Do(GetReport, TimeSpan.FromSeconds(timeout),
                    TimeSpan.FromSeconds(pollingInterval));

                report.Should().NotBeNull();
                report.ReportText.Should().Be(sentDto.ReportText);

                Report GetReport(IUnitOfWork currUoW)
                {
                    var reportId = currUoW.GetRepository<IReservationReadRepository>()
                        .GetById(sentDto.ReservationId)
                        .ReportId;
                    return currUoW.GetRepository<IReportReadRepository>()
                        .GetById(reportId.Value);
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Report not found for the Reservation : {reservation.ReservationId} with text {sentDto.ReportText} in '{timeout}' seconds");
            }
        }

        [Then(@"there should be a new reservation based on sent content created for the previously created user and service with name ""(.*)""")]
        public void ThereShouldBeANewReservationCreatedForTheCreatedUser(string serviceName)
        {
            var content = CommonSteps.ScenarioContext.Get<string>(TestConstants.Content);
            var user = CommonSteps.ScenarioContext.Get<User>(TestConstants.TestUser);
            var service = CommonSteps.ScenarioContext.Get<Service>(serviceName);

            var sentDto = JsonConvert.DeserializeObject<NewReservationDTO>(content);

            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var newReservation = uow.GetRepository<IReservationReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.UserId == user.UserId && x.ServiceId == service.ServiceId &&
                                     x.StartDateTime == sentDto.StartDateTime && x.EndDateTime == sentDto.EndDateTime);

            newReservation.Should().NotBeNull();
        }

    }
}
