using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVReceta : MVBase
    {
        private readonly IRecetaRepository _recetaRepository;
        private readonly IProductoRepository _productoRepository;

        public List<Producto> listaPlatos { get; private set; } = new();
        public List<Producto> listaIngredientes { get; private set; } = new();
        public ListCollectionView listaRecetas { get; private set; }
        public ListCollectionView listaResumenRecetas { get; private set; }
        public ObservableCollection<Recetum> nuevasLineas { get; private set; } = new();
        public Recetum? lineaSeleccionada { get; set; }

        public int idPlatoSeleccionado { get; set; }
        public int idIngredienteSeleccionado { get; set; }
        public decimal cantidadIngrediente { get; set; } = 1;
        public string? error { get; set; }

        public MVReceta(IRecetaRepository recetaRepository, IProductoRepository productoRepository)
        {
            _recetaRepository = recetaRepository;
            _productoRepository = productoRepository;
        }

        public async Task Inicializa()
        {
            listaPlatos = await _productoRepository.GetPlatosAsync();
            listaIngredientes = await _productoRepository.GetIngredientesAsync();
            OnPropertyChanged(nameof(listaPlatos));
            OnPropertyChanged(nameof(listaIngredientes));

            var recetas = await _recetaRepository.GetRecetasConRelacionesAsync();
            listaRecetas = new ListCollectionView(recetas);
            var resumen = recetas
                .GroupBy(r => new { r.IdProducto, Plato = r.IdProductoNavigation?.Nombre ?? $"Plato {r.IdProducto}" })
                .Select(g => new ResumenReceta
                {
                    IdProducto = g.Key.IdProducto,
                    Plato = g.Key.Plato,
                    Ingredientes = g.Count(),
                    TotalCantidad = g.Sum(x => x.Cantidad)
                })
                .OrderBy(x => x.Plato)
                .ToList();
            listaResumenRecetas = new ListCollectionView(resumen);
            OnPropertyChanged(nameof(listaRecetas));
            OnPropertyChanged(nameof(listaResumenRecetas));
        }

        public bool AgregarLineaReceta()
        {
            if (idPlatoSeleccionado <= 0 || idIngredienteSeleccionado <= 0 || cantidadIngrediente <= 0)
                return false;

            nuevasLineas.Add(new Recetum
            {
                IdProducto = idPlatoSeleccionado,
                IdIngrediente = idIngredienteSeleccionado,
                Cantidad = cantidadIngrediente
            });
            OnPropertyChanged(nameof(nuevasLineas));
            return true;
        }

        public async Task<bool> GuardarRecetaAsync()
        {
            try
            {
                if (!nuevasLineas.Any())
                {
                    error = "Debes añadir al menos una línea de ingredientes.";
                    return false;
                }

                foreach (var l in nuevasLineas)
                    await _recetaRepository.AddAsync(l);
                nuevasLineas.Clear();
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public bool QuitarLineaSeleccionada()
        {
            if (lineaSeleccionada == null)
                return false;

            var ok = nuevasLineas.Remove(lineaSeleccionada);
            if (ok)
                OnPropertyChanged(nameof(nuevasLineas));

            return ok;
        }
    }

    public class ResumenReceta
    {
        public int IdProducto { get; set; }
        public string Plato { get; set; } = string.Empty;
        public int Ingredientes { get; set; }
        public decimal TotalCantidad { get; set; }
    }
}
