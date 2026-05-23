using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("empleado")]
[Index("Dni", Name = "DNI", IsUnique = true)]
[Index("IdRol", Name = "id_rol")]
[Index("Usuario", Name = "usuario", IsUnique = true)]
public partial class Empleado
{
    [Key]
    [Column("id_empleado")]
    public int IdEmpleado { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    [StringLength(50)]
    public string Apellido { get; set; } = null!;

    [Column("DNI")]
    [StringLength(20)]
    public string Dni { get; set; } = null!;

    [Column("telefono")]
    [StringLength(9)]
    public string? Telefono { get; set; }

    [Column("direccion")]
    [StringLength(100)]
    public string? Direccion { get; set; }

    [Column("usuario")]
    [StringLength(50)]
    public string Usuario { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("id_rol")]
    public int? IdRol { get; set; }

    [ForeignKey("IdRol")]
    [InverseProperty("Empleados")]
    public virtual Rol? IdRolNavigation { get; set; }

    [InverseProperty("IdEmpleadoNavigation")]
    public virtual ICollection<PedidoVentum> PedidoVenta { get; set; } = new List<PedidoVentum>();
}
