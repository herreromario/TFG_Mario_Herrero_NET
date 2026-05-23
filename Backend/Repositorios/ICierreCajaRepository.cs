using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface ICierreCajaRepository : IGenericRepository<CierreCaja>
    {
        Task<List<CierreCaja>> GetCierresAsync();
    }
}
