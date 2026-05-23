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
    public partial class UCListarProveedores : UserControl
    {
        private readonly MVProveedor _mvProveedor;
        public event Action? SolicitarAñadirProveedor;
        public event Action<Proveedor>? SolicitarEditarProveedor;

        public UCListarProveedores(MVProveedor mvProveedor)
        {
            InitializeComponent();
            _mvProveedor = mvProveedor;
        }

        private async void UCListarProveedores_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvProveedor.Inicializa();
            DataContext = _mvProveedor;
        }

        private void textoNombreProveedor_TextChanged(object sender, TextChangedEventArgs e) => _mvProveedor.Filtrar();

        private void btnAñadirProveedor_Click(object sender, RoutedEventArgs e) => SolicitarAñadirProveedor?.Invoke();

        private void EditarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (dgProveedores.SelectedItem is not Proveedor proveedor) return;
            SolicitarEditarProveedor?.Invoke(proveedor);
        }

        private async void EliminarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (dgProveedores.SelectedItem is not Proveedor proveedor) return;

            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar el proveedor '{proveedor.Nombre}'?")) return;

            var ok = await _mvProveedor.EliminarProveedorAsync(proveedor);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Proveedor eliminado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvProveedor.error ?? "No se pudo eliminar el proveedor.");
        }

        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null)
            {
                row.IsSelected = true;
                dgProveedores.SelectedItem = row.Item;
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
