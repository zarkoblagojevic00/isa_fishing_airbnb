using API.Controllers.Base;
using API.DTOs;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Constants;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : AdvancedController
    {
        public AdminController(IUnitOfWork uow) : base(uow)
        {
        }


    }
}
