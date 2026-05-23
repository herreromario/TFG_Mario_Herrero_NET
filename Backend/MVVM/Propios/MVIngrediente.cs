using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVIngrediente : MVBase
    {
        private  readonly IProductoRepository _productoRepository;

        private Producto _ingrediente;
        public Producto ingrediente
        {
            get => _ingrediente;
            set => SetProperty(ref _ingrediente, value);
        }

        private List<string> _listaUnidades;
        public List<string> listaUnidades => _listaUnidades;

        private List<Producto> _listaIngredientes;
        public ListCollectionView listaIngredientes { get; set; }

        private String _textoNombreIngrediente;
        public String textoNombreIngrediente
        {
            get => _textoNombreIngrediente;
            set => SetProperty(ref _textoNombreIngrediente, value);
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

        public MVIngrediente(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
            _ingrediente = new Producto();
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
            _criterioNombre = new Predicate<Producto>(p => !string.IsNullOrEmpty(_textoNombreIngrediente)
                              && p.Nombre!.ToLower().StartsWith(_textoNombreIngrediente.ToLower()));
        }

        private void AddCriterios()
        {
            _criterios.Clear();
            if (!string.IsNullOrEmpty(_textoNombreIngrediente))
            {
                _criterios.Add(_criterioNombre);
            }
        }

        private bool FiltroCriterios(object item)
        {
            bool correcto = true;
            Producto ingrediente = (Producto)item;
            if (_criterios != null)
            {
                correcto = _criterios.TrueForAll(x => x(ingrediente));
            }
            return correcto;
        }

        private async Task InicializaListas()
        {
            try
            {
                _listaUnidades = new List<string> { "unidad", "g", "ml", "lamina", "loncha" };
                OnPropertyChanged(nameof(listaUnidades));

                _listaIngredientes = await _productoRepository.GetIngredientesAsync();
                listaIngredientes = new ListCollectionView(_listaIngredientes);
                _criterios = new List<Predicate<Producto>>();
                OnPropertyChanged(nameof(listaIngredientes));
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
            }
        }

        public void Filtrar()
        {
            AddCriterios();
            listaIngredientes.Filter = _predicadoFiltros;
        }

        public void LimpiarFiltros()
        {
            textoNombreIngrediente = string.Empty;
            listaIngredientes.Filter = null;
        }

        public async Task<bool> AñadirIngredienteAsync()
        {
            try
            {
                ingrediente.Tipo = "ingrediente";
                
                await _productoRepository.AddAsync(ingrediente);
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> ActualizarIngredienteAsync(Producto ingredienteEditado)
        {
            try
            {
                ingredienteEditado.Tipo = "ingrediente";
                await _productoRepository.UpdateAsync(ingredienteEditado);
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> EliminarIngredienteAsync(Producto ingredienteAEliminar)
        {
            try
            {
                await _productoRepository.RemoveAsync(ingredienteAEliminar);
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
