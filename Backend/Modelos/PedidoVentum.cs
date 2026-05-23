using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("pedido_venta")]
[Index("IdCliente", Name = "id_cliente")]
[Index("IdEmpleado", Name = "id_empleado")]
public partial class PedidoVentum
{
    [Key]
    [Column("id_transaccion")]
    public int IdTransaccion { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("id_empleado")]
    public int? IdEmpleado { get; set; }

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("tipo_transaccion", TypeName = "enum('pedido','venta')")]
    public string TipoTransaccion { get; set; } = null!;

    [Column("estado", TypeName = "enum('pendiente','confirmado','cancelado','entregado')")]
    public string Estado { get; set; } = null!;

    [Column("importe_total")]
    [Precision(10)]
    public decimal ImporteTotal { get; set; }

    [InverseProperty("IdTransaccionNavigation")]
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    [InverseProperty("IdTransaccionNavigation")]
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    [ForeignKey("IdCliente")]
    [InverseProperty("PedidoVenta")]
    public virtual Cliente? IdClienteNavigation { get; set; }

    [ForeignKey("IdEmpleado")]
    [InverseProperty("PedidoVenta")]
    public virtual Empleado? IdEmpleadoNavigation { get; set; }

    [InverseProperty("IdTransaccionNavigation")]
    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();
}
