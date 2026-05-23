namespace StockMeal.Backend.Modelos
{
    public class HistorialCompraLinea
    {
        public int IdCompra { get; set; }
        public DateTime? FechaCompra { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
