using StockMeal.Backend.MVVM.Propios;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCDashboard : UserControl
    {
        private readonly MVDashboard _mv;

        public UCDashboard(MVDashboard mv)
        {
            InitializeComponent();
            _mv = mv;
            DataContext = _mv;
        }

        private async void UCDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            await _mv.Inicializa();
        }
    }
}
