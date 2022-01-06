using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.ConfigurationObjects;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations;
using Services;
using Services.Constants;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BoatManagementController : AdvancedController
    {
        public BoatManagementController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.BoatOwner })]
        public IActionResult GetBoatInfo(int boatId)
        {
            if (!CheckOwnerShip(boatId))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            var service = UoW.GetRepository<IServiceReadRepository>()
                .GetById(boatId);
            var additionalServiceInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>()
                .GetAll()
                .First(x => x.ServiceId == boatId);
            var images = UoW.GetRepository<IImageReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == boatId)
                .Select(x => x.ImageId);
            var navigationTools = UoW.GetRepository<ILinkNavigationBoatReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == boatId)
                .Select(x => x.BoatServiceNavigationToolId);

            return Ok(new BoatDTO()
            {
                BoatId = boatId,
                Name = service.Name,
                BoatType = additionalServiceInfo.BoatType,
                Length = additionalServiceInfo.Length,
                EngineNum = additionalServiceInfo.NumberOfEngines,
                EnginePower = additionalServiceInfo.PowerOfEngines,
                Speed = additionalServiceInfo.MaxSpeed,
                NavigationalTools = navigationTools,
                Longitude = service.Longitude,
                Latitude = service.Latitude,
                PromoDescription = service.PromoDescription,
                ImageIds = images,
                TermsOfUse = service.TermsOfUse,
                AdditionalEquipment = service.AdditionalEquipment,
                PricePerDay = service.PricePerDay,
                IsPercentageTaken = service.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = service.PercentageToTake,
                CityName = UoW.GetRepository<ICityReadRepository>().GetById(service.CityId).Name,
                Address = service.Address,
                Capacity = service.Capacity,
                AvailableFrom = service.AvailableFrom,
                AvailableTo = service.AvailableTo
            });
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.BoatOwner })]
        public IEnumerable<BoatDTO> GetOwnerBoats()
        {
            var boats = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == GetUserIdFromCookie());
            var additionalInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(boats.Select(y => y.ServiceId).ToArray()));

            var result = boats.Join(additionalInfo, x => x.ServiceId, y => y.ServiceId, (x, y) => new BoatDTO()
            {
                Name = x.Name,
                Capacity = x.Capacity,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PricePerDay = x.PricePerDay,
                PromoDescription = x.PromoDescription,
                BoatId = x.ServiceId,
                ImageIds = UoW.GetRepository<IImageReadRepository>()
                    .GetAll()
                    .Where(z => z.ServiceId == x.ServiceId)
                    .Select(z => z.ImageId),
                CityName = UoW.GetRepository<ICityReadRepository>().GetById(x.CityId).Name,
                Address = x.Address,
                AvailableFrom = x.AvailableFrom,
                AvailableTo = x.AvailableTo
            });

            return result;
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.BoatOwner })]
        public IActionResult CreateBoat(BoatDTO newBoat)
        {
            if (newBoat.AvailableFrom != null && newBoat.AvailableTo == null)
            {
                ModelState.AddModelError("AvailableTo", "The ending date for availability needs to be specified!");
                return BadRequest(ModelState);
            }
            if (newBoat.AvailableTo != null && newBoat.AvailableFrom == null)
            {
                ModelState.AddModelError("AvailableFrom", "The beginning date for availability needs to be specified!");
                return BadRequest(ModelState);
            }
            if (newBoat.AvailableFrom >= newBoat.AvailableTo)
            {
                ModelState.AddModelError("AvailableFrom", "Beginning date is greater than ending date!");
                return BadRequest(ModelState);
            }

            var existingBoat = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == newBoat.Name && x.OwnerId == GetUserIdFromCookie());

            if (existingBoat != null)
            {
                ModelState.AddModelError("Name", "The service with that name for this user already exists!");
                return BadRequest(ModelState);
            }

            try
            {
                UoW.BeginTransaction();

                var boat = MapToBoatInfo(newBoat);
                
                UoW.GetRepository<IServiceWriteRepository>().Add(boat);
                
                var additionalBoatInfo = MapToAdditionalBoatInfo(newBoat);
                additionalBoatInfo.ServiceId = boat.ServiceId;
                UoW.GetRepository<IAdditionalBoatServiceInfoWriteRepository>().Add(additionalBoatInfo);

                foreach (var navigationToolId in newBoat.NavigationalTools.Distinct())
                {
                    var navigationTool = new LinkNavigationBoat()
                    {
                        ServiceId = boat.ServiceId,
                        BoatServiceNavigationToolId = navigationToolId
                    };
                    UoW.GetRepository<ILinkNavigationBoatWriteRepository>().Add(navigationTool);
                }

                UoW.Commit();
                return Ok(boat.ServiceId);
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.BoatOwner })]
        public IActionResult UpdateBoat(BoatDTO boat)
        {
            if (boat.BoatId == null)
            {
                ModelState.AddModelError("BoatId", "You have to specify the Boat!");
                return BadRequest(ModelState);
            }

            if (!CheckOwnerShip(boat.BoatId.Value))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            if (boat.AvailableFrom != null && boat.AvailableTo == null)
            {
                ModelState.AddModelError("AvailableTo", "The ending date for availability needs to be specified!");
                return BadRequest(ModelState);
            }
            if (boat.AvailableTo != null && boat.AvailableFrom == null)
            {
                ModelState.AddModelError("AvailableFrom", "The beginning date for availability needs to be specified!");
                return BadRequest(ModelState);
            }
            if (boat.AvailableFrom >= boat.AvailableTo)
            {
                ModelState.AddModelError("AvailableFrom", "Beginning date is greater than ending date!");
                return BadRequest(ModelState);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == boat.BoatId && !x.IsCanceled && !x.IsServiceUnavailableMarker);

            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            var boatService = UoW.GetRepository<IServiceReadRepository>().GetById(boat.BoatId.Value);
            var additionalInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>()
                .GetAll()
                .First(x => x.ServiceId == boat.BoatId);
            
            UoW.BeginTransaction();

            MapNewInformation(boat, boatService, additionalInfo);

            UoW.GetRepository<IServiceWriteRepository>().Update(boatService);
            UoW.GetRepository<IAdditionalBoatServiceInfoWriteRepository>().Update(additionalInfo);

            UoW.Commit();

            return Ok();
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.BoatOwner })]
        public IActionResult DeleteBoat(int boatId)
        {
            if (!CheckOwnerShip(boatId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && !x.IsCanceled && x.ServiceId == boatId &&
                            !x.IsServiceUnavailableMarker);

            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            try
            {
                UoW.BeginTransaction();
                var additionalInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>()
                    .GetAll()
                    .First(x => x.ServiceId == boatId);
                UoW.GetRepository<IAdditionalBoatServiceInfoWriteRepository>().Delete(additionalInfo);

                new DeleteService().DeleteAllInfoForService(boatId, UoW);

                UoW.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }
        }

        private void MapNewInformation(BoatDTO boat, Service boatService, AdditionalBoatServiceInfo additionalInfo)
        {
            boatService.Name = boat.Name;
            boatService.PricePerDay = boat.PricePerDay;
            boatService.Address = boat.Address;
            boatService.Longitude = boat.Longitude;
            boatService.Latitude = boat.Latitude;
            boatService.PromoDescription = boat.PromoDescription;
            boatService.TermsOfUse = boat.TermsOfUse;
            boatService.AdditionalEquipment = boat.AdditionalEquipment;
            boatService.Capacity = boat.Capacity;
            boatService.IsPercentageTakenFromCanceledReservations = boat.IsPercentageTaken;
            boatService.PercentageToTake = boat.PercentageToTake;
            boatService.AvailableFrom = boat.AvailableFrom;
            boatService.AvailableTo = boat.AvailableTo;

            additionalInfo.Length = boat.Length;
            additionalInfo.BoatType = boat.BoatType;
            additionalInfo.NumberOfEngines = boat.EngineNum;
            additionalInfo.PowerOfEngines = boat.EnginePower;
            additionalInfo.MaxSpeed = boat.Speed;

            var navigationTools = UoW.GetRepository<ILinkNavigationBoatReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == boatService.ServiceId);
            foreach (var navigationTool in navigationTools)
            {
                UoW.GetRepository<ILinkNavigationBoatWriteRepository>().Delete(navigationTool);
            }

            foreach (var boatNavigationalTool in boat.NavigationalTools.Distinct())
            {
                var navigationTool = new LinkNavigationBoat()
                {
                    ServiceId = boatService.ServiceId,
                    BoatServiceNavigationToolId = boatNavigationalTool
                };
                UoW.GetRepository<ILinkNavigationBoatWriteRepository>().Add(navigationTool);
            }
        }


        private AdditionalBoatServiceInfo MapToAdditionalBoatInfo(BoatDTO newBoat)
        {
            return new AdditionalBoatServiceInfo()
            {
                Length = newBoat.Length,
                BoatType = newBoat.BoatType,
                NumberOfEngines = newBoat.EngineNum,
                PowerOfEngines = newBoat.EnginePower,
                MaxSpeed = newBoat.Speed
            };
        }

        private Service MapToBoatInfo(BoatDTO newBoat)
        {
            return new Service()
            {
                OwnerId = GetUserIdFromCookie(),
                ServiceType = ServiceType.Boat,
                Name = newBoat.Name,
                PricePerDay = newBoat.PricePerDay,
                AdditionalEquipment = newBoat.AdditionalEquipment,
                Address = newBoat.Address,
                Longitude = newBoat.Longitude,
                Latitude = newBoat.Latitude,
                PromoDescription = newBoat.PromoDescription,
                TermsOfUse = newBoat.TermsOfUse,
                Capacity = newBoat.Capacity,
                IsPercentageTakenFromCanceledReservations = newBoat.IsPercentageTaken,
                PercentageToTake = newBoat.PercentageToTake,
                AvailableFrom = newBoat.AvailableFrom,
                AvailableTo = newBoat.AvailableTo
            };
        }
    }
}
