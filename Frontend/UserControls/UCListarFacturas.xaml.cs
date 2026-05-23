using StockMeal.Backend.MVVM.Propios;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarFacturas : UserControl
    {
        private readonly MVFactura _mvFactura;

        public UCListarFacturas(MVFactura mvFactura)
        {
            InitializeComponent();
            _mvFactura = mvFactura;
        }

        private async void UCListarFacturas_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvFactura.Inicializa();
            DataContext = _mvFactura;
        }
    }
}
