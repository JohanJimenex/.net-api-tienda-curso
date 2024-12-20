using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.data;

public class TiendaContext : DbContext { //Hereda de la clase DbContext para poder usar las funcionalidades de Entity Framework

    //Constructor que recibe las opciones de configuración de la base de datos
    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

    public DbSet<Producto> Productos { get; set; } //Propiedad que representa la tabla de productos en la base de datos
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }

    //con este metodo podemos sobreeescribir el nombre por si no queremos que se llame igual que la clase
    // protected override void OnModelCreating(ModelBuilder modelBuilder) { 
    //     modelBuilder.Entity<Producto>().ToTable("MisProductos"); //ahora en vez de llamarse "Productos" se llamará "MisProductos" 

    //Tambien se puede agregar las reglas de las tablas con Fluent API  
    //Establece que el campo "Nombre" de la tabla "Productos" tenga un máximo de 100 caracteres

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        //Esta linea de código busca todas las configuraciones que implementen la interfaz IEntityTypeConfiguration ejemplo ProductoConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Esta configuration se hizo en un archivo separado llamado "ProductoConfiguration.cs"
        //   // En este caso se establece varias reglas para la tabla "Productos"
        //         modelBuilder.Entity<Producto>(p =>
        //         {
        //             p.HasKey(e => e.Id);

        //             p.Property(e => e.Nombre)
        //                   .IsRequired()
        //                   .HasMaxLength(100);

        //             p.Property(e => e.Precio)
        //                   .HasColumnType("decimal(18,2)")
        //                   .HasDefaultValue(0.0m);

        //             p.Property(e => e.FechaCreacion)
        //                   .HasColumnType("date");

        //             p.HasOne(e => e.Marca)
        //                   .WithMany(m => m.Productos)
        //                   .HasForeignKey(e => e.MarcaId);

        //             p.HasOne(e => e.Categoria)
        //                   .WithMany(c => c.Productos)
        //                   .HasForeignKey(e => e.CategoriaId);
        //         });
        // }

        // ========================================================================================================

        //La otra forma de hacerlo es con Data Annotations
        // Ejemplo desde la misma clase Producto, (abrir la clase Producto.cs para verlo completo)

        // public class Producto
        // {
        //     [Key]
        //     public int Id { get; set; }

        //     [Required]
        //     [StringLength(100)]
        //     public string Nombre { get; set; }

        //     [Range(0.01, 10000.00)]
        //     public decimal Precio { get; set; }

        //     [DataType(DataType.Date)]
        //     public DateTime FechaCreacion { get; set; }

        //     [ForeignKey("Marca")]
        //     public int MarcaId { get; set; }
        //     public Marca Marca { get; set; }

        //     [ForeignKey("Categoria")]
        //     public int CategoriaId { get; set; }
        //     public Categoria Categoria { get; set; }
    }

}
