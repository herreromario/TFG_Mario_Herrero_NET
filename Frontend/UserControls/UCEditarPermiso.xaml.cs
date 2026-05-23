using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarPermiso : UserControl
    {
        private readonly MVPermiso _mv;
        public event Action? SolicitarVolver;
        public UCEditarPermiso(MVPermiso mv, Permiso permiso)
        {
            InitializeComponent();
            _mv = mv;
            _mv.permiso = new Permiso { IdPermiso = permiso.IdPermiso, Nombre = permiso.Nombre, Descripcion = permiso.Descripcion };
            DataContext = _mv;
        }
        private async void Guardar_Click(object sender, RoutedEventArgs e) { var ok = await _mv.ActualizarAsync(_mv.permiso); if (ok) { Mensajes.Exito(Window.GetWindow(this), "Permiso actualizado correctamente."); SolicitarVolver?.Invoke(); } else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo actualizar."); }
        private void Cancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
