using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ClientAdventurePromoDTO
    {
        public AdventureDTO Adventure { get; set; }
        public PromoActionWrapperDTO Promo { get; set; }
    }
}
