using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("cierre_caja")]
[Index("Fecha", Name = "fecha", IsUnique = true)]
public partial class CierreCaja
{
    [Key]
    [Column("id_cierre")]
    public int IdCierre { get; set; }

    [Column("fecha", TypeName = "date")]
    public DateTime Fecha { get; set; }

    [Column("total_ingresos")]
    [Precision(10)]
    public decimal TotalIngresos { get; set; }

    [Column("total_gastos")]
    [Precision(10)]
    public decimal TotalGastos { get; set; }

    [Column("saldo_final")]
    [Precision(10)]
    public decimal SaldoFinal { get; set; }

    [InverseProperty("IdCierreNavigation")]
    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();
}
