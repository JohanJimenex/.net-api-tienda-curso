using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Core.Interfaces;

public interface IUnitOfWork {

    IProductoRepository ProductosRepository { get; }
    ICategoriaRepository CategoriasRepository { get; }
    IMarcaRepository MarcasRepository { get; }
    IUsuarioRepository UsuariosRepository { get; }
    IRolRepository RolesRepository { get; }

    Task<int> Save();

    void Dispose();

}
