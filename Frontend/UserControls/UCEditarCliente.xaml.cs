using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarCliente : UserControl
    {
        private readonly MVCliente _mvCliente;
        public event Action? SolicitarVolver;

        public UCEditarCliente(MVCliente mvCliente, Cliente cliente)
        {
            InitializeComponent();
            _mvCliente = mvCliente;
            _mvCliente.cliente = new Cliente
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono,
                TipoCliente = cliente.TipoCliente
            };
            DataContext = _mvCliente;
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvCliente.ActualizarClienteAsync(_mvCliente.cliente);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Cliente actualizado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvCliente.error ?? "No se pudo actualizar el cliente.");
            if (ok) SolicitarVolver?.Invoke();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
