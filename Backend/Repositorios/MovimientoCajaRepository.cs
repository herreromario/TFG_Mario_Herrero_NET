using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class MovimientoCajaRepository : GenericRepository<MovimientoCaja>, IMovimientoCajaRepository
    {
        public MovimientoCajaRepository(StockMealContext context, ILogger<GenericRepository<MovimientoCaja>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<MovimientoCaja>> GetMovimientosConRelacionesAsync()
        {
            return await Query(asNoTracking: true, m => m.IdCompraNavigation, m => m.IdTransaccionNavigation, m => m.IdCierreNavigation)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }
    }
}
