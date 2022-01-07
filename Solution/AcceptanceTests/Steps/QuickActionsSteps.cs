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
using IntegrationTests.Parameters;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Service = Domain.Entities.Service;

namespace IntegrationTests.Steps
{
    [Binding]
    [Scope(Feature = "QuickActions")]
    public sealed class QuickActionsSteps
    {
        [Given(@"a QuickActionParameter was created for the villa ""(.*)"" with the following properties")]
        public void QuickActionDTOWasCreated(string villaName, Table table)
        {
            var parameter = table.CreateInstance<QuickActionParameter>();
            var service = CommonSteps.ScenarioContext.Get<Service>(villaName);

            if (parameter.EndDaysAfterToday < parameter.BeginDaysAfterToday)
            {
                throw new Exception(
                    $"Begin days '{parameter.BeginDaysAfterToday}' isn't less than End days '{parameter.EndDaysAfterToday}'");
            }

            var quickActionDto = new QuickActionDTO()
            {
                ServiceId = service.ServiceId,
                StartDateTime = DateTime.Now.AddDays(parameter.BeginDaysAfterToday),
                EndDateTime = DateTime.Now.AddDays(parameter.EndDaysAfterToday),
                Capacity = parameter.Capacity,
                PricePerDay = parameter.PricePerDay
            };

            var content = JsonConvert.SerializeObject(quickActionDto);

            CommonSteps.ScenarioContext.Set(content, TestConstants.Content);
        }

        [Given(@"that QuickActionDTO had the PromoActionId set previously")]
        public void ThatQuickActionHadPromoActionIdSetToTheLast()
        {
            var lastPromoAction = CommonSteps.ScenarioContext.Get<PromoAction>(TestConstants.TestPromoAction);

            var content = CommonSteps.ScenarioContext.Get<string>(TestConstants.Content);

            var dto = JsonConvert.DeserializeObject<QuickActionDTO>(content);
            dto.PromoActionId = lastPromoAction.PromoActionId;

            content = JsonConvert.SerializeObject(dto);

            CommonSteps.ScenarioContext.Set(content, TestConstants.Content);
        }

        [Given(@"previously created promo action was taken")]
        public void PreviouslyCreatedPromoActionWasTaken()
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var previouslyCreatedAction = CommonSteps.ScenarioContext.Get<PromoAction>(TestConstants.TestPromoAction);
            previouslyCreatedAction.IsTaken = true;

            uow.BeginTransaction();

            uow.GetRepository<IPromoActionWriteRepository>().Update(previouslyCreatedAction);

            uow.Commit();
            CommonSteps.ScenarioContext.Set(previouslyCreatedAction, TestConstants.TestPromoAction);
        }

        [Then(@"service with the name ""(.*)"" should have the promo action with the properties from current content")]
        public void ServiceWithTheNameShouldHavePromoActionWithTheProperties(string name)
        {
            var service = CommonSteps.ScenarioContext.Get<Service>(name);
            var sentContent = CommonSteps.ScenarioContext.Get<string>(TestConstants.Content);
            var sentDto = JsonConvert.DeserializeObject<QuickActionDTO>(sentContent);

            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var foundAction = uow.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == service.ServiceId && x.StartDateTime == sentDto.StartDateTime &&
                            x.EndDateTime == sentDto.EndDateTime);

            foundAction.Should().NotBeNull();
        }
    }
}
