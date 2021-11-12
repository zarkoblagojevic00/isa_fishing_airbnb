using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Base;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CityController : AdvancedController
    {
        public CityController(IUnitOfWork uow) : base(uow)
        { }

        [HttpGet]
        public IEnumerable<City> GetCities()
        {
            var cityReadRepo = UoW.GetRepository<ICityReadRepository>();

            return cityReadRepo.GetAll();
        }
    }
}
