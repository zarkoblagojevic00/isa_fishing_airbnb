using API.Controllers.Base;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InstructorController : AdvancedController
    {
        public InstructorController(IUnitOfWork uow) : base(uow)
        { }

        [HttpGet]
        public IEnumerable<AdditionalInstructorInfo> GetAllAdditionalInstructorInfos()
        {
            var additionalInstructorInfoReadRepo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>();
            return additionalInstructorInfoReadRepo.GetAll();
        }

        [HttpPost]
        public IActionResult AddAdditionalInstructorInfo(AdditionalInstructorInfo info)
        {
            var additionalInstructorInfoWriteRepo = UoW.GetRepository<IAdditionalInstructorInfoWriteRepository>();

            try
            {
                UoW.BeginTransaction();
                additionalInstructorInfoWriteRepo.Add(info);
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
