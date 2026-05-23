using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("proveedor")]
public partial class Proveedor
{
    [Key]
    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("telefono")]
    [StringLength(9)]
    public string? Telefono { get; set; }

    [Column("direccion")]
    [StringLength(100)]
    public string? Direccion { get; set; }

    [InverseProperty("IdProveedorNavigation")]
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
