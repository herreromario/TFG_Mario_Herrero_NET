using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IRecetaRepository : IGenericRepository<Recetum>
    {
        Task<List<Recetum>> GetRecetasConRelacionesAsync();
    }
}
