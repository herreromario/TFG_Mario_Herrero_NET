using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCListarIngredientes : UserControl
    {
        private readonly MVIngrediente _mvIngrediente;

        public event Action? SolicitarAñadirIngrediente;
        public event Action<Producto>? SolicitarEditarIngrediente;

        public UCListarIngredientes(MVIngrediente mVIngrediente)
        {
            InitializeComponent();
            _mvIngrediente = mVIngrediente;
        }

        private async void UCListarIngredientes_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvIngrediente.Inicializa();
            DataContext = _mvIngrediente;
        }

        private void textoNombreIngrediente_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mvIngrediente.Filtrar();
        }

        private void btnAñadirIngrediente_Click(object sender, RoutedEventArgs e)
        {
            SolicitarAñadirIngrediente?.Invoke();
        }

        private void EditarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            if (dgIngredientes.SelectedItem is not Producto ingrediente) return;
            SolicitarEditarIngrediente?.Invoke(ingrediente);
        }

        private async void EliminarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            if (dgIngredientes.SelectedItem is not Producto ingrediente) return;

            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar el ingrediente '{ingrediente.Nombre}'?")) return;

            var ok = await _mvIngrediente.EliminarIngredienteAsync(ingrediente);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Ingrediente eliminado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvIngrediente.error ?? "No se pudo eliminar el ingrediente.");
        }

        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null)
            {
                row.IsSelected = true;
                dgIngredientes.SelectedItem = row.Item;
            }
        }

        private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                    return parent;
                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }
    }
}
