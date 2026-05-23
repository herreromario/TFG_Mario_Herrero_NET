using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StockMeal.Backend.Modelos;

namespace StockMeal.Backend.DBContext;

public partial class StockMealContext : DbContext
{
    public StockMealContext()
    {
    }

    public StockMealContext(DbContextOptions<StockMealContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CierreCaja> CierreCajas { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<MovimientoCaja> MovimientoCajas { get; set; }

    public virtual DbSet<PedidoVentum> PedidoVenta { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }
    public virtual DbSet<Produccion> Produccions { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseMySQL("server=127.0.0.1;port=3306;database=gestion_stock;user=root;password=mysql;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CierreCaja>(entity =>
        {
            entity.HasKey(e => e.IdCierre).HasName("PRIMARY");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PRIMARY");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PRIMARY");

            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras).HasConstraintName("compra_ibfk_1");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras).HasConstraintName("detalle_compra_ibfk_1");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras).HasConstraintName("detalle_compra_ibfk_2");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos).HasConstraintName("detalle_pedido_ibfk_2");

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.DetallePedidos).HasConstraintName("detalle_pedido_ibfk_1");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PRIMARY");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Empleados)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("empleado_ibfk_1");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PRIMARY");

            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.Facturas).HasConstraintName("fk_factura_transaccion");
        });

        modelBuilder.Entity<MovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PRIMARY");

            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdCierreNavigation).WithMany(p => p.MovimientoCajas)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("movimiento_caja_ibfk_3");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.MovimientoCajas)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("movimiento_caja_ibfk_2");

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.MovimientoCajas).HasConstraintName("movimieto_pedido_venta");
        });

        modelBuilder.Entity<PedidoVentum>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion).HasName("PRIMARY");

            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.PedidoVenta)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedido_venta_ibfk_1");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.PedidoVenta)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedido_venta_ibfk_2");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PRIMARY");
        });

        modelBuilder.Entity<Produccion>(entity =>
        {
            entity.HasKey(e => e.IdProduccion).HasName("PRIMARY");
            entity.HasOne(d => d.Producto).WithMany().HasForeignKey(d => d.IdProducto);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.Property(e => e.Unidad).HasDefaultValueSql("'unidad'");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PRIMARY");
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => new { e.IdProducto, e.IdIngrediente }).HasName("PRIMARY");

            entity.HasOne(d => d.IdIngredienteNavigation).WithMany(p => p.RecetumIdIngredienteNavigations).HasConstraintName("receta_ibfk_2");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.RecetumIdProductoNavigations).HasConstraintName("receta_ibfk_1");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.HasMany(d => d.IdPermisos).WithMany(p => p.IdRols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("IdPermiso")
                        .HasConstraintName("rol_permiso_ibfk_2"),
                    l => l.HasOne<Rol>().WithMany()
                        .HasForeignKey("IdRol")
                        .HasConstraintName("rol_permiso_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdRol", "IdPermiso").HasName("PRIMARY");
                        j.ToTable("rol_permiso");
                        j.HasIndex(new[] { "IdPermiso" }, "id_permiso");
                        j.IndexerProperty<int>("IdRol").HasColumnName("id_rol");
                        j.IndexerProperty<int>("IdPermiso").HasColumnName("id_permiso");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
