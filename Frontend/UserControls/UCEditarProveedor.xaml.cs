using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarProveedor : UserControl
    {
        private readonly MVProveedor _mvProveedor;
        public event Action? SolicitarVolver;

        public UCEditarProveedor(MVProveedor mvProveedor, Proveedor proveedor)
        {
            InitializeComponent();
            _mvProveedor = mvProveedor;
            _mvProveedor.proveedor = new Proveedor
            {
                IdProveedor = proveedor.IdProveedor,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion
            };
            DataContext = _mvProveedor;
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvProveedor.ActualizarProveedorAsync(_mvProveedor.proveedor);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Proveedor actualizado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvProveedor.error ?? "No se pudo actualizar el proveedor.");
            if (ok) SolicitarVolver?.Invoke();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
