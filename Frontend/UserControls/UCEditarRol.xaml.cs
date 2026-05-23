using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarRol : UserControl
    {
        private readonly MVRol _mv;
        public event Action? SolicitarVolver;
        public UCEditarRol(MVRol mv, Rol rol)
        {
            InitializeComponent();
            _mv = mv;
            _mv.rol = new Rol { IdRol = rol.IdRol, Nombre = rol.Nombre, Descripcion = rol.Descripcion };
            DataContext = _mv;
        }
        private async void Guardar_Click(object sender, RoutedEventArgs e) { var ok = await _mv.ActualizarAsync(_mv.rol); if (ok) { Mensajes.Exito(Window.GetWindow(this), "Rol actualizado correctamente."); SolicitarVolver?.Invoke(); } else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo actualizar."); }
        private void Cancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
