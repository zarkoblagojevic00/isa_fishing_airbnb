using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly IUnitOfWork _uoW;

        public CityController(IUnitOfWork UoW)
        {
            _uoW = UoW;
        }

        [HttpGet]
        public IEnumerable<City> GetCities()
        {
            var cityReadRepo = _uoW.GetRepository<ICityReadRepository>();

            return cityReadRepo.GetAll();
        }
    }
}
