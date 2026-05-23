using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCRegistrarProduccion : UserControl
    {
        private readonly MVProduccion _mv;
        public UCRegistrarProduccion(MVProduccion mv)
        {
            InitializeComponent();
            _mv = mv;
            DataContext = _mv;
        }

        private async void UCRegistrarProduccion_Loaded(object sender, RoutedEventArgs e) => await _mv.Inicializa();

        private void btnAnadirLinea_Click(object sender, RoutedEventArgs e)
        {
            if (!_mv.AgregarLinea())
                Mensajes.Error(Window.GetWindow(this), "Completa producto y cantidad.");
        }

        private void btnQuitarLinea_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is StockMeal.Backend.Modelos.Produccion d)
                _mv.QuitarLinea(d);
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mv.RegistrarProduccionAsync();
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Producción registrada.");
            else
                Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo registrar la producción.");
        }
    }
}
