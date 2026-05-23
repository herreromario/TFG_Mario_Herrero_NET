using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IPermisoRepository : IGenericRepository<Permiso>
    {
        Task<List<Permiso>> GetPermisosAsync();
    }
}
