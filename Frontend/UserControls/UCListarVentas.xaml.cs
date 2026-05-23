using StockMeal.Backend.MVVM.Propios;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarVentas : UserControl
    {
        private readonly MVPedidoVenta _mvPedidoVenta;
        public event Action<Backend.Modelos.PedidoVentum>? SolicitarVerDetalleVenta;

        public UCListarVentas(MVPedidoVenta mvPedidoVenta)
        {
            InitializeComponent();
            _mvPedidoVenta = mvPedidoVenta;
        }

        private async void UCListarVentas_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvPedidoVenta.Inicializa();
            DataContext = _mvPedidoVenta;
        }

        private void btnVerDetalle_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Backend.Modelos.PedidoVentum venta)
                SolicitarVerDetalleVenta?.Invoke(venta);
        }
    }
}
