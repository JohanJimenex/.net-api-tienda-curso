using Core.Entities;
namespace Core.Interfaces;

public interface IProductoRepository : IRepository<Producto> {

    //GetProductosmas caro
    Task<IEnumerable<Producto>> GetProductosMasCaro(int cantidad);


}
