using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configuration;


public class MarcaConfiguration : IEntityTypeConfiguration<Marca> {

    public void Configure(EntityTypeBuilder<Marca> builder) {
        builder.ToTable("Marcas");
        // builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired();
        builder.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
    }

}
