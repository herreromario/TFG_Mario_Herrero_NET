using Microsoft.EntityFrameworkCore;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;
using Microsoft.Extensions.Logging;

namespace StockMeal.Backend.Repositorios
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        private readonly StockMealContext _context;

        public ProductoRepository(
            StockMealContext context,
            ILogger<GenericRepository<Producto>> logger)
            : base(context, logger)
        {
            _context = context;
        }

        /// <summary>
        /// Comprueba si existe un producto con un nombre dado.
        /// </summary>
        public async Task<bool> NombreProductoExisteAsync(string nombreProducto, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                .AnyAsync(p => p.Nombre == nombreProducto, cancellationToken);
        }

        public async Task<List<Producto>> GetPlatosAsync()
        {
            return await Query(asNoTracking: true)
                .Where(p => p.Tipo == "plato")
                .ToListAsync();
        }

        public async Task<List<Producto>> GetIngredientesAsync()
        {
            return await Query(asNoTracking: true)
                .Where(p => p.Tipo == "ingrediente")
                .ToListAsync();
        }
    }
}
