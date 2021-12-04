﻿using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using API.Mappers;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdventureController : AdvancedController
    {
        public AdventureController(IUnitOfWork uow) : base(uow)
        { }


        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true, UserType.Instructor })]
        public IActionResult AddAdventure(AdventureDTO adventure)
        {
            var serviceRepository = UoW.GetRepository<IServiceWriteRepository>();
            var adventureAdditionalInfoRepository = UoW.GetRepository<IAdditionalAdventureInfoWriteRepository>();

            var existingAdventure = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == adventure.Name && x.OwnerId == GetUserIdFromCookie());

            if (existingAdventure != null)
            {
                ModelState.AddModelError("Name", "The service with that name for this user already exists!");
                return BadRequest(ModelState);
            }

            try
            {
                UoW.BeginTransaction();
                int serviceId = serviceRepository.AddAndGetInsertedId(adventure.ToModel());
                adventureAdditionalInfoRepository.Add(adventure.ToModel(serviceId));
                adventure.AdventureId = serviceId;
                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest();
            }

            return Ok(adventure.AdventureId);
        }



        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IEnumerable<AdventureDTO> GetAllOwnedAdventures()
        {
            int ownerId = GetUserIdFromCookie();
            var adventures = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == ownerId);
            var additionalInformation = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>()
                .GetAll()
                .Where(x => x.AdventureId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var ownedAdventures = adventures.Join(additionalInformation, x => x.ServiceId, y => y.AdventureId, (x, y) => new AdventureDTO()
            {
                Name = x.Name,
                PricePerDay = x.PricePerDay,
                Address = x.Address,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                AvailableFrom = x.AvailableFrom,
                AvailableTo = x.AvailableTo,
                PromoDescription = x.PromoDescription,
                TermsOfUse = x.TermsOfUse,
                AdditionalEquipment = x.AdditionalEquipment,
                Capacity = x.Capacity,
                IsPercentageTakenFromCanceledReservations = x.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = x.PercentageToTake,
                OwnerId = x.OwnerId,
                AdditionalOffers = y.AdditionalOffers,
                AdventureId = x.ServiceId,
                ShortInstructorBiography = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetById(x.OwnerId).ShortBiography
            });

            foreach (AdventureDTO adventure in ownedAdventures)
            {
                adventure.ImageIds = images.Where(x => x.ServiceId == adventure.AdventureId.Value)
                    .Select(x => x.ImageId);
            }

            return ownedAdventures;
        }


        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IEnumerable<AdventureDTO> FilterOwnedAdventures(string name, string address)
        {
            int ownerId = GetUserIdFromCookie();
            var adventures = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == ownerId);
            var additionalInformation = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>()
                .GetAll()
                .Where(x => x.AdventureId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var ownedAdventures = adventures.Join(additionalInformation, x => x.ServiceId, y => y.AdventureId, (x, y) => new AdventureDTO()
            {
                Name = x.Name,
                PricePerDay = x.PricePerDay,
                Address = x.Address,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                PromoDescription = x.PromoDescription,
                TermsOfUse = x.TermsOfUse,
                AdditionalEquipment = x.AdditionalEquipment,
                Capacity = x.Capacity,
                IsPercentageTakenFromCanceledReservations = x.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = x.PercentageToTake,
                OwnerId = x.OwnerId,
                AdditionalOffers = y.AdditionalOffers,
                AdventureId = x.ServiceId,
                ShortInstructorBiography = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetById(x.OwnerId).ShortBiography
            });

            foreach (AdventureDTO adventure in ownedAdventures)
            {
                adventure.ImageIds = images.Where(x => x.ServiceId == adventure.AdventureId.Value)
                    .Select(x => x.ImageId);
            }

            return ownedAdventures.Where(adventure => String.IsNullOrEmpty(name) || adventure.Name.Contains(name))
                                  .Where(adventure => String.IsNullOrEmpty(address) || adventure.Address.Contains(address));
        }


        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult UpdateAdventure(AdventureDTO adventure)
        {
            if (adventure.AdventureId == null)
            {
                ModelState.AddModelError("AdventureId", "You have to specify the adventure");
                return BadRequest(ModelState);
            }

            if (!CheckOwnerShip(adventure.AdventureId.Value))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == adventure.AdventureId && !x.IsCanceled);

            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            var existingAdventure = UoW.GetRepository<IServiceReadRepository>().GetById(adventure.AdventureId.Value);
            var additionalAdventureInfo = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>()
                .GetAll().First(x => x.AdventureId == adventure.AdventureId.Value);

            MapNewInformation(adventure, existingAdventure, additionalAdventureInfo);

            try
            {
                UoW.BeginTransaction();

                UoW.GetRepository<IServiceWriteRepository>().Update(existingAdventure);
                UoW.GetRepository<IAdditionalAdventureInfoWriteRepository>().Update(additionalAdventureInfo);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest();
            }

            return Ok(Responses.Ok);
        }


        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult DeleteAdventure(int adventureId)
        {
            if (!CheckOwnerShip(adventureId))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == adventureId && !x.IsCanceled);

            var promoActionDates = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == adventureId && x.IsTaken);

            if (reservationDates.Any() || promoActionDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            try
            {

                UoW.BeginTransaction();

                var additionalServiceInfo = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>().GetAll()
                    .First(x => x.AdventureId == adventureId);

                var service = UoW.GetRepository<IServiceReadRepository>().GetById(adventureId);

                var promoActions = UoW.GetRepository<IPromoActionReadRepository>()
                    .GetAll()
                    .Where(x => x.ServiceId == adventureId);
                foreach (var promoAction in promoActions)
                {
                    promoAction.IsTaken = true;
                    UoW.GetRepository<IPromoActionWriteRepository>().Update(promoAction);
                }

                UoW.GetRepository<IAdditionalAdventureInfoWriteRepository>().Delete(additionalServiceInfo);
                UoW.GetRepository<IServiceWriteRepository>().Delete(service);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest();
            }

            return Ok(Responses.Ok);
        }


        private void MapNewInformation(AdventureDTO adventureDTO, Service adventure, AdditionalAdventureInfo additionalAdventureInfo)
        {
            adventure.Name = adventureDTO.Name;
            adventure.PricePerDay = adventureDTO.PricePerDay;
            adventure.Address = adventureDTO.Address;
            adventure.Longitude = adventureDTO.Longitude;
            adventure.Latitude = adventureDTO.Latitude;
            adventure.PromoDescription = adventureDTO.PromoDescription;
            adventure.TermsOfUse = adventureDTO.TermsOfUse;
            adventure.AdditionalEquipment = adventureDTO.AdditionalEquipment;
            adventure.AvailableFrom = adventureDTO.AvailableFrom;
            adventure.AvailableTo = adventureDTO.AvailableTo;
            adventure.AdditionalEquipment = adventureDTO.AdditionalEquipment;
            adventure.Capacity = adventureDTO.Capacity;
            adventure.IsPercentageTakenFromCanceledReservations = adventureDTO.IsPercentageTakenFromCanceledReservations;
            adventure.PercentageToTake = adventureDTO.PercentageToTake;

            additionalAdventureInfo.AdditionalOffers = adventureDTO.AdditionalOffers;
        }
    }
}