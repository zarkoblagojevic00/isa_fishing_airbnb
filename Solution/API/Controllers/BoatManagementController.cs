using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.ConfigurationObjects;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
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
        public IActionResult SearchBoats(
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

            var availableServices = new ServiceFinder(ServiceType.Boat, UoW).FindServices(@params, userId);

            var additionalInformation = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>().GetAll();
            var linkNavigationRepo = UoW.GetRepository<ILinkNavigationBoatReadRepository>();
            var navigationToolRepo = UoW.GetRepository<IBoatServiceNavigationToolReadRepository>();

            var result = availableServices.Select(fut => fut.Service).Join(additionalInformation, x => x.ServiceId, y => y.ServiceId, (x, y) => new BoatDTO()
            {
                BoatId = x.ServiceId,
                Name = x.Name,
                AdditionalEquipment = x.AdditionalEquipment,
                BoatType = y.BoatType,
                Length = y.Length,
                EngineNum = y.NumberOfEngines,
                EnginePower = y.PowerOfEngines,
                Speed = y.MaxSpeed,
                CityName = UoW.GetRepository<ICityReadRepository>().GetById(x.CityId).Name,
                Address = x.Address,
                AvailableFrom = x.AvailableFrom,
                AvailableTo = x.AvailableTo,
                Capacity = x.Capacity,
                IsPercentageTaken = x.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = x.PercentageToTake,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PricePerDay = x.PricePerDay,
                PromoDescription = x.PromoDescription,
                TermsOfUse = x.PromoDescription,
                ImageIds = UoW.GetRepository<IImageReadRepository>().GetAll().Where(z => z.ServiceId == x.ServiceId).Select(z => z.ImageId),
                AverageMark = new AverageMarkCalculator(UoW).CalculateAverageMark(x.ServiceId),
                NavigationToolsObj = linkNavigationRepo
                .GetAll()
                .Where(p => p.ServiceId == x.ServiceId)
                .Select(n => navigationToolRepo.GetById(n.BoatServiceNavigationToolId))
            });
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetQuickActionsForClient()
        {
            int userId = -1;
            try
            {
                userId = GetUserIdFromCookie();
            }
            catch (Exception ignore) { };

            var promoActions = new ServiceFinder(ServiceType.Boat, UoW).GetAvailablePromoActions(userId);
            var serviceReadRepository = UoW.GetRepository<IServiceReadRepository>();
            var boats = serviceReadRepository.GetAll().Where(s => s.ServiceType == ServiceType.Boat);

            var additionalInformation = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>().GetAll();
            var linkNavigationRepo = UoW.GetRepository<ILinkNavigationBoatReadRepository>();
            var navigationToolRepo = UoW.GetRepository<IBoatServiceNavigationToolReadRepository>();

            var boatDTOS = boats.Join(additionalInformation, x => x.ServiceId, y => y.ServiceId, (x, y) => new BoatDTO()
            {
                BoatId = x.ServiceId,
                Name = x.Name,
                AdditionalEquipment = x.AdditionalEquipment,
                BoatType = y.BoatType,
                Length = y.Length,
                EngineNum = y.NumberOfEngines,
                EnginePower = y.PowerOfEngines,
                Speed = y.MaxSpeed,
                CityName = UoW.GetRepository<ICityReadRepository>().GetById(x.CityId).Name,
                Address = x.Address,
                AvailableFrom = x.AvailableFrom,
                AvailableTo = x.AvailableTo,
                Capacity = x.Capacity,
                IsPercentageTaken = x.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = x.PercentageToTake,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PricePerDay = x.PricePerDay,
                PromoDescription = x.PromoDescription,
                TermsOfUse = x.PromoDescription,
                ImageIds = UoW.GetRepository<IImageReadRepository>().GetAll().Where(z => z.ServiceId == x.ServiceId).Select(z => z.ImageId),
                AverageMark = new AverageMarkCalculator(UoW).CalculateAverageMark(x.ServiceId),
                NavigationToolsObj = linkNavigationRepo
                .GetAll()
                .Where(p => p.ServiceId == x.ServiceId)
                .Select(n => navigationToolRepo.GetById(n.BoatServiceNavigationToolId))
            });

            // var addBoatResDetails = UoW.GetRepository<IBoatReservationDetailReadRepository>().GetAll();
            var result = boatDTOS.Join(promoActions, p => p.BoatId, q => q.ServiceId, (p, q) => new ClientBoatPromoDTO
            {
                Boat = p,
                Promo = new PromoActionWrapperDTO()
                {
                    PromoActionId = q.PromoActionId,
                    ServiceId = q.ServiceId,
                    PricePerDay = q.PricePerDay,
                    IsTaken = q.IsTaken,
                    Capacity = q.Capacity,
                    AddedBenefits = q.AddedBenefits,
                    Place = q.Place,
                    StartDateTime = q.StartDateTime,
                    EndDateTime = q.EndDateTime,
                    // Role = addBoatResDetails.FirstOrDefault(m => m.RelevantId == q.PromoActionId).BoatOwnerResponsibilityType.ToString()
                },
            });

            return Ok(result);

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
        public IEnumerable<BoatDTO> GetOwnedBoats()
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

            var existingCity = UoW.GetRepository<ICityReadRepository>().GetAll()
                .FirstOrDefault(x => x.Name == newBoat.CityName);
            if (existingCity == null)
            {
                ModelState.AddModelError("CityName", "City that is being requested doesn't exist");
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

                if (newBoat.NavigationalTools != null)
                {
                    foreach (var navigationToolId in newBoat.NavigationalTools.Distinct())
                    {
                        var navigationTool = new LinkNavigationBoat()
                        {
                            ServiceId = boat.ServiceId,
                            BoatServiceNavigationToolId = navigationToolId
                        };
                        UoW.GetRepository<ILinkNavigationBoatWriteRepository>().Add(navigationTool);
                    }
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

            var existingCity = UoW.GetRepository<ICityReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name == boat.CityName);
            if (existingCity == null)
            {
                ModelState.AddModelError("CityName", "City that is being requested doesn't exist");
                return BadRequest(ModelState);
            }

            var existingBoat = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .FirstOrDefault(x =>
                    x.OwnerId == GetUserIdFromCookie() && x.ServiceId != boat.BoatId && x.Name == boat.Name);
            if (existingBoat != null)
            {
                ModelState.AddModelError("Name", "There is already another boat with that name that you possess!");
                return BadRequest(ModelState);
            }

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == boat.BoatId && !x.IsCanceled && !x.IsServiceUnavailableMarker);

            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            UoW.BeginTransaction();
            
            var boatService = new Service();
            try
            {
                boatService = new ServiceLocker(UoW).ObtainLockedService(boat.BoatId.Value);
            }
            catch
            {
                return BadRequest(Responses.UnavailableRightNow);
            }
            var additionalInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>()
                .GetAll()
                .First(x => x.ServiceId == boat.BoatId);
            
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

                var lockedService = new ServiceLocker(UoW).ObtainLockedService(boatId);

                var additionalInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>()
                    .GetAll()
                    .First(x => x.ServiceId == boatId);
                UoW.GetRepository<IAdditionalBoatServiceInfoWriteRepository>().Delete(additionalInfo);

                new DeleteService().DeleteAllInfoForService(boatId, UoW);

                var navigationTools = UoW.GetRepository<ILinkNavigationBoatReadRepository>()
                    .GetAll()
                    .Where(x => x.ServiceId == boatId);
                foreach (var navigationTool in navigationTools)
                {
                    UoW.GetRepository<ILinkNavigationBoatWriteRepository>().Delete(navigationTool);
                }

                UoW.Commit();
                return Ok();
            }
            catch (ADOException e)
            {
                return BadRequest(Responses.UnavailableRightNow);
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
            boatService.CityId = UoW.GetRepository<ICityReadRepository>()
                .GetAll()
                .First(x => x.Name == boat.CityName)
                .CityId;

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
                AvailableTo = newBoat.AvailableTo,
                CityId = UoW.GetRepository<ICityReadRepository>().GetAll().First(x => x.Name == newBoat.CityName).CityId
            };
        }
    }
}
