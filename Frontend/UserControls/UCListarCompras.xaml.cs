using StockMeal.Backend.MVVM.Propios;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarCompras : UserControl
    {
        private readonly MVCompra _mvCompra;
        public event Action<Backend.Modelos.Compra>? SolicitarVerDetalleCompra;

        public UCListarCompras(MVCompra mvCompra)
        {
            InitializeComponent();
            _mvCompra = mvCompra;
        }

        private async void UCListarCompras_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvCompra.Inicializa();
            DataContext = _mvCompra;
        }

        private void btnVerDetalle_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Backend.Modelos.HistorialCompraLinea linea)
                return;

            var compra = _mvCompra.comprasDetalle.FirstOrDefault(c => c.IdCompra == linea.IdCompra);
            if (compra != null)
                SolicitarVerDetalleCompra?.Invoke(compra);
        }
    }
}
