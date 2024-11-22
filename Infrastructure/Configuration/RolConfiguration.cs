
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RolConfiguration : IEntityTypeConfiguration<Rol> {

    public void Configure(EntityTypeBuilder<Rol> builder) {

        // builder.HasKey(r => r.Id); //esto no es necesario porque ya se hizo en BaseEntityConfiguration
        builder.Property(r => r.Nombre).IsRequired().HasMaxLength(50);

        // Esto no es necesario porque ya se hizo en UsuarioConfiguration
        // builder.HasMany(r => r.Usuarios)
        // .WithMany(u => u.Roles)
        // .UsingEntity(j => j.ToTable("UsuariosRoles"));
    }

}