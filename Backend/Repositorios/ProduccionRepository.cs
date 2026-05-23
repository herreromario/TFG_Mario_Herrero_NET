using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class ProduccionRepository : GenericRepository<Produccion>, IProduccionRepository
    {
        public ProduccionRepository(StockMealContext context, ILogger<GenericRepository<Produccion>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<HistorialProduccionLinea>> GetHistorialLineasAsync()
        {
            return await _context.Produccions
                .AsNoTracking()
                .Include(p => p.Producto)
                .OrderByDescending(p => p.Fecha)
                .ThenByDescending(p => p.IdProduccion)
                .Select(p => new HistorialProduccionLinea
                {
                    IdProduccion = p.IdProduccion,
                    Fecha = p.Fecha,
                    Empleado = "-",
                    Plato = p.Producto != null ? p.Producto.Nombre : $"Producto {p.IdProducto}",
                    Cantidad = p.Cantidad
                })
                .ToListAsync();
        }

        public async Task RegistrarProduccionAsync(IEnumerable<Produccion> lineas)
        {
            var lista = lineas.ToList();
            if (!lista.Any())
                throw new DataAccessException("Debe existir al menos una línea de producción.");

            await using var tx = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                await _context.Produccions.AddRangeAsync(lista).ConfigureAwait(false);

                foreach (var linea in lista)
                {
                    var receta = await _context.Receta
                        .Where(r => r.IdProducto == linea.IdProducto)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    foreach (var r in receta)
                    {
                        var ingrediente = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == r.IdIngrediente).ConfigureAwait(false);
                        if (ingrediente != null)
                        {
                            var consumo = (int)Math.Ceiling(r.Cantidad * linea.Cantidad);
                            ingrediente.StockDisponible = (ingrediente.StockDisponible ?? 0) - consumo;
                            _context.Productos.Update(ingrediente);
                        }
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync().ConfigureAwait(false);
                throw new DataAccessException("Error registrando producción.", ex);
            }
        }
    }
}
