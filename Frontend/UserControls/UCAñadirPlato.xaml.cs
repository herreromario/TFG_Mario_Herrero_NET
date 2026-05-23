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
    public partial class UCAñadirPlato : UserControl
    {
        private MVPlato _mvPlato;

        public event Action? SolicitarVolver;
        public UCAñadirPlato(MVPlato mvPlato)
        {
            InitializeComponent();
            _mvPlato = mvPlato;
            DataContext = mvPlato;
            this.AddHandler(
                Validation.ErrorEvent,
                new RoutedEventHandler(mvPlato.OnErrorEvent));
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SolicitarVolver?.Invoke();
        }

        private async void btnAñadirPlato_Click(object sender, RoutedEventArgs e)
        {
            bool resultado = await _mvPlato.AñadirPlatoAsync();

            try
            {
                if (resultado)
                {
                    Mensajes.Exito(Window.GetWindow(this), "Plato añadido correctamente.");
                    SolicitarVolver?.Invoke();
                }
                else
                {
                    Mensajes.Error(Window.GetWindow(this), _mvPlato.error ?? "No se pudo añadir el plato. Por favor, revise los datos e inténtelo de nuevo.");
                }
            }
            catch (Exception ex)
            {
                Mensajes.Error(Window.GetWindow(this), $"Error al añadir el plato: {ex.Message}");
                return;
            }
        }
    }
}
