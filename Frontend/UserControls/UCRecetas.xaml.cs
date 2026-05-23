using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCRecetas : UserControl
    {
        private readonly MVReceta _mv;
        public UCRecetas(MVReceta mv)
        {
            InitializeComponent();
            _mv = mv;
            DataContext = _mv;
        }

        private async void UCRecetas_Loaded(object sender, RoutedEventArgs e) => await _mv.Inicializa();

        private void btnAnadirLinea_Click(object sender, RoutedEventArgs e)
        {
            if (!_mv.AgregarLineaReceta())
                Mensajes.Error(Window.GetWindow(this), "Completa plato, ingrediente y cantidad.");
        }

        private async void btnGuardarReceta_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mv.GuardarRecetaAsync();
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Receta guardada.");
            else
                Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo guardar la receta.");
        }
    }
}
