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
    public partial class UCListarPlatos : UserControl
    {
        private readonly MVPlato _mvPlato;

        public event Action? SolicitarAñadirPlato;
        public event Action<Producto>? SolicitarEditarPlato;

        public UCListarPlatos(MVPlato mVPlato)
        {
            InitializeComponent();
            _mvPlato = mVPlato;
        }

        private async void UCListarPlatos_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvPlato.Inicializa();
            DataContext = _mvPlato;
        }

        private void textoNombrePlato_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mvPlato.Filtrar();
        }

        private void btnAñadirPlato_Click(object sender, RoutedEventArgs e)
        {
            SolicitarAñadirPlato?.Invoke();
        }

        private void EditarPlato_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlatos.SelectedItem is not Producto plato) return;
            SolicitarEditarPlato?.Invoke(plato);
        }

        private async void EliminarPlato_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlatos.SelectedItem is not Producto plato) return;

            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar el plato '{plato.Nombre}'?")) return;

            var ok = await _mvPlato.EliminarPlatoAsync(plato);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Plato eliminado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvPlato.error ?? "No se pudo eliminar el plato.");
        }

        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null)
            {
                row.IsSelected = true;
                dgPlatos.SelectedItem = row.Item;
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
