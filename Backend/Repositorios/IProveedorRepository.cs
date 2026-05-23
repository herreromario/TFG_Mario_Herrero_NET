using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.Repositorios
{
    public interface IProveedorRepository : IGenericRepository<Proveedor>
    {
        Task<List<Proveedor>> GetProveedoresAsync();
    }
}
