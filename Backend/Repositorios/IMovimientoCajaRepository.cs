using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IMovimientoCajaRepository : IGenericRepository<MovimientoCaja>
    {
        Task<List<MovimientoCaja>> GetMovimientosConRelacionesAsync();
    }
}
