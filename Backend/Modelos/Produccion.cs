using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMeal.Backend.Modelos
{
    [Table("produccion")]
    public class Produccion
    {
        [Key]
        [Column("id_produccion")]
        public int IdProduccion { get; set; }

        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("fecha", TypeName = "date")]
        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public virtual Producto? Producto { get; set; }
    }
}
