using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("compra")]
[Index("IdProveedor", Name = "id_proveedor")]
public partial class Compra
{
    [Key]
    [Column("id_compra")]
    public int IdCompra { get; set; }

    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("importe_total")]
    [Precision(10)]
    public decimal ImporteTotal { get; set; }

    [InverseProperty("IdCompraNavigation")]
    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    [ForeignKey("IdProveedor")]
    [InverseProperty("Compras")]
    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    [InverseProperty("IdCompraNavigation")]
    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();
}
