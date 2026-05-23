using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockMeal.Backend.Modelos;

[PrimaryKey("IdProducto", "IdIngrediente")]
[Table("receta")]
[Index("IdIngrediente", Name = "id_ingrediente")]
public partial class Recetum
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Key]
    [Column("id_ingrediente")]
    public int IdIngrediente { get; set; }

    [Column("cantidad")]
    [Precision(10)]
    public decimal Cantidad { get; set; }

    [ForeignKey("IdIngrediente")]
    [InverseProperty("RecetumIdIngredienteNavigations")]
    public virtual Producto IdIngredienteNavigation { get; set; } = null!;

    [ForeignKey("IdProducto")]
    [InverseProperty("RecetumIdProductoNavigations")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
