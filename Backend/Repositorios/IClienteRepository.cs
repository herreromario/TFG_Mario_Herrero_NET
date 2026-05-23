using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IClienteRepository : IGenericRepository<Cliente>
    {
        Task<List<Cliente>> GetClientesAsync();
    }
}
