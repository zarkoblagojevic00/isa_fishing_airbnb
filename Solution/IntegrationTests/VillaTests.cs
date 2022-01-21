using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using IntegrationTests.Base;
using Shouldly;
using Xunit;

namespace IntegrationTests
{
    public class VillaTests : BaseTest
    {
        public VillaTests(BaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Should_Be_Able_To_Create_Villa()
        {
            var user = InsertUserInfo("integrationTestVillaOwner", UserType.VillaOwner);
            var villa = new VillaDTO()
            {
                Name = "Integration test villa",
                PricePerDay = 5,
                CityName = UoW.GetRepository<ICityReadRepository>().GetAll().First().Name,
                Address = "test address 123",
                Longitude = 25,
                Latitude = 25,
                PromoDescription = "Integration testing",
                Capacity = 5,
                IsPercentageTakenFromCanceledReservations = false,
                PercentageToTake = 0,
                NumberOfBeds = 4,
                NumberOfRooms = 3
            };

            EnsureNoVillaWithName(villa.Name, user.UserId);

            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");
            var response = Client.PostAsync(BaseUrl + "VillaManagement/CreateVilla", GetContent(villa))
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var foundVillas = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.Name == villa.Name);
            foundVillas.Count().ShouldBe(1);
        }

        [Fact]
        public void Should_Be_Able_To_Delete_Villa()
        {
            var user = InsertUserInfo("integrationTestVillaOwner", UserType.VillaOwner);
            var villa = InsertServiceInfo("Integration test villa", user.UserId);
            ClearAllActionsAndReservationsForService(villa.ServiceId);

            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");
            var response = Client.DeleteAsync(BaseUrl + "VillaManagement/DeleteVilla?villaId=" + villa.ServiceId)
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var foundVillas = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.Name == villa.Name);
            foundVillas.Count().ShouldBe(0);
        }
        
        private void EnsureNoVillaWithName(string name, int ownerId)
        {
            var foundVillas = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == ownerId && x.Name == name);

            if (foundVillas.Any())
            {
                UoW.BeginTransaction();
                foreach (var foundVilla in foundVillas)
                {
                    foundVilla.Name += Guid.NewGuid();
                    UoW.GetRepository<IServiceWriteRepository>().Update(foundVilla);
                }
                UoW.Commit();
            }
        }
    }
}
