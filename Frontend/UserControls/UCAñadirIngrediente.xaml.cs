using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockMeal.Frontend.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCAñadirIngrediente.xaml
    /// </summary>
    public partial class UCAñadirIngrediente : UserControl
    {
        private MVIngrediente _mvIngrediente;

        public event Action? SolicitarVolver;
        public UCAñadirIngrediente(MVIngrediente mvIngrediente)
        {
            InitializeComponent();
            _mvIngrediente = mvIngrediente;
            DataContext = mvIngrediente;
            this.AddHandler(
                Validation.ErrorEvent,
                new RoutedEventHandler(mvIngrediente.OnErrorEvent));
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SolicitarVolver?.Invoke();
        }

        private async void btnAñadirIngrediente_Click(object sender, RoutedEventArgs e)
        {
            bool resultado = await _mvIngrediente.AñadirIngredienteAsync();

            try
            {
                if (resultado)
                {
                    Mensajes.Exito(Window.GetWindow(this), "Ingrediente añadido correctamente.");
                    SolicitarVolver?.Invoke();
                }
                else
                {
                    Mensajes.Error(Window.GetWindow(this), _mvIngrediente.error ?? "No se pudo añadir el ingrediente. Por favor, revise los datos e inténtelo de nuevo.");
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(Window.GetWindow(this), $"Error al añadir el ingrediente: {ex.Message}");
                return;
            }
        }

        private async void AñadirIngrediente_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvIngrediente.Inicializa();
        }
    }
}
