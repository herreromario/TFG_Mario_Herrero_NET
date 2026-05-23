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
    public partial class UCListarPermisos : UserControl
    {
        private readonly MVPermiso _mv;
        public event Action? SolicitarAnadir;
        public event Action<Permiso>? SolicitarEditar;
        public UCListarPermisos(MVPermiso mvPermiso) { InitializeComponent(); _mv = mvPermiso; }
        private async void UCListarPermisos_Loaded(object sender, RoutedEventArgs e) { await _mv.Inicializa(); DataContext = _mv; }
        private void Anadir_Click(object sender, RoutedEventArgs e) => SolicitarAnadir?.Invoke();
        private void Editar_Click(object sender, RoutedEventArgs e) { if (dg.SelectedItem is Permiso p) SolicitarEditar?.Invoke(p); }
        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem is not Permiso p) return;
            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar permiso '{p.Nombre}'?")) return;
            var ok = await _mv.EliminarAsync(p);
            if (ok) Mensajes.Exito(Window.GetWindow(this), "Permiso eliminado correctamente.");
            else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo eliminar.");
        }
        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) { var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject); if (row != null) { row.IsSelected = true; dg.SelectedItem = row.Item; } }
        private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject { while (child != null) { if (child is T p) return p; child = VisualTreeHelper.GetParent(child); } return null; }
    }
}
