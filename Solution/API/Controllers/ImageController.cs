using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using API.Extensions;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Services;
using Services.Constants;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImageController : AdvancedController
    {
        private readonly int _maxImageSize;

        public ImageController(IUnitOfWork uow) : base(uow)
        {
            _maxImageSize = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("MaxImageSize");
        }

        [HttpGet]
        [ResponseCache(Duration = 259_200)]
        public IActionResult GetImage(int id)
        {
            var image = UoW.GetRepository<IImageReadRepository>().GetById(id);

            return File(image.Bytes, image.GetImageFormat());
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult AddImage(ImageDTO image)
        {
            if (!CheckOwnerShip(image.ServiceId))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            try
            {
                UoW.BeginTransaction();

                var newImage = CreateImage(image);

                UoW.GetRepository<IImageWriteRepository>().Add(newImage);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest(Responses.WrongImageFormat);
            }

            return Ok(Responses.Ok);
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult DeleteImage(int id)
        {
            var image = UoW.GetRepository<IImageReadRepository>().GetById(id);
            if (image == null)
            {
                return NotFound(Responses.NotFound);
            }
            if (!CheckOwnerShip(image.ServiceId))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            UoW.BeginTransaction();

            UoW.GetRepository<IImageWriteRepository>().Delete(image);

            UoW.Commit();

            return Ok(Responses.Ok);
        }
        
        private Image CreateImage(ImageDTO image)
        {
            return new Image()
            {
                ServiceId = image.ServiceId,
                Bytes = image.ImageAsBase64.GetBytes()
            };
        }
    }
}
