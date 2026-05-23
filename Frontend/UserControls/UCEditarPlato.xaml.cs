using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarPlato : UserControl
    {
        private readonly MVPlato _mvPlato;
        public event Action? SolicitarVolver;

        public UCEditarPlato(MVPlato mvPlato, Producto plato)
        {
            InitializeComponent();
            _mvPlato = mvPlato;
            _mvPlato.plato = new Producto
            {
                IdProducto = plato.IdProducto,
                Nombre = plato.Nombre,
                Descripcion = plato.Descripcion,
                Precio = plato.Precio,
                StockDisponible = plato.StockDisponible,
                StockMinimo = plato.StockMinimo,
                Tipo = plato.Tipo,
                Unidad = plato.Unidad
            };
            DataContext = _mvPlato;
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvPlato.ActualizarPlatoAsync(_mvPlato.plato);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Plato actualizado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvPlato.error ?? "No se pudo actualizar el plato.");
            if (ok) SolicitarVolver?.Invoke();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
