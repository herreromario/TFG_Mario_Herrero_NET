using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMeal.Backend.Modelos
{
    [Table("produccion_diaria")]
    public class ProduccionDiaria
    {
        [Key]
        [Column("id_produccion")]
        public int IdProduccion { get; set; }

        [Required]
        [Column("fecha", TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column("id_empleado")]
        public int IdEmpleado { get; set; }

        [Column("observaciones")]
        public string? Observaciones { get; set; }

        // Navegación
        [ForeignKey(nameof(IdEmpleado))]
        public virtual Empleado? Empleado { get; set; }

        public virtual ICollection<DetalleProduccion> Detalles { get; set; }
            = new List<DetalleProduccion>();
    }
}

