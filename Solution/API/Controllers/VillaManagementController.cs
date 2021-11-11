﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Services;
using Services.Constants;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class VillaManagementController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public VillaManagementController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.VillaOwner })]
        public IActionResult CreateVilla(VillaDTO newVilla)
        {
            if (newVilla.IsPercentageTakenFromCanceledReservations)
            {
                if (newVilla.PercentageToTake <= 0)
                {
                    ModelState.AddModelError("PercentageToTake", "In this case the percentage to take has to be greater than 0!");
                }
                else if (newVilla.PercentageToTake > 100)
                {
                    ModelState.AddModelError("PercentageToTake", "The percentage to take cannot be greater than 100!");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            try
            {
                var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
                var villa = CreateNewVilla(currentUser, newVilla);
                var villaWriteRepo = _uow.GetRepository<IServiceWriteRepository>();

                var additionalVillaInfoWriteRepo = _uow.GetRepository<IAdditionalVillaServiceInfoWriteRepository>();

                _uow.BeginTransaction();

                villaWriteRepo.Add(villa);

                var additionalInfo = CreateAdditionalVillaServiceInfo(villa.ServiceId, newVilla);
                additionalVillaInfoWriteRepo.Add(additionalInfo);

                _uow.Commit();

                return Ok(villa.ServiceId);
            }
            catch (Exception e)
            {
                _uow.Rollback();
                return Problem(e.Message);
            }
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.VillaOwner })]
        public IActionResult UpdateVilla(VillaDTO villa)
        {
            if (villa.VillaId == null)
            {
                ModelState.AddModelError("VillaId", "You have to specify the Villa");
                return BadRequest(ModelState);
            }

            var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
            var validationResponse = new ValidateServiceOwnerService(_uow).ValidateOwnerShip(villa.VillaId.Value, currentUser);

            if (!validationResponse)
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var villaService = _uow.GetRepository<IServiceReadRepository>().GetById(villa.VillaId.Value);
            var additionalVillaInfo = _uow.GetRepository<IAdditionalVillaServiceInfoReadRepository>()
                .GetAll().First(x => x.ServiceId == villa.VillaId.Value);

            MapNewInformation(villa, villaService, additionalVillaInfo);

            _uow.BeginTransaction();

            _uow.GetRepository<IServiceWriteRepository>().Update(villaService);
            _uow.GetRepository<IAdditionalVillaServiceInfoWriteRepository>().Update(additionalVillaInfo);

            _uow.Commit();

            return Ok(Responses.Ok);
        }

        private Service CreateNewVilla(int currentUser, VillaDTO newVilla)
        {
            return new Service()
            {
                OwnerId = currentUser,
                ServiceType = ServiceType.Villa,
                Name = newVilla.Name,
                PricePerDay = newVilla.PricePerDay,
                Address = newVilla.Address,
                Longitude = newVilla.Longitude,
                Latitude = newVilla.Latitude,
                PromoDescription = newVilla.PromoDescription,
                TermsOfUse = newVilla.TermsOfUse,
                AdditionalEquipment = newVilla.AdditionalEquipment,
                AvailableFrom = newVilla.AvailableFrom,
                AvailableTo = newVilla.AvailableTo,
                Capacity = newVilla.Capacity,
                IsPercentageTakenFromCanceledReservations = newVilla.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = newVilla.PercentageToTake
            };
        }

        private AdditionalVillaServiceInfo CreateAdditionalVillaServiceInfo(int serviceId, VillaDTO newVilla)
        {
            return new AdditionalVillaServiceInfo()
            {
                ServiceId = serviceId,
                NumberOfBeds = newVilla.NumberOfBeds,
                NumberOfRooms = newVilla.NumberOfRooms
            };
        }

        private void MapNewInformation(VillaDTO villa, Service villaService,
            AdditionalVillaServiceInfo additionalVillaServiceInfo)
        {
            villaService.Name = villa.Name;
            villaService.PricePerDay = villa.PricePerDay;
            villaService.Address = villa.Address;
            villaService.Longitude = villa.Longitude;
            villaService.Latitude = villa.Latitude;
            villaService.PromoDescription = villa.PromoDescription;
            villaService.TermsOfUse = villa.TermsOfUse;
            villaService.AdditionalEquipment = villa.AdditionalEquipment;
            villaService.AvailableFrom = villa.AvailableFrom;
            villaService.AvailableTo = villa.AvailableTo;
            villaService.AdditionalEquipment = villa.AdditionalEquipment;
            villaService.Capacity = villa.Capacity;
            villaService.IsPercentageTakenFromCanceledReservations = villa.IsPercentageTakenFromCanceledReservations;
            villaService.PercentageToTake = villa.PercentageToTake;

            additionalVillaServiceInfo.NumberOfBeds = villa.NumberOfBeds;
            additionalVillaServiceInfo.NumberOfRooms = villa.NumberOfRooms;
        }
    }
}
