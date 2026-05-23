using StockMeal.Backend.MVVM.Propios;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarRecetas : UserControl
    {
        private readonly MVReceta _mv;

        public event Action? SolicitarAgregarReceta;

        public UCListarRecetas(MVReceta mv)
        {
            InitializeComponent();
            _mv = mv;
        }

        private async void UCListarRecetas_Loaded(object sender, RoutedEventArgs e)
        {
            await _mv.Inicializa();
            DataContext = _mv;
        }

        private void btnAgregarReceta_Click(object sender, RoutedEventArgs e)
        {
            SolicitarAgregarReceta?.Invoke();
        }
    }
}
