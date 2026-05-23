using StockMeal.Backend.MVVM.Propios;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCHistorialProduccion : UserControl
    {
        private readonly MVProduccion _mv;
        public UCHistorialProduccion(MVProduccion mv)
        {
            InitializeComponent();
            _mv = mv;
        }

        private async void UCHistorialProduccion_Loaded(object sender, RoutedEventArgs e)
        {
            await _mv.Inicializa();
            DataContext = _mv;
        }
    }
}
