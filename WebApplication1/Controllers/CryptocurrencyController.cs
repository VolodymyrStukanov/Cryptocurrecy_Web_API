using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptocurrencyController : APIController
    {
        private readonly ILogger<CryptocurrencyController> _logger;
        private ICurrencyService _currencyService;

        public CryptocurrencyController(
            ICurrencyService currencyService, 
            ILogger<CryptocurrencyController> logger)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        [HttpGet]
        [Route("GetAvailableCryptocurrencies")]
        public IActionResult GetAvailableCryptocurrencies()
        {
            try
            {
                var currencies = _currencyService.GetAllCurrencies().ToList();
                if(currencies.Count() != 0)
                {
                    List<object> result = new List<object>();
                    foreach (var item in currencies)
                    {
                        dynamic temp = new ExpandoObject();
                        temp.AssetId = item.AssetId;
                        temp.Name = item.Name;

                        result.Add(temp);
                    }
                    return BuildSuccessResult(result, "Currencies are found", 200);
                }
                throw new Exception("There are no any cryptocurrencies");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs in GetAvailableCryptocurrencies method: {ex.Message}");
                return BuildErrorResult(ex.Message, 400);
            }
        }

        [HttpGet]
        [Route("GetCryptocurrencyPrice/{assetId}")]
        public IActionResult GetCryptocurrencyPrice(string assetId)
        {
            try
            {
                var currency = _currencyService.GetCurrency(assetId);

                if (currency != null && currency.Rate != null)
                {
                    return BuildSuccessResult(currency, "CurrencyInfo is found", 200);
                }
                throw new Exception($"There are no info about the currency with assert id {assetId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurs in GetCryptocurrencyPrice method: {ex.Message}");
                return BuildErrorResult(ex.Message, 400);
            }
        }
    }
}
