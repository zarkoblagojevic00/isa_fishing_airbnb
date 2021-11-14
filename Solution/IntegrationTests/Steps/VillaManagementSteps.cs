using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API.DTOs;
using Autofac;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace IntegrationTests.Steps
{
    [Binding]
    [Scope(Feature = "VillaManagement")]
    public sealed class VillaManagementSteps
    {
        [Given(@"there was no villa with the name ""(.*)"" linked with test villa owner")]
        public void GivenThereWasNoVillaWithNameLinkedWithUser(string name)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            uow.BeginTransaction();

            var testVillaOwner = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Email == TestConstants.TestVillaOwnerMail);

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
                .FirstOrDefault(x => x.Name == name && x.OwnerId == testVillaOwner.UserId && x.ServiceType == ServiceType.Villa);

            if (service != null)
            {
                var adInfo = uow.GetRepository<IAdditionalVillaServiceInfoReadRepository>()
                    .GetAll()
                    .FirstOrDefault(x => x.ServiceId == service.ServiceId);
                uow.GetRepository<IAdditionalVillaServiceInfoWriteRepository>().Delete(adInfo);
                uow.GetRepository<IServiceWriteRepository>().Delete(service);
            }

            uow.Commit();
        }

        [Given(@"a new villa DTO was created with the following properties")]
        public void NewVillaDTOWasCreatedWithFollowingProperties(Table table)
        {
            var villaDTO = table.CreateInstance<VillaDTO>();

            CommonSteps.ScenarioContext.Set<VillaDTO>(villaDTO);
        }
    }
}
