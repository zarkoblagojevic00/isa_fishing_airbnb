﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Entities.Abstractions;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Services;
using Services.Constants;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VillaManagementController : AdvancedController
    {
        public VillaManagementController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] {false, UserType.VillaOwner})]
        public IActionResult GetVillaInfo(int villaId)
        {
            if (!CheckOwnerShip(villaId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var service = UoW.GetRepository<IServiceReadRepository>().GetById(villaId);
            var additionalServiceInfo = UoW.GetRepository<IAdditionalVillaServiceInfoReadRepository>()
                .GetAll()
                .First(x => x.ServiceId == villaId);
            var images = UoW.GetRepository<IImageReadRepository>().GetAll()
                .Where(x => x.ServiceId == villaId)
                .Select(x => x.ImageId);
            
            return Ok(new VillaDTO()
            {
                VillaId = villaId,
                Name = service.Name,
                PricePerDay = service.PricePerDay,
                Address = service.Address,
                Longitude = service.Longitude,
                Latitude = service.Latitude,
                PromoDescription = service.PromoDescription,
                TermsOfUse = service.TermsOfUse,
                AdditionalEquipment = service.AdditionalEquipment,
                AvailableFrom = service.AvailableFrom,
                AvailableTo = service.AvailableTo,
                Capacity = service.Capacity,
                IsPercentageTakenFromCanceledReservations = service.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = service.PercentageToTake,
                NumberOfBeds = additionalServiceInfo.NumberOfBeds,
                NumberOfRooms = additionalServiceInfo.NumberOfRooms,
                ImageIds = images
            });
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.VillaOwner })]
        public IEnumerable<VillaDTO> GetOwnedVillas()
        {
            var ownerId = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId]);

            var villas = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == ownerId);
            var additionalInformation = UoW.GetRepository<IAdditionalVillaServiceInfoReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(villas.Select(y => y.ServiceId).ToArray()));


            var result = villas.Join(additionalInformation, x => x.ServiceId, y => y.ServiceId, (x, y) => new VillaDTO()
            {
                AdditionalEquipment = x.AdditionalEquipment,
                Address = x.Address,
                AvailableFrom = x.AvailableFrom,
                AvailableTo = x.AvailableTo,
                Capacity = x.Capacity,
                IsPercentageTakenFromCanceledReservations = x.IsPercentageTakenFromCanceledReservations,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Name = x.Name,
                NumberOfBeds = y.NumberOfBeds,
                NumberOfRooms = y.NumberOfRooms,
                PercentageToTake = x.PercentageToTake,
                PricePerDay = x.PricePerDay,
                PromoDescription = x.PromoDescription,
                TermsOfUse = x.PromoDescription,
                VillaId = x.ServiceId,
                ImageIds = UoW.GetRepository<IImageReadRepository>().GetAll().Where(z => z.ServiceId == x.ServiceId).Select(z => z.ImageId)
            });

            return result;
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

            var existingVilla = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == newVilla.Name && x.OwnerId == GetUserIdFromCookie());

            if (existingVilla != null)
            {
                ModelState.AddModelError("Name", "The service with that name for this user already exists!");
                return BadRequest(ModelState);
            }

            try
            {
                var currentUser = GetUserIdFromCookie();
                var villa = CreateNewVilla(currentUser, newVilla);
                var villaWriteRepo = UoW.GetRepository<IServiceWriteRepository>();

                var additionalVillaInfoWriteRepo = UoW.GetRepository<IAdditionalVillaServiceInfoWriteRepository>();

                UoW.BeginTransaction();

                villaWriteRepo.Add(villa);

                var additionalInfo = CreateAdditionalVillaServiceInfo(villa.ServiceId, newVilla);
                additionalVillaInfoWriteRepo.Add(additionalInfo);

                UoW.Commit();

                return Ok(villa.ServiceId);
            }
            catch (Exception e)
            {
                UoW.Rollback();
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

            if (!CheckOwnerShip(villa.VillaId.Value))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == villa.VillaId && !x.IsCanceled);

            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            var villaService = UoW.GetRepository<IServiceReadRepository>().GetById(villa.VillaId.Value);
            var additionalVillaInfo = UoW.GetRepository<IAdditionalVillaServiceInfoReadRepository>()
                .GetAll().First(x => x.ServiceId == villa.VillaId.Value);

            MapNewInformation(villa, villaService, additionalVillaInfo);

            UoW.BeginTransaction();

            UoW.GetRepository<IServiceWriteRepository>().Update(villaService);
            UoW.GetRepository<IAdditionalVillaServiceInfoWriteRepository>().Update(additionalVillaInfo);

            UoW.Commit();

            return Ok(Responses.Ok);
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.VillaOwner })]
        public IActionResult DeleteVilla(int villaId)
        {
            if (!CheckOwnerShip(villaId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && !x.IsCanceled && x.ServiceId == villaId);
            
            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            try
            {
                UoW.BeginTransaction();

                var additionalServiceInfo = UoW.GetRepository<IAdditionalVillaServiceInfoReadRepository>().GetAll()
                    .First(x => x.ServiceId == villaId);
                var service = UoW.GetRepository<IServiceReadRepository>().GetById(villaId);

                var promoActions = UoW.GetRepository<IPromoActionReadRepository>()
                    .GetAll()
                    .Where(x => x.ServiceId == villaId);
                foreach (var promoAction in promoActions)
                {
                    promoAction.IsTaken = true;
                    UoW.GetRepository<IPromoActionWriteRepository>().Update(promoAction);
                }

                UoW.GetRepository<IAdditionalVillaServiceInfoWriteRepository>().Delete(additionalServiceInfo);
                UoW.GetRepository<IServiceWriteRepository>().Delete(service);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

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

        private void MapNewInformation(VillaDTO villa, Service villaService, AdditionalVillaServiceInfo additionalVillaServiceInfo)
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
