using ElectivaI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectivaI.Data.Context;

public partial class ElectivaIContext : DbContext
{
    public ElectivaIContext()
    {
    }

    public ElectivaIContext(DbContextOptions<ElectivaIContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Orden> Ordens { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TipoEmpleado> TipoEmpleados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer("Server=3.135.183.231,1433;Database=ElectivaI;User Id=sa;Password=Electiva1; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A47126B57");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__88B513948CFB0620");

            entity.ToTable("Empleado");

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.IdTipoEmpleado).HasColumnName("id_tipo_empleado");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empleado_Persona");

            entity.HasOne(d => d.IdTipoEmpleadoNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdTipoEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empleado_TipoEmpleado");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("PK__Inventar__013AEB519D2142AF");

            entity.ToTable("Inventario");

            entity.Property(e => e.IdInventario).HasColumnName("id_inventario");
            entity.Property(e => e.CantidadInventario).HasColumnName("cantidad_inventario");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Empleado");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Producto");
        });

        modelBuilder.Entity<Orden>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PK__Orden__DD5B8F33AE26984C");

            entity.ToTable("Orden");

            entity.Property(e => e.IdOrden).HasColumnName("id_orden");
            entity.Property(e => e.CantidadOrden).HasColumnName("cantidad_orden");
            entity.Property(e => e.FechaOrden)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_orden");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.TotalOrden)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_orden");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orden_Persona");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orden_Producto");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__Persona__228148B0F9064BFC");

            entity.ToTable("Persona");

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.ApellidoPersona)
                .HasMaxLength(100)
                .HasColumnName("apellido_persona");
            entity.Property(e => e.ContrasenaPersona)
                .HasMaxLength(100)
                .HasColumnName("contrasena_persona");
            entity.Property(e => e.CorreoPersona)
                .HasMaxLength(150)
                .HasColumnName("correo_persona");
            entity.Property(e => e.DireccionPersona)
                .HasMaxLength(200)
                .HasColumnName("direccion_persona");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombrePersona)
                .HasMaxLength(100)
                .HasColumnName("nombre_persona");
            entity.Property(e => e.TelefonoPersona)
                .HasMaxLength(20)
                .HasColumnName("telefono_persona");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_Rol");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__FF341C0D3427F0BC");

            entity.ToTable("Producto");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.DescripcionProducto)
                .HasMaxLength(255)
                .HasColumnName("descripcion_producto");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioProducto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_producto");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Empleado");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__6ABCB5E0BD271D5C");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<TipoEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdTipoEmpleado).HasName("PK__TipoEmpl__F614CD3351CF5216");

            entity.ToTable("TipoEmpleado");

            entity.Property(e => e.IdTipoEmpleado).HasColumnName("id_tipo_empleado");
            entity.Property(e => e.TipoEmpleado1)
                .HasMaxLength(50)
                .HasColumnName("tipo_empleado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}