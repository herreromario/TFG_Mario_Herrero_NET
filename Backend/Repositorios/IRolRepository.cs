using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IRolRepository : IGenericRepository<Rol>
    {
        Task<List<Rol>> GetRolesConPermisosAsync();
    }
}
