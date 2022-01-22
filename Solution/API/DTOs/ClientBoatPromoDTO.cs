using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ClientBoatPromoDTO
    {
        public BoatDTO Boat { get; set; }

        public PromoActionWrapperDTO Promo { get; set; }
    }
}
