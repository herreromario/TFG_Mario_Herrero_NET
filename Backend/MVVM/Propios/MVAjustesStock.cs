using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Collections.ObjectModel;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVAjustesStock : MVBase
    {
        private readonly IProductoRepository _productoRepository;
        public ObservableCollection<Producto> Ingredientes { get; } = new();
        public string? error { get; set; }

        public MVAjustesStock(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task Inicializa()
        {
            try
            {
                var ingredientes = await _productoRepository.GetIngredientesAsync();
                Ingredientes.Clear();
                foreach (var i in ingredientes.OrderBy(x => x.Nombre))
                    Ingredientes.Add(i);
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
            }
        }

        public async Task<bool> GuardarIngredienteAsync(Producto ingrediente)
        {
            try
            {
                if (ingrediente.StockDisponible < 0 || ingrediente.StockMinimo < 0)
                {
                    error = "Stock disponible y stock mínimo no pueden ser negativos.";
                    return false;
                }

                await _productoRepository.UpdateAsync(ingrediente);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }
    }
}
