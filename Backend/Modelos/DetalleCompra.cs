using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("detalle_compra")]
[Index("IdCompra", Name = "id_compra")]
[Index("IdProducto", Name = "id_producto")]
public partial class DetalleCompra
{
    [Key]
    [Column("id_detalle")]
    public int IdDetalle { get; set; }

    [Column("id_compra")]
    public int IdCompra { get; set; }

    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("precio_unitario")]
    [Precision(10)]
    public decimal PrecioUnitario { get; set; }

    [Column("subtotal")]
    [Precision(10)]
    public decimal Subtotal { get; set; }

    [ForeignKey("IdCompra")]
    [InverseProperty("DetalleCompras")]
    public virtual Compra IdCompraNavigation { get; set; } = null!;

    [ForeignKey("IdProducto")]
    [InverseProperty("DetalleCompras")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
