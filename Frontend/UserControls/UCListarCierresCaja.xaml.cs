using StockMeal.Backend.MVVM.Propios;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarCierresCaja : UserControl
    {
        private readonly MVCierreCaja _mvCierreCaja;

        public UCListarCierresCaja(MVCierreCaja mvCierreCaja)
        {
            InitializeComponent();
            _mvCierreCaja = mvCierreCaja;
        }

        private async void UCListarCierresCaja_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvCierreCaja.Inicializa();
            DataContext = _mvCierreCaja;
        }
    }
}
