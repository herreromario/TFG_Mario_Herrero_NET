using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCAñadirCompra : UserControl
    {
        private readonly MVCompra _mvCompra;
        public event Action? SolicitarVolver;

        public UCAñadirCompra(MVCompra mvCompra)
        {
            InitializeComponent();
            _mvCompra = mvCompra;
            DataContext = mvCompra;
        }

        private async void AñadirCompra_Loaded(object sender, RoutedEventArgs e) => await _mvCompra.Inicializa();

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvCompra.AñadirCompraAsync();
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Compra añadida correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvCompra.error ?? "No se pudo guardar la compra. Por favor, revise los datos e inténtelo de nuevo.");
            if (ok) SolicitarVolver?.Invoke();
        }

        private void cmbProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mvCompra.SeleccionarProductoDetalle();
        }

        private void btnAñadirDetalle_Click(object sender, RoutedEventArgs e)
        {
            var ok = _mvCompra.AnadirDetalleActual();
            if (!ok)
                Mensajes.Error(Window.GetWindow(this), "Revisa producto, cantidad y precio para añadir la línea.");
        }

        private void btnQuitarDetalle_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is DetalleCompra detalle)
                _mvCompra.QuitarDetalle(detalle);
        }
    }
}
