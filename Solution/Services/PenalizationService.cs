using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Services
{
    public class PenalizationService
    {
        private readonly IUnitOfWork _uow;

        public PenalizationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void PenalizeUser(int reservationId)
        {
            var correspondingReservation = _uow.GetRepository<IReservationReadRepository>()
                .GetById(reservationId);

            var correspondingUser = _uow.GetRepository<IFurtherClientInfoReadRepository>()
                .GetAll()
                .First(x => x.UserId == correspondingReservation.UserId);

            correspondingUser.NumberOfPenalties += 1;
        }
    }
}
