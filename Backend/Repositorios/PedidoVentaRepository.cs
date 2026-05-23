using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class PedidoVentaRepository : GenericRepository<PedidoVentum>, IPedidoVentaRepository
    {
        public PedidoVentaRepository(StockMealContext context, ILogger<GenericRepository<PedidoVentum>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<PedidoVentum>> GetPedidosConRelacionesAsync()
        {
            return await Query(asNoTracking: true, p => p.IdClienteNavigation, p => p.IdEmpleadoNavigation, p => p.DetallePedidos)
                .Include(p => p.DetallePedidos)
                    .ThenInclude(d => d.IdProductoNavigation)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
    }
}
