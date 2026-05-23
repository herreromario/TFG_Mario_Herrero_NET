namespace StockMeal.Backend.Modelos
{
    public class HistorialProduccionLinea
    {
        public int IdProduccion { get; set; }
        public DateTime Fecha { get; set; }
        public string Empleado { get; set; } = string.Empty;
        public string Plato { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
