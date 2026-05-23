using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCAñadirEmpleado : UserControl
    {
        private readonly MVEmpleado _mv;
        public event Action? SolicitarVolver;
        public UCAñadirEmpleado(MVEmpleado mv) { InitializeComponent(); _mv = mv; DataContext = _mv; }
        private async void LoadedHandler(object sender, RoutedEventArgs e) => await _mv.Inicializa();
        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            _mv.empleado.Password = PasswordBox.Password;
            var ok = await _mv.GuardarNuevoAsync();
            if (ok) { Mensajes.Exito(Window.GetWindow(this), "Empleado añadido correctamente."); SolicitarVolver?.Invoke(); }
            else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo guardar.");
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
