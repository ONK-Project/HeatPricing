﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricityPricing.Services
{
    public interface IPublicChargingService
    {
        Task<PriceAndTaxes> GetHeatPriceForDate(DateTime dateTime);
    }
}
