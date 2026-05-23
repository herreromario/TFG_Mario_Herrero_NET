using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Base;
using StockMeal.Backend.Repositorios;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace StockMeal.Backend.MVVM.Propios
{
    public class MVCompra : MVBase
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IProductoRepository _productoRepository;

        public Compra compra { get; set; } = new Compra { Fecha = DateTime.Now };
        public List<Proveedor> listaProveedores { get; private set; } = new();
        public List<Producto> listaIngredientesCompra { get; private set; } = new();

        public ObservableCollection<DetalleCompra> detallesCompra { get; private set; } = new();
        public ListCollectionView listaCompras { get; private set; }
        public List<Compra> comprasDetalle { get; private set; } = new();

        public int idProductoDetalleSeleccionado { get; set; }
        public int cantidadDetalle { get; set; } = 1;
        public decimal precioUnitarioDetalle { get; set; }

        public string? error { get; set; }

        public MVCompra(
            ICompraRepository compraRepository,
            IProveedorRepository proveedorRepository,
            IProductoRepository productoRepository)
        {
            _compraRepository = compraRepository;
            _proveedorRepository = proveedorRepository;
            _productoRepository = productoRepository;
        }

        public async Task Inicializa()
        {
            listaProveedores = await _proveedorRepository.GetProveedoresAsync();
            OnPropertyChanged(nameof(listaProveedores));

            listaIngredientesCompra = await _productoRepository.GetIngredientesAsync();
            OnPropertyChanged(nameof(listaIngredientesCompra));

            comprasDetalle = await _compraRepository.GetComprasConProveedorAsync();
            OnPropertyChanged(nameof(comprasDetalle));

            var historialLineas = await _compraRepository.GetHistorialComprasLineasAsync();
            listaCompras = new ListCollectionView(historialLineas);
            OnPropertyChanged(nameof(listaCompras));
        }

        public void SeleccionarProductoDetalle()
        {
            var producto = listaIngredientesCompra.FirstOrDefault(p => p.IdProducto == idProductoDetalleSeleccionado);
            if (producto?.Precio != null)
            {
                precioUnitarioDetalle = producto.Precio.Value;
                OnPropertyChanged(nameof(precioUnitarioDetalle));
            }
        }

        public bool AnadirDetalleActual()
        {
            if (idProductoDetalleSeleccionado <= 0 || cantidadDetalle <= 0 || precioUnitarioDetalle <= 0)
                return false;

            var detalle = new DetalleCompra
            {
                IdProducto = idProductoDetalleSeleccionado,
                Cantidad = cantidadDetalle,
                PrecioUnitario = precioUnitarioDetalle,
                Subtotal = cantidadDetalle * precioUnitarioDetalle
            };

            detallesCompra.Add(detalle);
            OnPropertyChanged(nameof(detallesCompra));

            compra.ImporteTotal = detallesCompra.Sum(d => d.Subtotal);
            OnPropertyChanged(nameof(compra));

            cantidadDetalle = 1;
            precioUnitarioDetalle = 0;
            OnPropertyChanged(nameof(cantidadDetalle));
            OnPropertyChanged(nameof(precioUnitarioDetalle));
            return true;
        }

        public void QuitarDetalle(DetalleCompra detalle)
        {
            if (detalle == null) return;
            detallesCompra.Remove(detalle);
            compra.ImporteTotal = detallesCompra.Sum(d => d.Subtotal);
            OnPropertyChanged(nameof(detallesCompra));
            OnPropertyChanged(nameof(compra));
        }

        public async Task<bool> AñadirCompraAsync()
        {
            try
            {
                if (!detallesCompra.Any())
                    throw new Exception("Debe añadir al menos una línea de detalle.");

                if (compra.Fecha == null)
                    compra.Fecha = DateTime.Now;

                await _compraRepository.RegistrarCompraConDetallesYActualizarStockAsync(compra, detallesCompra.ToList());

                compra = new Compra { Fecha = DateTime.Now };
                detallesCompra = new ObservableCollection<DetalleCompra>();
                idProductoDetalleSeleccionado = 0;
                cantidadDetalle = 1;
                precioUnitarioDetalle = 0;

                OnPropertyChanged(nameof(compra));
                OnPropertyChanged(nameof(detallesCompra));
                OnPropertyChanged(nameof(idProductoDetalleSeleccionado));
                OnPropertyChanged(nameof(cantidadDetalle));
                OnPropertyChanged(nameof(precioUnitarioDetalle));
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
