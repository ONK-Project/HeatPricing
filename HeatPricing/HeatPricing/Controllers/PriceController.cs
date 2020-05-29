using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricityPricing.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace HeatPricing.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PriceController : ControllerBase
    {


        private readonly ILogger<PriceController> _logger;
        private readonly IPublicChargingService _publicChargingService;

        public PriceController(
            ILogger<PriceController> logger,
            IPublicChargingService publicChargingService)
        {
            _logger = logger;
            _publicChargingService = publicChargingService;
        }


        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] DateTime dateTime,
            [FromQuery] double ressourceUsage,
            [FromQuery] string unitOfMeassure)
        {
            var priceRequest = new PriceRequest()
            {
                DateTime = dateTime,
                RessourceUsage = ressourceUsage,
                UnitOfMeassure = unitOfMeassure
            };

            var totalPrice = await CalculateSubmissionPrice(priceRequest);

            return Ok(totalPrice);
        }

        private async Task<SubmissionPrice> CalculateSubmissionPrice(PriceRequest priceRequest)
        {
            var heatPriceInfo = await _publicChargingService.GetHeatPriceForDate(priceRequest.DateTime);
            var price = priceRequest.RessourceUsage * (heatPriceInfo.Price + heatPriceInfo.Tax);

            var submissionPrice = new SubmissionPrice()
            {
                Currency = heatPriceInfo.Currency,
                TotalCost = price,
            };

            return submissionPrice;
        }
    }
}
