using API.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
    public static class ModelDTOConverter
    {
        public static Service ToModel(this AdventureDTO dto)
        {
            if (dto == null) return null;

            return new Service
            {
                Name = dto.Name,
                ServiceType = ServiceType.Adventure,
                PricePerDay = dto.PricePerDay,
                Address = dto.Address,
                Longitude = dto.Longitude,
                Latitude = dto.Latitude,
                PromoDescription = dto.PromoDescription,
                TermsOfUse = dto.TermsOfUse,
                AdditionalEquipment = dto.AdditionalEquipment,
                Capacity = dto.Capacity,
                IsPercentageTakenFromCanceledReservations = dto.IsPercentageTakenFromCanceledReservations,
                PercentageToTake = dto.PercentageToTake,
                OwnerId = dto.OwnerId,
                AvailableFrom = dto.AvailableFrom,
                AvailableTo = dto.AvailableTo
            };
        }

        public static AdditionalAdventureInfo ToModel(this AdventureDTO dto, int adventureId)
        {
            return new AdditionalAdventureInfo
            {
                AdditionalOffers = dto.AdditionalOffers,
                AdventureId = adventureId
            };
        }

        public static UserAvailability ToModel(this UserAvailabilityPeriodDTO dto)
        {
            if (dto == null) return null;

            return new UserAvailability
            {
                UserId = dto.UserId,
                PeriodStart = dto.PeriodStart,
                PeriodEnd = dto.PeriodEnd,
                Status = dto.Status
            };
        }
    }
}
