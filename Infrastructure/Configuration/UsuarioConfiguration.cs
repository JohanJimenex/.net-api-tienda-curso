
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario> {

    public void Configure(EntityTypeBuilder<Usuario> builder) {
        // builder.HasKey(u => u.Id); //esto no es necesario porque ya se hizo en BaseEntityConfiguration
        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(50);
        builder.Property(u => u.ApellidoPaterno).IsRequired().HasMaxLength(50);
        builder.Property(u => u.ApellidoMaterno).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Correo).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Contrasena).IsRequired().HasMaxLength(250);
        // Se configura la relación muchos a muchos con la tabla UsuariosRoles

        // Se configura la relación muchos a muchos con la tabla UsuariosRoles, en vez de crear una entidad UsuariosRoles se usa el método UsingEntity
        builder.HasMany(u => u.Roles)
        .WithMany(r => r.Usuarios)      
        .UsingEntity(j => j.ToTable("UsuariosRoles"));

    }

}