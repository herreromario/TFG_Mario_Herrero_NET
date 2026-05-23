using StockMeal.Backend.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StockMeal.Frontend.UserControls
{
    public partial class UCDetalleCompra : UserControl
    {
        public event Action? SolicitarVolver;

        public string Titulo { get; }
        public string Resumen { get; }
        public List<LineaDetalleCompra> Lineas { get; }

        public UCDetalleCompra(Compra compra)
        {
            InitializeComponent();

            Titulo = $"Detalle de compra #{compra.IdCompra}";
            Resumen = $"Proveedor: {compra.IdProveedorNavigation?.Nombre ?? "-"} | Fecha: {compra.Fecha:dd/MM/yyyy HH:mm} | Importe: {compra.ImporteTotal:F2}";
            Lineas = (compra.DetalleCompras ?? new List<DetalleCompra>())
                .Select(d => new LineaDetalleCompra
                {
                    Producto = d.IdProductoNavigation?.Nombre ?? $"Producto {d.IdProducto}",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                })
                .ToList();

            DataContext = this;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e) => SolicitarVolver?.Invoke();
    }

    public class LineaDetalleCompra
    {
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
