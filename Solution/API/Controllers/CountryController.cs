using System;
using API.Controllers.Base;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CountryController : AdvancedController
    {
        public CountryController(IUnitOfWork uow) : base(uow)
        { }

        [HttpPost]
        public IActionResult AddCountry(Country country)
        {
            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<ICountryWriteRepository>().Add(country);
                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
            }
            
            return Ok("success");
        }
    }
}