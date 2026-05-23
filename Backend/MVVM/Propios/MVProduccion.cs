using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVProduccion : MVBase
    {
        private readonly IProduccionRepository _produccionRepository;
        private readonly IProductoRepository _productoRepository;

        public DateTime fechaProduccion { get; set; } = DateTime.Today;
        public ObservableCollection<Produccion> lineasProduccion { get; private set; } = new();
        public List<Producto> listaPlatos { get; private set; } = new();
        public ListCollectionView historialProduccion { get; private set; }

        public int idProductoSeleccionado { get; set; }
        public int cantidadProducto { get; set; } = 1;
        public string? error { get; set; }

        public MVProduccion(IProduccionRepository produccionRepository, IProductoRepository productoRepository)
        {
            _produccionRepository = produccionRepository;
            _productoRepository = productoRepository;
        }

        public async Task Inicializa()
        {
            listaPlatos = await _productoRepository.GetPlatosAsync();
            OnPropertyChanged(nameof(listaPlatos));

            var historial = await _produccionRepository.GetHistorialLineasAsync();
            historialProduccion = new ListCollectionView(historial);
            OnPropertyChanged(nameof(historialProduccion));
        }

        public bool AgregarLinea()
        {
            if (idProductoSeleccionado <= 0 || cantidadProducto <= 0)
                return false;

            lineasProduccion.Add(new Produccion
            {
                IdProducto = idProductoSeleccionado,
                Cantidad = cantidadProducto,
                Fecha = fechaProduccion.Date
            });
            OnPropertyChanged(nameof(lineasProduccion));
            return true;
        }

        public void QuitarLinea(Produccion linea)
        {
            lineasProduccion.Remove(linea);
            OnPropertyChanged(nameof(lineasProduccion));
        }

        public async Task<bool> RegistrarProduccionAsync()
        {
            try
            {
                await _produccionRepository.RegistrarProduccionAsync(lineasProduccion.ToList());
                lineasProduccion = new ObservableCollection<Produccion>();
                OnPropertyChanged(nameof(lineasProduccion));
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
