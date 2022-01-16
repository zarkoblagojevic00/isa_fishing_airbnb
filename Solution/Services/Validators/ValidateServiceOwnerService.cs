using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Services
{
    public class ValidateServiceOwnerService
    {
        private readonly IUnitOfWork _uow;

        public ValidateServiceOwnerService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public bool ValidateOwnerShip(Service service, User owner)
        {
            return CheckCorrelatingTypes(service.ServiceType, owner.UserType) && service.OwnerId == owner.UserId;
        }

        public bool ValidateOwnerShip(int serviceId, int ownerId)
        {
            var service = _uow.GetRepository<IServiceReadRepository>().GetById(serviceId);
            var owner = _uow.GetRepository<IUserReadRepository>().GetById(ownerId);

            return service != null && owner != null && ValidateOwnerShip(service, owner);
        }

        private bool CheckCorrelatingTypes(ServiceType serviceType, UserType userType)
        {
            if (serviceType == ServiceType.Adventure && userType == UserType.Instructor)
                return true;
            if (serviceType == ServiceType.Boat && userType == UserType.BoatOwner)
                return true;
            if (serviceType == ServiceType.Villa && userType == UserType.VillaOwner)
                return true;

            return false;
        }
    }
}
