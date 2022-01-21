using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using API.Mappers;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
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
    public class AdventureController : AdvancedController
    {
        public AdventureController(IUnitOfWork uow) : base(uow)
        { }


        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true, UserType.Instructor })]
        public IActionResult AddAdventure(AdventureDTO adventure)
        {
            var ownerId = GetUserIdFromCookie();
            if(adventure == null)
            {
                return BadRequest("No adventure passed.");
            }
            adventure.OwnerId = ownerId;

            var city = UoW.GetRepository<ICityReadRepository>()
                .GetAll()
                .Where(ci => ci.Name == adventure.CityName)
                .FirstOrDefault();

            if(city == null)
            {
                return BadRequest("City not found.");
            }

            var serviceRepository = UoW.GetRepository<IServiceWriteRepository>();
            var adventureAdditionalInfoRepository = UoW.GetRepository<IAdditionalAdventureInfoWriteRepository>();

            var existingAdventure = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == adventure.Name && x.OwnerId == ownerId);

            if (existingAdventure != null)
            {
                ModelState.AddModelError("Name", "The service with that name for this user already exists!");
                return BadRequest(ModelState);
            }

            try
            {
                UoW.BeginTransaction();
                int serviceId = serviceRepository.AddAndGetInsertedId(adventure.ToModel(city));
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
        public IActionResult GetAdventureInfoById(int adventureId)
        {
            var adventure = SafeGetAdventureById(adventureId);

            if (adventure == null)
            {
                return BadRequest("Adventure not found");
            }

            var city = UoW.GetRepository<ICityReadRepository>().GetById(adventure.CityId);

            if (city == null)
            {
                return BadRequest("City not found.");
            }

            var additionalInformation = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>()
                .GetAll()
                .Where(x => x.AdventureId == adventureId).FirstOrDefault();

            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == adventureId);

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetById(adventure.OwnerId);

            var adventureInfo = new AdventureDTO
            {
                Name = adventure.Name,
                PricePerDay = adventure.PricePerDay,
                Address = adventure.Address,
                Longitude = adventure.Longitude,
                Latitude = adventure.Latitude,
                AvailableFrom = adventure.AvailableFrom,
                AvailableTo = adventure.AvailableTo,
                PromoDescription = adventure.PromoDescription,
                TermsOfUse = adventure.TermsOfUse,
                AdditionalEquipment = adventure.AdditionalEquipment,
                Capacity = adventure.Capacity,
                IsPercentageTakenFromCanceledReservations = adventure.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = adventure.PercentageToTake,
                OwnerId = adventure.OwnerId,
                AdditionalOffers = additionalInformation.AdditionalOffers,
                AdventureId = adventure.ServiceId,
                ShortInstructorBiography = additionalInstructorInfo.ShortBiography,
                CityName = city.Name,
            };

            adventureInfo.ImageIds = images.Select(x => x.ImageId);

            return Ok(adventureInfo);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true, UserType.Instructor })]
        public IActionResult GetAdventureByName(string name)
        {
            var ownerId = GetUserIdFromCookie();

            var adventure = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(adv => adv.Name == name)
                .Where(adv => adv.OwnerId == ownerId)
                .FirstOrDefault();

            if(adventure == null)
            {
                return BadRequest("No adventure with such name for user.");
            }

            return Ok(adventure);
        }

        [HttpGet]
        public IEnumerable<AdventureDTO> GetAllAdventures()
        {
            var adventures = UoW.GetRepository<IServiceReadRepository>()
                .GetAll();

            var additionalInformation = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>()
                .GetAll()
                .Where(x => x.AdventureId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>()
                .GetAll();

            var cities = UoW.GetRepository<ICityReadRepository>().GetAll();

            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var allAdventures = adventures
                .Join(additionalInformation, ad => ad.ServiceId, ai => ai.AdventureId, (ad, ai) => (ad, ai))
                .Join(additionalInstructorInfo, adi => adi.ad.OwnerId, aii => aii.UserId, (adi, aii) => (adi, aii))
                .Join(cities, adii => adii.adi.ad.CityId, c => c.CityId, (adii, c) => (adii, c))
                .Select(information => new AdventureDTO
            {                                
                Name = information.adii.adi.ad.Name,
                PricePerDay = information.adii.adi.ad.PricePerDay,
                Address = information.adii.adi.ad.Address,
                Longitude = information.adii.adi.ad.Longitude,
                Latitude = information.adii.adi.ad.Latitude,
                AvailableFrom = information.adii.adi.ad.AvailableFrom,
                AvailableTo = information.adii.adi.ad.AvailableTo,
                PromoDescription = information.adii.adi.ad.PromoDescription,
                TermsOfUse = information.adii.adi.ad.TermsOfUse,
                AdditionalEquipment = information.adii.adi.ad.AdditionalEquipment,
                Capacity = information.adii.adi.ad.Capacity,
                IsPercentageTakenFromCanceledReservations = information.adii.adi.ad.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = information.adii.adi.ad.PercentageToTake,
                OwnerId = information.adii.adi.ad.OwnerId,
                AdditionalOffers = information.adii.adi.ai.AdditionalOffers,
                AdventureId = information.adii.adi.ad.ServiceId,
                ShortInstructorBiography = information.adii.aii.ShortBiography,
                ImageIds = images.Where(x => x.ServiceId == information.adii.adi.ai.AdventureId).Select(img => img.ImageId),
                CityName = information.c.Name,
            });

            return allAdventures;
        }

        [HttpGet]
        public IActionResult SearchAdventures(
            [FromQuery] ServiceSearchParametersDTO searchParams)
        {
            int userId = -1;
            try
            {
                userId = GetUserIdFromCookie();
            }
            catch (Exception ignore) { };

            ServiceSearchParameters @params = new()
            {
                ServiceName = searchParams.Name ?? "",
                LocationName = searchParams.Location ?? "",
                DateRange = new CalendarItem() { StartDateTime = searchParams.FromDate, EndDateTime = searchParams.ToDate },
                PriceRange = new PriceRange(searchParams.FromPrice, searchParams.ToPrice),
                GivenMark = searchParams.GivenMark,
                Capacity = searchParams.Capacity,
            };

            var potentiallyAvailableServices = new ServiceFinder(ServiceType.Adventure, UoW).FindServices(@params, userId);
            
            var ownerAvailabilityService = new UserUnavailabilityValidationService(UoW);
            var availableServices = potentiallyAvailableServices
                .Select(fut => fut.Service)
                .Where(s => ownerAvailabilityService.IsInstructorAvailable(s.OwnerId, searchParams.FromDate, searchParams.ToDate));

            var additionalInformation = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>().GetAll();
            
            var result = availableServices.Join(additionalInformation, x => x.ServiceId, y => y.AdventureId, (x, y) => new AdventureDTO()
            {
                AdditionalEquipment = x.AdditionalEquipment,
                CityName = UoW.GetRepository<ICityReadRepository>().GetById(x.CityId).Name,
                Address = x.Address,
                AvailableFrom = x.AvailableFrom,
                AvailableTo = x.AvailableTo,
                Capacity = x.Capacity,
                IsPercentageTakenFromCanceledReservations = x.IsPercentageTakenFromCanceledReservations,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Name = x.Name,
                PercentageToTake = x.PercentageToTake,
                PricePerDay = x.PricePerDay,
                PromoDescription = x.PromoDescription,
                TermsOfUse = x.PromoDescription,
                AdventureId = x.ServiceId,
                ShortInstructorBiography = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetById(x.OwnerId).ShortBiography,
                ImageIds = UoW.GetRepository<IImageReadRepository>().GetAll().Where(z => z.ServiceId == x.ServiceId).Select(z => z.ImageId),
                AverageMark = new AverageMarkCalculator(UoW).CalculateAverageMark(x.ServiceId),
            });
            return Ok(result);
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

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>()
                .GetAll();

            var cities = UoW.GetRepository<ICityReadRepository>().GetAll();

            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var allAdventures = adventures
                .Join(additionalInformation, ad => ad.ServiceId, ai => ai.AdventureId, (ad, ai) => (ad, ai))
                .Join(additionalInstructorInfo, adi => adi.ad.OwnerId, aii => aii.UserId, (adi, aii) => (adi, aii))
                .Join(cities, adii => adii.adi.ad.CityId, c => c.CityId, (adii, c) => (adii, c))
                .Select(information => new AdventureDTO
                {
                    Name = information.adii.adi.ad.Name,
                    PricePerDay = information.adii.adi.ad.PricePerDay,
                    Address = information.adii.adi.ad.Address,
                    Longitude = information.adii.adi.ad.Longitude,
                    Latitude = information.adii.adi.ad.Latitude,
                    AvailableFrom = information.adii.adi.ad.AvailableFrom,
                    AvailableTo = information.adii.adi.ad.AvailableTo,
                    PromoDescription = information.adii.adi.ad.PromoDescription,
                    TermsOfUse = information.adii.adi.ad.TermsOfUse,
                    AdditionalEquipment = information.adii.adi.ad.AdditionalEquipment,
                    Capacity = information.adii.adi.ad.Capacity,
                    IsPercentageTakenFromCanceledReservations = information.adii.adi.ad.IsPercentageTakenFromCanceledReservations,
                    PercentageToTake = information.adii.adi.ad.PercentageToTake,
                    OwnerId = information.adii.adi.ad.OwnerId,
                    AdditionalOffers = information.adii.adi.ai.AdditionalOffers,
                    AdventureId = information.adii.adi.ad.ServiceId,
                    ShortInstructorBiography = information.adii.aii.ShortBiography,
                    ImageIds = images.Where(x => x.ServiceId == information.adii.adi.ai.AdventureId).Select(img => img.ImageId),
                    CityName = information.c.Name,
                });

            return allAdventures;
        }


        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IEnumerable<AdventureDTO> FilterOwnedAdventures(string name)
        {
            int ownerId = GetUserIdFromCookie();
            var adventures = UoW.GetRepository<IServiceReadRepository>()
                .GetAll();

            var additionalInformation = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>()
                .GetAll()
                .Where(x => x.AdventureId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>()
                .GetAll();

            var cities = UoW.GetRepository<ICityReadRepository>().GetAll();

            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(adventures.Select(y => y.ServiceId).ToArray()));

            var allAdventures = adventures
                .Join(additionalInformation, ad => ad.ServiceId, ai => ai.AdventureId, (ad, ai) => (ad, ai))
                .Join(additionalInstructorInfo, adi => adi.ad.OwnerId, aii => aii.UserId, (adi, aii) => (adi, aii))
                .Join(cities, adii => adii.adi.ad.CityId, c => c.CityId, (adii, c) => (adii, c))
                .Select(information => new AdventureDTO
                {
                    Name = information.adii.adi.ad.Name,
                    PricePerDay = information.adii.adi.ad.PricePerDay,
                    Address = information.adii.adi.ad.Address,
                    Longitude = information.adii.adi.ad.Longitude,
                    Latitude = information.adii.adi.ad.Latitude,
                    AvailableFrom = information.adii.adi.ad.AvailableFrom,
                    AvailableTo = information.adii.adi.ad.AvailableTo,
                    PromoDescription = information.adii.adi.ad.PromoDescription,
                    TermsOfUse = information.adii.adi.ad.TermsOfUse,
                    AdditionalEquipment = information.adii.adi.ad.AdditionalEquipment,
                    Capacity = information.adii.adi.ad.Capacity,
                    IsPercentageTakenFromCanceledReservations = information.adii.adi.ad.IsPercentageTakenFromCanceledReservations,
                    PercentageToTake = information.adii.adi.ad.PercentageToTake,
                    OwnerId = information.adii.adi.ad.OwnerId,
                    AdditionalOffers = information.adii.adi.ai.AdditionalOffers,
                    AdventureId = information.adii.adi.ad.ServiceId,
                    ShortInstructorBiography = information.adii.aii.ShortBiography,
                    ImageIds = images.Where(x => x.ServiceId == information.adii.adi.ai.AdventureId).Select(img => img.ImageId),
                    CityName = information.c.Name,
                });

            return allAdventures.Where(adventure => String.IsNullOrEmpty(name) || adventure.Name.Contains(name));
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

            var city = UoW.GetRepository<ICityReadRepository>()
                .GetAll()
                .Where(c => c.Name == adventure.CityName)
                .FirstOrDefault();

            if(city == null)
            {
                return BadRequest("City not found");
            }

            MapNewInformation(adventure, existingAdventure, additionalAdventureInfo, city.CityId);

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

        [HttpGet]
        public IActionResult GetAddressInfoByAdventureId(int adventureId)
        {
            var adventure = SafeGetAdventureById(adventureId);

            if(adventure == null)
            {
                return NotFound();
            }

            AddresInfoDTO addressInfo = new AddresInfoDTO() { Address = adventure.Address, Latitude = adventure.Latitude, Longitude = adventure.Longitude };

            return Ok(addressInfo);
            
        }

        [HttpGet]
        public IActionResult GetBasicInfo(int adventureId)
        {
            var adventure = SafeGetAdventureById(adventureId);

            if (adventure == null)
            {
                return NotFound();
            }

            var userInfo = UoW.GetRepository<IUserReadRepository>().GetById(adventure.OwnerId);
            var city = UoW.GetRepository<ICityReadRepository>().GetById(userInfo.CityId);
            var country = UoW.GetRepository<ICountryReadRepository>().GetById(city.CountryId);

            var dto = new UserInfoDTO()
            {
                Name = userInfo.Name,
                Surname = userInfo.Surname,
                Address = userInfo.Address,
                Phone = userInfo.PhoneNumber,
                Email = userInfo.Email,
                City = city.Name,
                //Country = country.Name
            };

            return Ok(dto);
        }


        private void MapNewInformation(AdventureDTO adventureDTO, Service adventure, AdditionalAdventureInfo additionalAdventureInfo, int cityId)
        {
            adventure.Name = adventureDTO.Name;
            adventure.PricePerDay = adventureDTO.PricePerDay;
            adventure.Address = adventureDTO.Address;
            adventure.Longitude = adventureDTO.Longitude;
            adventure.Latitude = adventureDTO.Latitude;
            adventure.PromoDescription = adventureDTO.PromoDescription;
            adventure.TermsOfUse = adventureDTO.TermsOfUse;
            adventure.AdditionalEquipment = adventureDTO.AdditionalEquipment;
            adventure.AdditionalEquipment = adventureDTO.AdditionalEquipment;
            adventure.Capacity = adventureDTO.Capacity;
            adventure.IsPercentageTakenFromCanceledReservations = adventureDTO.IsPercentageTakenFromCanceledReservations;
            adventure.PercentageToTake = adventureDTO.PercentageToTake;
            adventure.CityId = cityId;

            additionalAdventureInfo.AdditionalOffers = adventureDTO.AdditionalOffers;
        }

        private Service SafeGetAdventureById(int adventureId)
        {
            return UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(s => s.ServiceType == ServiceType.Adventure)
                .FirstOrDefault(a => a.ServiceId == adventureId);
        }
    }
}
