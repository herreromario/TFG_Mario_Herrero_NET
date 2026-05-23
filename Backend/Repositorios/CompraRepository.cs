using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class CompraRepository : GenericRepository<Compra>, ICompraRepository
    {
        public CompraRepository(StockMealContext context, ILogger<GenericRepository<Compra>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Compra>> GetComprasConProveedorAsync()
        {
            return await Query(asNoTracking: true, c => c.IdProveedorNavigation, c => c.DetalleCompras)
                .Include(c => c.DetalleCompras)
                    .ThenInclude(d => d.IdProductoNavigation)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<List<HistorialCompraLinea>> GetHistorialComprasLineasAsync()
        {
            return await _context.DetalleCompras
                .AsNoTracking()
                .Include(d => d.IdCompraNavigation)
                    .ThenInclude(c => c.IdProveedorNavigation)
                .Include(d => d.IdProductoNavigation)
                .OrderByDescending(d => d.IdCompraNavigation.Fecha)
                .ThenByDescending(d => d.IdCompra)
                .Select(d => new HistorialCompraLinea
                {
                    IdCompra = d.IdCompra,
                    FechaCompra = d.IdCompraNavigation.Fecha,
                    Proveedor = d.IdCompraNavigation.IdProveedorNavigation.Nombre,
                    Producto = d.IdProductoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                })
                .ToListAsync();
        }

        public async Task RegistrarCompraConDetallesYActualizarStockAsync(Compra compra, IEnumerable<DetalleCompra> detalles)
        {
            if (compra == null) throw new ArgumentNullException(nameof(compra));
            if (detalles == null) throw new ArgumentNullException(nameof(detalles));

            var detalleList = detalles.ToList();
            if (!detalleList.Any())
                throw new DataAccessException("La compra debe incluir al menos un detalle para actualizar stock.");

            await using var tx = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                if (compra.Fecha == null)
                    compra.Fecha = DateTime.Now;

                foreach (var detalle in detalleList)
                {
                    detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                }

                compra.ImporteTotal = detalleList.Sum(d => d.Subtotal);
                compra.DetalleCompras = detalleList;

                await _context.Compras.AddAsync(compra).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                foreach (var detalle in detalleList)
                {
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto)
                        .ConfigureAwait(false);

                    if (producto == null)
                        throw new DataAccessException($"No existe el producto con id {detalle.IdProducto}.");

                    if (producto.Tipo == "ingrediente")
                    {
                        producto.StockDisponible = (producto.StockDisponible ?? 0) + detalle.Cantidad;
                        _context.Productos.Update(producto);
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync().ConfigureAwait(false);
                throw new DataAccessException("Error al registrar la compra y actualizar el stock de ingredientes.", ex);
            }
        }
    }
}
