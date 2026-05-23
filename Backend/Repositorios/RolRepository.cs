using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(StockMealContext context, ILogger<GenericRepository<Rol>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Rol>> GetRolesConPermisosAsync()
        {
            return await Query(asNoTracking: true, r => r.IdPermisos)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }
    }
}
