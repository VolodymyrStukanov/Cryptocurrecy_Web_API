using WebApplication1.DB.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ICurrencyService
    {
        public void AddCurrency(Currency model);
        public void UpdateCurrency(Currency model);
        public void RemoveCurrency(Currency model);
        public void RemoveCurrency(string assetId);
        public IEnumerable<Currency> GetAllCurrencies();
        public Currency? GetCurrency(string assetId);
    }
}
