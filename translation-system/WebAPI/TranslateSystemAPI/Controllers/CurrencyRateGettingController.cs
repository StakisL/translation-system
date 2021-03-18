using Microsoft.AspNetCore.Mvc;

namespace TranslateSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRateGettingController : ControllerBase
    {
        public CurrencyRateGettingController()
        {
        }

        [HttpGet]
        //todo Добавить модель для возвращения нужного типа!
        public void GetCurrentExchangeCurrencyRate()
        {
        }
    }
}
