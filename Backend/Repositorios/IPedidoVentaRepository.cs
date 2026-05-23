using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IPedidoVentaRepository : IGenericRepository<PedidoVentum>
    {
        Task<List<PedidoVentum>> GetPedidosConRelacionesAsync();
    }
}
