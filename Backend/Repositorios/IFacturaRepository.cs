using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IFacturaRepository : IGenericRepository<Factura>
    {
        Task<List<Factura>> GetFacturasConPedidoAsync();
    }
}
