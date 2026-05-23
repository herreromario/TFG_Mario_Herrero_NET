using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(StockMealContext context, ILogger<GenericRepository<Factura>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Factura>> GetFacturasConPedidoAsync()
        {
            return await Query(asNoTracking: true, f => f.IdTransaccionNavigation)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }
    }
}
