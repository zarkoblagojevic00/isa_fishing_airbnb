using System;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CountryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public IActionResult AddCountry(Country country)
        {
            try
            {
                _uow.BeginTransaction();
                _uow.GetRepository<ICountryWriteRepository>().Add(country);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
            }
            
            return Ok("success");
        }
    }
}