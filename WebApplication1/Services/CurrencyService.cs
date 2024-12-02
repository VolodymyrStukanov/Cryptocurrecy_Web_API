using WebApplication1.DB;
using WebApplication1.DB.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(ApplicationDbContext context,
            ILogger<CurrencyService> logger)
        {
            _context = context;
            _logger = logger;
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
            {
                _logger.LogError("There is no currency with assert id {0}", model.AssetId);
                return;
            }

            _context.Entry(cur).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            _context.Currencies.Update(model);
            _context.SaveChanges();
        }

        public void RemoveCurrency(Currency model)
        {
            var cur = _context.Currencies.FirstOrDefault(x => x.AssetId == model.AssetId);
            if (cur == null)
            {
                _logger.LogError("There is no currency with assert id {0}", model.AssetId);
                return;
            }

            _context.Currencies.Remove(model);
            _context.SaveChanges();
        }

        public void RemoveCurrency(string assetId)
        {
            var cur = _context.Currencies.FirstOrDefault(x => x.AssetId == assetId);

            if (cur == null)
            {
                _logger.LogError("There is no currency with assert id {0}", assetId);
                return;
            }

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
