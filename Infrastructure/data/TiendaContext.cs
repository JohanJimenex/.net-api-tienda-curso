using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.data;

public class TiendaContext : DbContext { //Hereda de la clase DbContext para poder usar las funcionalidades de Entity Framework

    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { //Constructor que recibe las opciones de configuración de la base de datos
    }

    public DbSet<Producto> Productos { get; set; } //Propiedad que representa la tabla de productos en la base de datos

    // protected override void OnModelCreating(ModelBuilder modelBuilder) { //Método que se ejecuta al crear el modelo de la base de datos
    //     modelBuilder.Entity<Producto>().ToTable("Productos"); //Se indica que la tabla de productos se llamará "Productos"
    // }

}
