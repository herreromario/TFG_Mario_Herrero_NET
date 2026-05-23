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
    public partial class UCListarRoles : UserControl
    {
        private readonly MVRol _mv;
        public event Action? SolicitarAnadir;
        public event Action<Rol>? SolicitarEditar;
        public UCListarRoles(MVRol mvRol) { InitializeComponent(); _mv = mvRol; }
        private async void UCListarRoles_Loaded(object sender, RoutedEventArgs e) { await _mv.Inicializa(); DataContext = _mv; }
        private void Anadir_Click(object sender, RoutedEventArgs e) => SolicitarAnadir?.Invoke();
        private void Editar_Click(object sender, RoutedEventArgs e) { if (dg.SelectedItem is Rol r) SolicitarEditar?.Invoke(r); }
        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem is not Rol r) return;
            if (!Mensajes.Confirmar(Window.GetWindow(this), $"¿Eliminar rol '{r.Nombre}'?")) return;
            var ok = await _mv.EliminarAsync(r);
            if (ok) Mensajes.Exito(Window.GetWindow(this), "Rol eliminado correctamente.");
            else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo eliminar.");
        }
        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) { var row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject); if (row != null) { row.IsSelected = true; dg.SelectedItem = row.Item; } }
        private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject { while (child != null) { if (child is T p) return p; child = VisualTreeHelper.GetParent(child); } return null; }
    }
}
