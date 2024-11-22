using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;

namespace Infrastructure.Repositories;

public class UsuarioRepositoryImpl : BaseRepositoryImpl<Usuario>, IUsuarioRepository { //hereda de BaseRepositoryImpl y e implementa la interfaz IUsuarioRepository
 
    public UsuarioRepositoryImpl(TiendaContext context) : base(context) { }

}
