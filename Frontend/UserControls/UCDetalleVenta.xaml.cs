using StockMeal.Backend.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCDetalleVenta : UserControl
    {
        public event Action? SolicitarVolver;

        public string Titulo { get; }
        public string Resumen { get; }
        public List<LineaDetalleVenta> Lineas { get; }

        public UCDetalleVenta(PedidoVentum venta)
        {
            InitializeComponent();

            Titulo = $"Detalle de venta #{venta.IdTransaccion}";
            Resumen = $"Cliente: {venta.IdClienteNavigation?.Nombre ?? "-"} | Fecha: {venta.Fecha:dd/MM/yyyy HH:mm} | Importe: {venta.ImporteTotal:F2}";
            Lineas = (venta.DetallePedidos ?? new List<DetallePedido>())
                .Select(d => new LineaDetalleVenta
                {
                    Producto = d.IdProductoNavigation?.Nombre ?? $"Producto {d.IdProducto}",
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                })
                .ToList();

            DataContext = this;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }

    public class LineaDetalleVenta
    {
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
