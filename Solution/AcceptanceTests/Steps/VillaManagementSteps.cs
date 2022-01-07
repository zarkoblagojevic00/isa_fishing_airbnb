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
using FluentNHibernate.Utils;
using Newtonsoft.Json;
using NHibernate;
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

        [Given(@"the villa with the name ""(.*)"" that is linked with test villa owner had no reservations")]
        public void TheVillaThatIsLinkedHadNoReservations(string name)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var owner = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .First(x => x.Email == TestConstants.TestVillaOwnerMail);

            var service = uow.GetRepository<IServiceReadRepository>()
                .GetAll()
                .First(x => x.Name == name && x.OwnerId == owner.UserId);

            var reservations = uow.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == service.ServiceId && !x.IsCanceled);
            foreach (var reservation in reservations)
            {
                reservation.IsCanceled = true;
                uow.GetRepository<IReservationWriteRepository>().Update(reservation);
            }
        }


        [Given(@"a new villa DTO was created with the following properties")]
        public void NewVillaDTOWasCreatedWithFollowingProperties(Table table)
        {
            var villaDTO = table.CreateInstance<VillaDTO>();

            var content = JsonConvert.SerializeObject(villaDTO);

            CommonSteps.ScenarioContext.Set(content, TestConstants.Content);
        }

        [Given(@"an id of the villa named ""(.*)"" which is linked to the test owner is included as path parameter")]
        public void AnIdOfVillaNamedIsIncludedAsPathParameter(string name)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var testVillaOwner = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .First(x => x.Email == TestConstants.TestVillaOwnerMail);

            var service = uow.GetRepository<IServiceReadRepository>()
                .GetAll()
                .First(x => x.Name == name && x.OwnerId == testVillaOwner.UserId);

            var pathParams = CommonSteps.ScenarioContext.Get<string>(TestConstants.PathParam);
            if (string.IsNullOrEmpty(pathParams))
            {
                pathParams = "?villaId=" + service.ServiceId;
            }
            else
            {
                pathParams += "&villaId=" + service.ServiceId;
            }

            CommonSteps.ScenarioContext.Set(pathParams, TestConstants.PathParam);
        }

        [Given(@"a new villa DTO was formed based on the villa from the database that is linked with test owner")]
        public void AVillaWasCreatedWithPropertiesFromTheDatabase(Table table)
        {
            var villaDTO = table.CreateInstance<VillaDTO>();
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();
            
            var owner = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .First(x => x.Email == TestConstants.TestVillaOwnerMail);

            var service = uow.GetRepository<IServiceReadRepository>()
                .GetAll()
                .First(x => x.Name == villaDTO.Name && x.OwnerId == owner.UserId);

            villaDTO.VillaId = service.ServiceId;

            var content = JsonConvert.SerializeObject(villaDTO);

            CommonSteps.ScenarioContext.Set(content, TestConstants.Content);
        }

        [Then(@"a villa with the name ""(.*)"" will be created and will be owned by ""(.*)""")]
        public void VillaWillBeCreatedAndWillBeOwnedBy(string villaName, string ownerEmail)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var user = uow.GetRepository<IUserReadRepository>().GetAll().FirstOrDefault(x => x.Email == ownerEmail);

            if (user == null)
            {
                throw new ObjectNotFoundByUniqueKeyException("user", "email", ownerEmail);
            }

            var villa = uow.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == villaName && x.OwnerId == user.UserId);

            villa.Should().NotBeNull();
        }

        [Then(@"the villa with the name ""(.*)"" will be deleted for the owner ""(.*)""")]
        public void VillaWillBeDeleted(string name, string email)
        {
            var uow = CommonSteps.FeatureContext.Get<ILifetimeScope>().Resolve<IUnitOfWork>();

            var user = uow.GetRepository<IUserReadRepository>()
                .GetAll()
                .First(x => x.Email == email);

            var service = uow.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.OwnerId == user.UserId && x.Name == name);

            service.Should().BeNull();
        }
    }
}
