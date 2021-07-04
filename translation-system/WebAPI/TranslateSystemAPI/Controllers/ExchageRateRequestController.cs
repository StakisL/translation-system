using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TranslateSystemAPI.Services;

namespace TranslateSystemAPI.Controllers
{
    [Route("api/[controller]"), ApiController]
    public sealed class ExchangeRateRequestController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateRequestController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public async Task GetCurrentExchangeCurrencyRate() => await _exchangeRateService.ExchangeRateRequest();
    }
}
