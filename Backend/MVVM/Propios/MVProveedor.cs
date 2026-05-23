using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVProveedor : MVBase
    {
        private readonly IProveedorRepository _proveedorRepository;
        private List<Proveedor> _listaProveedores;
        private List<Predicate<Proveedor>> _criterios;
        private Predicate<Proveedor> _criterioNombre;
        private Predicate<object> _predicadoFiltros;

        public Proveedor proveedor { get; set; } = new Proveedor();
        public ListCollectionView listaProveedores { get; private set; }
        public string textoNombreProveedor { get; set; } = string.Empty;
        public string? error { get; set; }

        public MVProveedor(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        public async Task Inicializa()
        {
            _listaProveedores = await _proveedorRepository.GetProveedoresAsync();
            listaProveedores = new ListCollectionView(_listaProveedores);
            _criterios = new List<Predicate<Proveedor>>();
            _criterioNombre = p => !string.IsNullOrWhiteSpace(textoNombreProveedor)
                && p.Nombre.ToLower().StartsWith(textoNombreProveedor.ToLower());
            _predicadoFiltros = new Predicate<object>(FiltroCriterios);
            OnPropertyChanged(nameof(listaProveedores));
        }

        public void Filtrar()
        {
            _criterios.Clear();
            if (!string.IsNullOrWhiteSpace(textoNombreProveedor))
                _criterios.Add(_criterioNombre);
            listaProveedores.Filter = _predicadoFiltros;
        }

        private bool FiltroCriterios(object item) => _criterios.TrueForAll(x => x((Proveedor)item));

        public void LimpiarFiltros()
        {
            textoNombreProveedor = string.Empty;
            OnPropertyChanged(nameof(textoNombreProveedor));
            listaProveedores.Filter = null;
        }

        public async Task<bool> AnadirProveedorAsync()
        {
            try
            {
                await _proveedorRepository.AddAsync(proveedor);
                proveedor = new Proveedor();
                OnPropertyChanged(nameof(proveedor));
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> ActualizarProveedorAsync(Proveedor proveedorEditado)
        {
            try
            {
                await _proveedorRepository.UpdateAsync(proveedorEditado);
                await Inicializa();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }

        public async Task<bool> EliminarProveedorAsync(Proveedor proveedorAEliminar)
        {
            try
            {
                await _proveedorRepository.RemoveAsync(proveedorAEliminar);
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
