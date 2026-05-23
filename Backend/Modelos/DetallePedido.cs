using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("detalle_pedido")]
[Index("IdProducto", Name = "id_producto")]
[Index("IdTransaccion", Name = "id_transaccion")]
public partial class DetallePedido
{
    [Key]
    [Column("id_detalle")]
    public int IdDetalle { get; set; }

    [Column("id_transaccion")]
    public int IdTransaccion { get; set; }

    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("subtotal")]
    [Precision(10)]
    public decimal Subtotal { get; set; }

    [ForeignKey("IdProducto")]
    [InverseProperty("DetallePedidos")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;

    [ForeignKey("IdTransaccion")]
    [InverseProperty("DetallePedidos")]
    public virtual PedidoVentum IdTransaccionNavigation { get; set; } = null!;
}
