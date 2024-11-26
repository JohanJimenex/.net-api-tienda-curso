using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;



public class ProductoRepositoryImpl : BaseRepositoryImpl<Producto>, IProductoRepository {

    public ProductoRepositoryImpl(TiendaContext context) : base(context) { }

    public override async Task<Producto> GetByIdAsync(int id) {
        return await _context.Productos
                            .Include(p => p.Categoria)
                            .Include(p => p.Marca)
                            .FirstOrDefaultAsync(p => p.Id == id) ?? null!;
    }

    public override async Task<(int totalItems, IEnumerable<Producto> items)> GetAllAsync(int pageIndex, int pageSize, string search) {

        var totalItems = await _context.Productos.CountAsync();

        var productos = await _context.Productos
                            .Where(p => p.Nombre!.ToLower().Contains(search.ToLower()))
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
