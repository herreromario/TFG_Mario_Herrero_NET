using StockMeal.Backend.Modelos;
using StockMeal.Backend.MVVM.Propios;
using StockMeal.Frontend.Dialogos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCEditarIngrediente : UserControl
    {
        private readonly MVIngrediente _mvIngrediente;
        private readonly Producto _ingredienteEditando;
        public event Action? SolicitarVolver;

        public UCEditarIngrediente(MVIngrediente mvIngrediente, Producto ingrediente)
        {
            InitializeComponent();
            _mvIngrediente = mvIngrediente;
            _ingredienteEditando = new Producto
            {
                IdProducto = ingrediente.IdProducto,
                Nombre = ingrediente.Nombre,
                Descripcion = ingrediente.Descripcion,
                Precio = ingrediente.Precio,
                StockDisponible = ingrediente.StockDisponible,
                StockMinimo = ingrediente.StockMinimo,
                Tipo = ingrediente.Tipo,
                Unidad = ingrediente.Unidad
            };
        }

        private async void UCEditarIngrediente_Loaded(object sender, RoutedEventArgs e)
        {
            await _mvIngrediente.Inicializa();
            _mvIngrediente.ingrediente = _ingredienteEditando;
            DataContext = _mvIngrediente;
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            var ok = await _mvIngrediente.ActualizarIngredienteAsync(_mvIngrediente.ingrediente);
            if (ok)
                Mensajes.Exito(Window.GetWindow(this), "Ingrediente actualizado correctamente.");
            else
                Mensajes.Error(Window.GetWindow(this), _mvIngrediente.error ?? "No se pudo actualizar el ingrediente.");
            if (ok) SolicitarVolver?.Invoke();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }
}
