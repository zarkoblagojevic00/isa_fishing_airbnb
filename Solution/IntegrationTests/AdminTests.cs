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
    public class AdminTests : BaseTest
    {
        public AdminTests(BaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Should_Be_Able_To_Update_Money_Percentage_System_Takes()
        {
            var user = InsertUserInfo("integrationTestAdmin", UserType.Admin);
            var moneyPercentageItem = new MoneyPercentageSystemMakesDTO
            {
                Name = "MoneyPercentageSystemTakes",
                Value = "13",
            };
            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");

            var response = Client.PutAsync(BaseUrl + "Admin/UpdateMoneyPercentageSystemTakes", GetContent(moneyPercentageItem))
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var systemConfig = UoW.GetRepository<ISystemConfigReadRepository>()
                .GetAll()
                .Where(con => con.Name == "MoneyPercentageSystemTakes")
                .First();

            systemConfig.Value.ShouldBe("13");
        }

        [Fact]
        public void Should_Be_Able_To_Accept_Mark_Request()
        {
            var admin = InsertUserInfo("integrationTestAdmin", UserType.Admin);
            var mark = InsertMarkInfo();
            var markRequest = new UserMarkInformationDTO
            {
                Mark = 5.0,
                Email = "maraipera1@gmail.com",
                MarkId = mark.MarkId
            };
            AddCookie("userId", admin.UserId.ToString(), "localhost");
            AddCookie("email", admin.Email, "localhost");

            var response = Client.PutAsync(BaseUrl + "Admin/ApproveMarkRequest", GetContent(markRequest))
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);
            var updatedMark = UoW.GetRepository<IMarkReadRepository>().GetById(mark.MarkId);

            updatedMark.IsApproved.ShouldBe(true);
        }

        [Fact]
        public void Should_Be_Able_To_Delete_Service()
        {
            var user = InsertUserInfo("integrationTestAdmin", UserType.Admin);
            var instructor = InsertUserInfo("integrationTestInstructor", UserType.Instructor);
            var service = InsertServiceInfo("testService", instructor.UserId);
            ClearAllActionsAndReservationsForService(service.ServiceId);

            AddCookie("userId", user.UserId.ToString(), "localhost");
            AddCookie("email", user.Email, "localhost");

            var response = Client.DeleteAsync(BaseUrl + "Admin/DeleteService?serviceId=" + service.ServiceId.ToString())
                .GetAwaiter()
                .GetResult();

            response.IsSuccessStatusCode.ShouldBe(true);

            var services = UoW.GetRepository<IServiceReadRepository>().GetAll()
                .Where(ser => ser.Name == "testService");

            services.Count().ShouldBe(0);

        }
    }
}
