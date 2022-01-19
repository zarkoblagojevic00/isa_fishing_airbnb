using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests.Base
{
    [Collection("IntegrationTests")]
    public abstract class BaseTest : IClassFixture<BaseFixture>
    {
        private readonly BaseFixture _fixture;

        protected BaseTest(BaseFixture fixture)
        {
            _fixture = fixture;
        }

        public IUnitOfWork UoW => _fixture.UoW;
        public HttpClient Client => _fixture.Client;
        public CookieContainer CookieContainer => _fixture.CookieContainer;
        public string BaseUrl => "https://localhost:44383/api/";

        public void AddCookie(string name, string value, string domain)
        {
            CookieContainer.Add(new Cookie(name, value) { Domain = domain });
        }

        public StringContent GetContent(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content, settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }), Encoding.UTF8, "application/json");
        }

        public User InsertUserInfo(string email, UserType type)
        {
            var user = UoW.GetRepository<IUserReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Email == email);
            UoW.BeginTransaction();
            if (user == null)
            {
                user = new User()
                {
                    Email = email,
                    UserType = type,
                    IsAccountActive = true,
                    IsAccountVerified = true,
                };
                UoW.GetRepository<IUserWriteRepository>().Add(user);
            }
            else if (!user.IsAccountActive || !user.IsAccountVerified || user.UserType != type)
            {
                user.UserType = type;
                user.IsAccountActive = true;
                user.IsAccountVerified = true;
                UoW.GetRepository<IUserWriteRepository>().Update(user);
            }
            UoW.Commit();
            return user;
        }

        public Service InsertServiceInfo(string name, int ownerId)
        {
            var service = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.OwnerId == ownerId && x.Name == name);

            UoW.BeginTransaction();

            if (service == null)
            {
                service = new Service()
                {
                    OwnerId = ownerId,
                    Name = name,
                };
                UoW.GetRepository<IServiceWriteRepository>()
                    .Add(service);
            }

            UoW.Commit();
            return service;
        }

        public void ClearAllActionsAndReservationsForService(int serviceId)
        {
            UoW.BeginTransaction();
            var allReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId);
            foreach (var res in allReservations)
            {
                UoW.GetRepository<IReservationWriteRepository>()
                    .Delete(res);
            }

            var allQuickActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId);
            foreach (var quickAc in allQuickActions)
            {
                UoW.GetRepository<IPromoActionWriteRepository>()
                    .Delete(quickAc);
            }
            UoW.Commit();
        }

        public Mark InsertMarkInfo()
        {
            Mark mark = new Mark
            {
                IsApproved = false,
                IsReviewed = false,
                Description = "test",
                ServiceId = 1,
                GivenMark = 5,
                UserId = 1, 
            };
            UoW.BeginTransaction();
            UoW.GetRepository<IMarkWriteRepository>().Add(mark);
            UoW.Commit();
            return mark;
        }
    }
}
