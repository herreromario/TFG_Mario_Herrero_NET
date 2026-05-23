using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("cliente")]
public partial class Cliente
{
    [Key]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    [StringLength(50)]
    public string? Apellido { get; set; }

    [Column("telefono")]
    [StringLength(9)]
    public string? Telefono { get; set; }

    [Column("tipo_cliente", TypeName = "enum('habitual','nuevo','grupo')")]
    public string TipoCliente { get; set; } = null!;

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<PedidoVentum> PedidoVenta { get; set; } = new List<PedidoVentum>();
}
