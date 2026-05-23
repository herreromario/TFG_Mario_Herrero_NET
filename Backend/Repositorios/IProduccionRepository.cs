using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IProduccionRepository : IGenericRepository<Produccion>
    {
        Task<List<HistorialProduccionLinea>> GetHistorialLineasAsync();
        Task RegistrarProduccionAsync(IEnumerable<Produccion> lineas);
    }
}
