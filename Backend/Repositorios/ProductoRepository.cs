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


        private static void NormalizeProducto(Producto producto)
        {
            if (producto == null) return;

            producto.Descripcion ??= string.Empty;
            producto.Unidad ??= string.Empty;
            producto.Tipo ??= string.Empty;
            producto.StockDisponible ??= 0;
            producto.StockMinimo ??= 0;
            producto.Precio ??= 0m;
        }

        public override async Task AddAsync(Producto entity)
        {
            NormalizeProducto(entity);
            await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(Producto entity)
        {
            NormalizeProducto(entity);
            await base.UpdateAsync(entity);
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
