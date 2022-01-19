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
    public class BoatTests : BaseTest
    {
        public BoatTests(BaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Should_Be_Able_To_Create_Boat()
        {
            var user = InsertUserInfo("integrationTestBoatOwner", UserType.BoatOwner);
            var boat = new BoatDTO()
            {
                Name = "Integration test boat",
                BoatType = BoatType.Bass,
                Length = 5,
                EngineNum = 5,
                EnginePower = 12,
                Speed = 5,
                NavigationalTools = new int[] { 0, 1 },
                Longitude = 5,
                Latitude = 5,
                PromoDescription = "testing",
                PricePerDay = 5,
                IsPercentageTaken = false,
                PercentageToTake = 0,
                CityName = UoW.GetRepository<ICityReadRepository>().GetAll().First().Name,
                Address = "testing",
                Capacity = 5,
            };
            EnsureNoBoatWithName(boat.Name, user.UserId);

            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");
            var response = Client.PostAsync(BaseUrl + "BoatManagement/CreateBoat", GetContent(boat))
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var foundBoats = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.Name == boat.Name);
            foundBoats.Count().ShouldBe(1);
        }

        [Fact]
        public void Should_Be_Able_To_Delete_Boat()
        {
            var user = InsertUserInfo("integrationTestBoatOwner", UserType.BoatOwner);
            var boat = InsertServiceInfo("Integration test boat", user.UserId);
            ClearAllActionsAndReservationsForService(boat.ServiceId);

            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");
            var response = Client.DeleteAsync(BaseUrl + "BoatManagement/DeleteBoat?boatId=" + boat.ServiceId)
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var foundVillas = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.Name == boat.Name);
            foundVillas.Count().ShouldBe(0);
        }

        private void EnsureNoBoatWithName(string name, int ownerId)
        {
            var foundBoats = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == ownerId && x.Name == name);

            if (foundBoats.Any())
            {
                UoW.BeginTransaction();
                foreach (var foundBoat in foundBoats)
                {
                    foundBoat.Name += Guid.NewGuid();
                    UoW.GetRepository<IServiceWriteRepository>().Update(foundBoat);
                }
                UoW.Commit();
            }
        }
    }
}
