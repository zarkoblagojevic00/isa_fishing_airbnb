﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ClientVillaPromoDTO
    {
        public VillaDTO Villa { get; set; }

        public PromoActionWrapperDTO Promo { get; set; }
    }
}
