using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.data;

public class TiendaContext : DbContext { //Hereda de la clase DbContext para poder usar las funcionalidades de Entity Framework

    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { //Constructor que recibe las opciones de configuración de la base de datos
    }

    public DbSet<Producto> Productos { get; set; } //Propiedad que representa la tabla de productos en la base de datos

    //con este metodo podemos sobreeescribir el nombre por si no queremos que se llame igual que la clase
    // protected override void OnModelCreating(ModelBuilder modelBuilder) { 
    //     modelBuilder.Entity<Producto>().ToTable("MisProductos"); //ahora en vez de llamarse "Productos" se llamará "MisProductos" 
    // }

}
