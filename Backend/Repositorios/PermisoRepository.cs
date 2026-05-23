using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class PermisoRepository : GenericRepository<Permiso>, IPermisoRepository
    {
        public PermisoRepository(StockMealContext context, ILogger<GenericRepository<Permiso>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Permiso>> GetPermisosAsync()
        {
            return await Query(asNoTracking: true)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
    }
}
