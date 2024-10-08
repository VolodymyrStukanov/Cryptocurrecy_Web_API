using WebApplication1.DB;
using WebApplication1.DB.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _context;

        public CurrencyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddCurrency(Currency model)
        {
            _context.Currencies.Add(model);
            _context.SaveChanges();
        }

        public void UpdateCurrency(Currency model)
        {
            var cur = _context.Currencies.FirstOrDefault(x => x.AssetId == model.AssetId);
            if (cur == null)
                throw new Exception($"There is no currency with assert id {model}");

            _context.Entry(cur).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            _context.Currencies.Update(model);
            _context.SaveChanges();
        }

        public void RemoveCurrency(Currency model)
        {
            var cur = _context.Currencies.FirstOrDefault(x => x.AssetId == model.AssetId);
            if (cur == null)
                throw new Exception($"There is no currency with assert id {model}");

            _context.Currencies.Remove(model);
            _context.SaveChanges();
        }

        public void RemoveCurrency(string assetId)
        {
            var cur = _context.Currencies.FirstOrDefault(x => x.AssetId == assetId);
            if (cur == null)
                throw new Exception($"There is no currency with assert id {assetId}");

            _context.Currencies.Remove(cur);
            _context.SaveChanges();
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            return _context.Currencies;
        }

        public Currency? GetCurrency(string assetId)
        {
            return _context.Currencies.FirstOrDefault(x => x.AssetId == assetId);
        }
    }
}
