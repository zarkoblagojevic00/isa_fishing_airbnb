using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Domain.Repositories;
using Domain.UnitOfWork;
using TechTalk.SpecFlow;

namespace IntegrationTests.Steps
{
    [Binding]
    [Scope(Feature = "QuickActions")]
    public sealed class QuickActionsSteps
    {
        [Given(@"there were no quick actions nur reservations in past ""(.*)"" days for the villa ""(.*)"" linked with ""(.*)""")]
        public void ThereWereNoQuickActionsNurReservationsInPastDays(int days, string name, string ownerEmail)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var owner = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .First(x => x.Email == ownerEmail);

        }
    }
}
