using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class CierreCajaRepository : GenericRepository<CierreCaja>, ICierreCajaRepository
    {
        public CierreCajaRepository(StockMealContext context, ILogger<GenericRepository<CierreCaja>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<CierreCaja>> GetCierresAsync()
        {
            return await Query(asNoTracking: true)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }
    }
}
