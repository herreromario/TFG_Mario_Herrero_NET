using StockMeal.Backend.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMeal.Backend.Repositorios
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        // Método para verificar si un nombre de producto ya existe en la base de datos
        Task<bool> NombreProductoExisteAsync(string nombreProducto, CancellationToken cancellationToken = default);

        Task<List<Producto>> GetPlatosAsync();

        Task<List<Producto>> GetIngredientesAsync();
    }
}
