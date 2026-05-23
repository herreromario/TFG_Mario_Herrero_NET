using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using StockMeal.Backend.MVVM.Base;

namespace StockMeal.Backend.Modelos;

[Table("producto")]
public partial class Producto : ValidatableViewModel
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Column("descripcion", TypeName = "text")]
    public string? Descripcion { get; set; }

    [Column("unidad")]
    [StringLength(20)]
    public string Unidad { get; set; } = null!;

    [Column("precio")]
    [Precision(10)]
    public decimal? Precio { get; set; }

    [Column("tipo", TypeName = "enum('plato','ingrediente')")]
    public string Tipo { get; set; } = null!;

    [Column("stock_disponible")]
    [Required(ErrorMessage = "El stock dispoible es obligatorio.")]
    public int? StockDisponible { get; set; }

    [Column("stock_minimo")]
    public int? StockMinimo { get; set; }

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    [InverseProperty("IdIngredienteNavigation")]
    public virtual ICollection<Recetum> RecetumIdIngredienteNavigations { get; set; } = new List<Recetum>();

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<Recetum> RecetumIdProductoNavigations { get; set; } = new List<Recetum>();
}
