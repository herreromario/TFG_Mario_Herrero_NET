using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCAjustesStock : UserControl
    {
        private readonly MVAjustesStock _mv;

        public UCAjustesStock(MVAjustesStock mv)
        {
            InitializeComponent();
            _mv = mv;
            DataContext = _mv;
        }

        private async void UCAjustesStock_Loaded(object sender, RoutedEventArgs e)
        {
            await _mv.Inicializa();
        }

        private async void GuardarFila_Click(object sender, RoutedEventArgs e)
        {
            if (dgStock.SelectedItem is not Producto ingrediente)
            {
                Mensajes.Error(Window.GetWindow(this), "Selecciona una fila para guardar.");
                return;
            }

            var ok = await _mv.GuardarIngredienteAsync(ingrediente);
            if (ok)
            {
                Mensajes.Exito(Window.GetWindow(this), "Stock actualizado correctamente.");
                await _mv.Inicializa();
            }
            else
            {
                Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo actualizar el stock.");
            }
        }

        private async void Refrescar_Click(object sender, RoutedEventArgs e)
        {
            await _mv.Inicializa();
        }
    }
}
