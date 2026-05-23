using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCAñadirPermiso : UserControl
    {
        private readonly MVPermiso _mv;
        public event Action? SolicitarVolver;
        public UCAñadirPermiso(MVPermiso mv) { InitializeComponent(); _mv = mv; DataContext = _mv; }
        private async void Guardar_Click(object sender, RoutedEventArgs e) { var ok = await _mv.GuardarNuevoAsync(); if (ok) { Mensajes.Exito(Window.GetWindow(this), "Permiso añadido correctamente."); SolicitarVolver?.Invoke(); } else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo guardar."); }
        private void Cancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
