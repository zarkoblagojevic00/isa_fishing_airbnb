using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AdminRequestsService
    {
        private readonly IUnitOfWork UoW;
        public AdminRequestsService(IUnitOfWork uow)
        {
            UoW = uow;
        }

        public bool CanUserBeDeleted(int userId)
        {
            var user = UoW.GetRepository<IUserReadRepository>().GetById(userId);

            if (user == null)
                return false;

            var services = UoW.GetRepository<IServiceReadRepository>().GetAll()
                            .Where(service => service.OwnerId == userId);

            var userReservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.UserId == userId && !x.IsCanceled);

            var ownerReservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && services.Any(service => service.ServiceId == x.ServiceId) && !x.IsCanceled);

            var connectedPromoActions = UoW.GetRepository<IPromoActionReadRepository>()
                    .GetAll()
                    .Where(act => services.Any(ser => ser.ServiceId == act.ServiceId) && act.IsTaken)
                    .Where(act => act.StartDateTime > DateTime.Now);



            if (userReservationDates.Any() || ownerReservationDates.Any() || connectedPromoActions.Any() || user.UserType == UserType.Admin)
            {
                return false;
            }

            return true;
        }

        public void DeleteRequestedUser(User user)
        {
            var connectedServices = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(ser => ser.OwnerId == user.UserId);

            var connectedPromoActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(act => connectedServices.Any(ser => ser.ServiceId == act.ServiceId))
                .Where(act => act.StartDateTime > DateTime.Now);

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetAll()
                .FirstOrDefault(x => x.UserId == user.UserId);

            if (additionalInstructorInfo != null)
                UoW.GetRepository<IAdditionalInstructorInfoWriteRepository>().Delete(additionalInstructorInfo);

            if (connectedServices.Any())
            {
                foreach(var service in connectedServices)
                {
                    UoW.GetRepository<IServiceWriteRepository>().Delete(service);
                }
            }

            if (connectedPromoActions.Any())
            {
                foreach (var action in connectedPromoActions)
                {
                    UoW.GetRepository<IPromoActionWriteRepository>().Delete(action);
                }
            }

            UoW.GetRepository<IUserWriteRepository>().Delete(user);

        }
    }
}
