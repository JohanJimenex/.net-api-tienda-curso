using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Core.Interfaces;

public interface IUnitOfWork {

    IProductoRepository Productosrepository { get; }
    ICategoriaRepository CategoriasRepository { get; }
    IMarcaRepository MarcasRepository { get; }

    Task<int> Save();

    void Dispose();

}
