
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto> {

    public void Configure(EntityTypeBuilder<Producto> builder) {
        //Esta configuración tambien se puede hacer directo en el DbContext

        builder.ToTable("Productos"); //podemos renombrar la tabla
        // builder.HasKey(p => p.Id); //especificamos la clave primaria
        // builder.Property(p => p.Id).IsRequired(); //especificamos que el Id es autoincremental
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Precio).IsRequired().HasColumnType("decimal(18,2)"); //especificar el tipo de dato manualmente para SQL
        //Relaciones con otras tablas, no es necesario especificar la propiedad en las otras tablas
        builder.HasOne(p => p.Marca).WithMany(m => m.Productos).HasForeignKey(p => p.MarcaId);
        builder.HasOne(p => p.Categoria).WithMany(c => c.Productos).HasForeignKey(p => p.CategoriaId);

        //Sirve para insertar datos de prueba en la base de datos
        //Tambien se puede hacer usando un archivo CSV como en TiendaContextSeed.cs
        // builder.HasData(
        //     new Producto { Id = 1, Nombre = "Producto 1", Precio = 100, MarcaId = 1, CategoriaId = 1 },
        //     new Producto { Id = 2, Nombre = "Producto 2", Precio = 200, MarcaId = 2, CategoriaId = 2 }
        // );

    }
}

