using Domain.Repositories;
using Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AverageMarkCalculator
    {
        private IUnitOfWork UoW;

        public AverageMarkCalculator(IUnitOfWork uoW)
        {
            UoW = uoW;
        }

        public double CalculateAverageMark(int serviceId)
        {
            return (double)UoW.GetRepository<IMarkReadRepository>()
                                .GetAll()
                                .Where(mark => mark.ServiceId == serviceId)
                                .Select(mark => mark.GivenMark)
                                .DefaultIfEmpty(0.0)
                                .Average();
        }
    }
}
