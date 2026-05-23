using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarEmpleado : UserControl
    {
        private readonly MVEmpleado _mv;
        public event Action? SolicitarVolver;
        public UCEditarEmpleado(MVEmpleado mv, Empleado empleado)
        {
            InitializeComponent();
            _mv = mv;
            _mv.empleado = new Empleado
            {
                IdEmpleado = empleado.IdEmpleado,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Dni = empleado.Dni,
                Telefono = empleado.Telefono,
                Direccion = empleado.Direccion,
                Usuario = empleado.Usuario,
                Password = empleado.Password,
                IdRol = empleado.IdRol
            };
            DataContext = _mv;
        }
        private async void LoadedHandler(object sender, RoutedEventArgs e) => await _mv.Inicializa();
        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mv.ActualizarAsync(_mv.empleado);
            if (ok) { Mensajes.Exito(Window.GetWindow(this), "Empleado actualizado correctamente."); SolicitarVolver?.Invoke(); }
            else Mensajes.Error(Window.GetWindow(this), _mv.error ?? "No se pudo actualizar.");
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
