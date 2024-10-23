using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto> {

    public void Configure(EntityTypeBuilder<Producto> builder) {
        //Esta configuración tambien se puede hacer directo en el DbContext

        builder.ToTable("Productos"); //podemos renombrar la tabla
        // builder.HasKey(p => p.Id); //especificamos la clave primaria
        builder.Property(p => p.Id).IsRequired(); //especificamos que el Id es autoincremental
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Precio).IsRequired().HasColumnType("decimal(18,2)");

        //Relaciones con otras tablas, no es necesario especificar la propiedad en las otras tablas
        builder.HasOne(p => p.Marca).WithMany(m => m.Productos).HasForeignKey(p => p.MarcaId);
        builder.HasOne(p => p.Categoria).WithMany(c => c.Productos).HasForeignKey(p => p.CategoriaId);

    }
}

