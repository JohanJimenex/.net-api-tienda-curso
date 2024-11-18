using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;



public class ProductoRepositoryImpl : BaseRepository<Producto>, IProductoRepository {

    private readonly TiendaContext _context;

    public ProductoRepositoryImpl(TiendaContext context) : base(context) {
        _context = context;
    }

    public override async Task<Producto> GetByIdAsync(int id) {
        try {
            return await _context.Productos
                                .Include(p => p.Categoria)
                                .Include(p => p.Marca)
                                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Producto not found");
        }
        catch (Exception ex) {
            throw new Exception("Error al obtener el producto por Id", ex);
        }
    }

    public override async Task<(int totalItems, IEnumerable<Producto> items)> GetAllAsync(int pageIndex, int pageSize) {

        var totalItems = await _context.Productos.CountAsync();
        
        var productos = await _context.Productos
                            .Include(p => p.Categoria)
                            .Include(p => p.Marca)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return (totalItems, productos);
    }

    //se cambio apra devolver con paginador (arriba)
    // public override async Task<IEnumerable<Producto>> GetAllAsync() {
    //     return await _context.Productos
    //     .Include(p => p.Categoria)
    //     .Include(p => p.Marca)
    //     .ToListAsync();
    // }

    public async Task<IEnumerable<Producto>> GetProductosMasCaro(int cantidad) {
        return await _context.Productos.Include(p => p.Categoria).OrderByDescending(p => p.Precio).Take(cantidad).ToListAsync();
    }
}
