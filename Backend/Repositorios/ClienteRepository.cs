using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(StockMealContext context, ILogger<GenericRepository<Cliente>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await GetAllAsync();
        }
    }
}
