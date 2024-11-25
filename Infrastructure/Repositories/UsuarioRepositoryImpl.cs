using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepositoryImpl : BaseRepositoryImpl<Usuario>, IUsuarioRepository { //hereda de BaseRepositoryImpl y e implementa la interfaz IUsuarioRepository

    public UsuarioRepositoryImpl(TiendaContext context) : base(context) { }

    public async Task<Usuario> GetUsuarioByCorreo(string correo) {
        var usuario = await _context.Usuarios.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Correo.ToLower().Equals(correo.ToLower()));
        return usuario ?? throw new Exception("Usuario not found");
    }

}
