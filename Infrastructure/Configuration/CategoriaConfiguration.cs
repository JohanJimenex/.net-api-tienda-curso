
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria> {

    public void Configure(EntityTypeBuilder<Categoria> builder) {
        builder.ToTable("Categorias");
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
    }

}
