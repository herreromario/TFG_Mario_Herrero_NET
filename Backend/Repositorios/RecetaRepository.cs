using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class RecetaRepository : GenericRepository<Recetum>, IRecetaRepository
    {
        public RecetaRepository(StockMealContext context, ILogger<GenericRepository<Recetum>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Recetum>> GetRecetasConRelacionesAsync()
        {
            return await Query(asNoTracking: true, r => r.IdProductoNavigation, r => r.IdIngredienteNavigation)
                .OrderBy(r => r.IdProductoNavigation.Nombre)
                .ToListAsync();
        }
    }
}
