using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
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
    [Route("[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly int _maxImageSize;

        public ImageController(IUnitOfWork uow)
        {
            _uow = uow;
            _maxImageSize = uow.GetRepository<ISystemConfigReadRepository>().GetValue<int>("MaxImageSize");
        }

        [HttpGet]
        [ResponseCache(Duration = 259_200)]
        public IActionResult GetImage(int id)
        {
            var image = _uow.GetRepository<IImageReadRepository>().GetById(id);

            return File(image.Bytes, image.GetImageFormat());
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult AddImage(ImageDTO image)
        {
            var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
            var validationResponse = new ValidateServiceOwnerService(_uow).ValidateOwnerShip(image.ServiceId,currentUser);

            if (!validationResponse)
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            try
            {
                _uow.BeginTransaction();

                var newImage = CreateImage(image);

                _uow.GetRepository<IImageWriteRepository>().Add(newImage);

                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                return BadRequest(Responses.WrongImageFormat);
            }

            return Ok(Responses.Ok);
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult DeleteImage(int id)
        {
            var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
            var image = _uow.GetRepository<IImageReadRepository>().GetById(id);

            var validationResponse = new ValidateServiceOwnerService(_uow).ValidateOwnerShip(image.ServiceId, currentUser);
            if (!validationResponse)
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            _uow.BeginTransaction();

            _uow.GetRepository<IImageWriteRepository>().Delete(image);

            _uow.Commit();

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
