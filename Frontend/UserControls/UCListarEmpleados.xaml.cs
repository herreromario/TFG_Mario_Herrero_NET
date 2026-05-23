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
    public partial class UCListarEmpleados : UserControl
    {
        private readonly MVEmpleado _mvEmpleado;
        public event Action? SolicitarAnadir;
        public event Action<Empleado>? SolicitarEditar;

        public UCListarEmpleados(MVEmpleado mvEmpleado) { InitializeComponent(); _mvEmpleado = mvEmpleado; }
        private async void UCListarEmpleados_Loaded(object sender, RoutedEventArgs e) { await _mvEmpleado.Inicializa(); DataContext = _mvEmpleado; }
        private void Anadir_Click(object sender, RoutedEventArgs e) => SolicitarAnadir?.Invoke();
        private void Editar_Click(object sender, RoutedEventArgs e) { if (dg.SelectedItem is Empleado em) SolicitarEditar?.Invoke(em); }
        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem is not Empleado em) return;
            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar empleado '{em.Nombre} {em.Apellido}'?")) return;
            var ok = await _mvEmpleado.EliminarAsync(em);
            if (ok) Mensajes.Exito(Window.GetWindow(this), "Empleado eliminado correctamente.");
            else Mensajes.Error(Window.GetWindow(this), _mvEmpleado.error ?? "No se pudo eliminar.");
        }
        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null) { row.IsSelected = true; dg.SelectedItem = row.Item; }
        }
        private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject { while (child != null) { if (child is T p) return p; child = VisualTreeHelper.GetParent(child); } return null; }
    }
}
