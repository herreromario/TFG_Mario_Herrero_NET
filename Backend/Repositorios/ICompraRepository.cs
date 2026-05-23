using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface ICompraRepository : IGenericRepository<Compra>
    {
        Task<List<Compra>> GetComprasConProveedorAsync();
        Task<List<HistorialCompraLinea>> GetHistorialComprasLineasAsync();
        Task RegistrarCompraConDetallesYActualizarStockAsync(Compra compra, IEnumerable<DetalleCompra> detalles);
    }
}
