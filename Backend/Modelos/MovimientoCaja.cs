using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("movimiento_caja")]
[Index("IdCierre", Name = "id_cierre")]
[Index("IdCompra", Name = "id_compra")]
[Index("IdTransaccion", Name = "id_transaccion")]
public partial class MovimientoCaja
{
    [Key]
    [Column("id_movimiento")]
    public int IdMovimiento { get; set; }

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("tipo_movimiento", TypeName = "enum('ingreso','gasto')")]
    public string TipoMovimiento { get; set; } = null!;

    [Column("importe")]
    [Precision(10)]
    public decimal Importe { get; set; }

    [Column("descripcion", TypeName = "text")]
    public string? Descripcion { get; set; }

    [Column("id_transaccion")]
    public int? IdTransaccion { get; set; }

    [Column("id_compra")]
    public int? IdCompra { get; set; }

    [Column("id_cierre")]
    public int? IdCierre { get; set; }

    [ForeignKey("IdCierre")]
    [InverseProperty("MovimientoCajas")]
    public virtual CierreCaja? IdCierreNavigation { get; set; }

    [ForeignKey("IdCompra")]
    [InverseProperty("MovimientoCajas")]
    public virtual Compra? IdCompraNavigation { get; set; }

    [ForeignKey("IdTransaccion")]
    [InverseProperty("MovimientoCajas")]
    public virtual PedidoVentum? IdTransaccionNavigation { get; set; }
}
