using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("factura")]
[Index("IdTransaccion", Name = "fk_factura_transaccion")]
[Index("NumeroFactura", Name = "numero_factura", IsUnique = true)]
public partial class Factura
{
    [Key]
    [Column("id_factura")]
    public int IdFactura { get; set; }

    [Column("id_transaccion")]
    public int IdTransaccion { get; set; }

    [Column("numero_factura")]
    [StringLength(20)]
    public string NumeroFactura { get; set; } = null!;

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("base_imponible")]
    [Precision(10)]
    public decimal BaseImponible { get; set; }

    [Column("iva_porcentaje")]
    [Precision(5)]
    public decimal IvaPorcentaje { get; set; }

    [Column("total_con_iva")]
    [Precision(10)]
    public decimal TotalConIva { get; set; }

    [ForeignKey("IdTransaccion")]
    [InverseProperty("Facturas")]
    public virtual PedidoVentum IdTransaccionNavigation { get; set; } = null!;
}
