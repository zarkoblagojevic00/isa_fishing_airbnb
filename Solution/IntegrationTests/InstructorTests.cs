using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using IntegrationTests.Base;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class InstructorTests : BaseTest
    {
        public InstructorTests(BaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Should_Be_Able_To_Add_UnavailabilityPeriod()
        {
            var user = InsertUserInfo("integrationTestInstructorUnvlbl", UserType.Instructor);

            ClearUserUnavailabilities(user.UserId);

            var availabilityPeriod = new UserAvailabilityPeriodDTO
            {
                UserId = user.UserId,
                PeriodStart = DateTime.Now.AddDays(3),
                PeriodEnd = DateTime.Now.AddDays(5),
            };
            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");

            var response = Client.PostAsync(BaseUrl + "Instructor/AddInstructorAvailabilityPeriod", GetContent(availabilityPeriod))
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var userAvailabilities = UoW.GetRepository<IUserAvailabilityReadRepository>()
                .GetAll()
                .Where(av => av.UserId == user.UserId);

            userAvailabilities.Count().ShouldBe(1);
        }


        [Fact]
        public void Should_Not_Be_Able_To_Add_UnavailabilityPeriod()
        {
            var user = InsertUserInfo("integrationTestInstructorUnvlblNeg", UserType.Instructor);

            ClearUserUnavailabilities(user.UserId);
            InsertUnavailabilityPeriod(user.UserId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4));

            var availabilityPeriod = new UserAvailabilityPeriodDTO
            {
                UserId = user.UserId,
                PeriodStart = DateTime.Now.AddDays(3),
                PeriodEnd = DateTime.Now.AddDays(5),
            };
            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");

            var response = Client.PostAsync(BaseUrl + "Instructor/AddInstructorAvailabilityPeriod", GetContent(availabilityPeriod))
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBeFalse();

            var userAvailabilities = UoW.GetRepository<IUserAvailabilityReadRepository>()
                .GetAll()
                .Where(av => av.UserId == user.UserId && av.PeriodStart == availabilityPeriod.PeriodStart && av.PeriodEnd == availabilityPeriod.PeriodEnd);

            userAvailabilities.Count().ShouldBe(0);
        }
    }
}
