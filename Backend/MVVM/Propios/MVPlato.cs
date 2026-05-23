using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVPlato : MVBase
    {
        private readonly IProductoRepository _productoRepository;

        private Producto _plato;
        public Producto plato
        {
            get => _plato;
            set => SetProperty(ref _plato, value);
        }

        private List<Producto> _listaPlatos;
        public ListCollectionView listaPlatos { get; set; }

        private String _textoNombrePlato;
        public String textoNombrePlato
        {
            get => _textoNombrePlato;
            set => SetProperty(ref _textoNombrePlato, value);
        }

        private List<Predicate<Producto>> _criterios;
        private Predicate<Producto> _criterioNombre;
        private Predicate<object> _predicadoFiltros;


        private string? _error;
        public string? error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        public MVPlato(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
            _plato = new Producto();
        }

        public async Task Inicializa()
        {
            try
            {
                await InicializaListas();
                InicializaCriterios();
                _predicadoFiltros = new Predicate<object>(FiltroCriterios);
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
            }
        }

        private void InicializaCriterios()
        {
            _criterioNombre = new Predicate<Producto>(p => !string.IsNullOrEmpty(_textoNombrePlato)
                              && p.Nombre!.ToLower().StartsWith(_textoNombrePlato.ToLower()));
        }

        private void AddCriterios()
        {
            _criterios.Clear();
            if (!string.IsNullOrEmpty(_textoNombrePlato))
            {
                _criterios.Add(_criterioNombre);
            }
        }

        private bool FiltroCriterios(object item)

        {
            bool correcto = true;
            Producto plato = (Producto)item;
            if (_criterios != null)
            {
                correcto = _criterios.TrueForAll(x => x(plato));
            }
            return correcto;
        }

        private async Task InicializaListas()
        {
            try
            {
                _listaPlatos = await _productoRepository.GetPlatosAsync();
                listaPlatos = new ListCollectionView(_listaPlatos);
                _criterios = new List<Predicate<Producto>>();
                OnPropertyChanged(nameof(listaPlatos));
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
            }
        }

        public void Filtrar()
        {
            AddCriterios();
            listaPlatos.Filter = _predicadoFiltros;
        }

        public void LimpiarFiltros()
        {
            textoNombrePlato = string.Empty;
            listaPlatos.Filter = null;
        }

        public async Task<bool> AñadirPlatoAsync()
        {
            try
            {
                plato.Tipo = "plato";
                plato.Unidad = "unidad";

                await _productoRepository.AddAsync(plato);
                await Inicializa();
                return true;
            }
            catch (Exception ex) 
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> ActualizarPlatoAsync(Producto platoEditado)
        {
            try
            {
                platoEditado.Tipo = "plato";
                platoEditado.Unidad = "unidad";
                await _productoRepository.UpdateAsync(platoEditado);
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> EliminarPlatoAsync(Producto platoAEliminar)
        {
            try
            {
                await _productoRepository.RemoveAsync(platoAEliminar);
                await Inicializa();
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
