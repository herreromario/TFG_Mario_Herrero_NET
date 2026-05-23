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
    public partial class UCListarClientes : UserControl
    {
        private readonly MVCliente _mvCliente;
        public event Action? SolicitarAñadirCliente;
        public event Action<Cliente>? SolicitarEditarCliente;

        public UCListarClientes(MVCliente mvCliente)
        {
            InitializeComponent();
            _mvCliente = mvCliente;
        }

        private async void UCListarClientes_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvCliente.Inicializa();
            DataContext = _mvCliente;
        }

        private void textoNombreCliente_TextChanged(object sender, TextChangedEventArgs e) => _mvCliente.Filtrar();

        private void btnAñadirCliente_Click(object sender, RoutedEventArgs e) => SolicitarAñadirCliente?.Invoke();

        private void EditarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is not Cliente cliente) return;
            SolicitarEditarCliente?.Invoke(cliente);
        }

        private async void EliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is not Cliente cliente) return;

            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar el cliente '{cliente.Nombre}'?")) return;

            var ok = await _mvCliente.EliminarClienteAsync(cliente);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Cliente eliminado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvCliente.error ?? "No se pudo eliminar el cliente.");
        }

        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null)
            {
                row.IsSelected = true;
                dgClientes.SelectedItem = row.Item;
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
