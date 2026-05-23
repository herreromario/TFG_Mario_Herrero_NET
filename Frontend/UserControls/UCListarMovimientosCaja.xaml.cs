using StockMeal.Backend.MVVM.Propios;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarMovimientosCaja : UserControl
    {
        private readonly MVMovimientoCaja _mvMovimientoCaja;

        public UCListarMovimientosCaja(MVMovimientoCaja mvMovimientoCaja)
        {
            InitializeComponent();
            _mvMovimientoCaja = mvMovimientoCaja;
        }

        private async void UCListarMovimientosCaja_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvMovimientoCaja.Inicializa();
            DataContext = _mvMovimientoCaja;
        }
    }
}
