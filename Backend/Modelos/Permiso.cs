using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[Table("permiso")]
public partial class Permiso
{
    [Key]
    [Column("id_permiso")]
    public int IdPermiso { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion", TypeName = "text")]
    public string? Descripcion { get; set; }

    [ForeignKey("IdPermiso")]
    [InverseProperty("IdPermisos")]
    public virtual ICollection<Rol> IdRols { get; set; } = new List<Rol>();
}
