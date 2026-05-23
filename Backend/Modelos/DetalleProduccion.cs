using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMeal.Backend.Modelos
{
    [Table("detalle_produccion")]
    public class DetalleProduccion
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("id_produccion")]
        public int IdProduccion { get; set; }

        [Required]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Column("cantidad")]
        public int Cantidad { get; set; }

        [ForeignKey(nameof(IdProduccion))]
        public virtual ProduccionDiaria? Produccion { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public virtual Producto? Producto { get; set; }
    }
}

