using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class ProveedorRepository : GenericRepository<Proveedor>, IProveedorRepository
    {
        public ProveedorRepository(StockMealContext context, ILogger<GenericRepository<Proveedor>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Proveedor>> GetProveedoresAsync()
        {
            return await GetAllAsync();
        }
    }
}
