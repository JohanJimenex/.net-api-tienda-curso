using System.Linq.Expressions;
using Core.Interfaces;
using Infrastructure.data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;


public class BaseRepository<T> : IRepository<T> where T : class {

    //Ojo en esta impleemntacion no se usara el metodo de guardar cambios,
    //sino que se usara el metodo de guardar cambios con el patron de diseno UnitOfWork, se peude hacer desde aqui si es una app pequena

    private readonly TiendaContext _context;

    public BaseRepository(TiendaContext context) {
        _context = context;
    }

    public virtual async Task<T> GetByIdAsync(int id) {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() {
        return await _context.Set<T>().ToListAsync();
    }

    //Otra forma de ahcer esto, es que devuelva un objeto con la cantidad de elementos y los elementos
    //ejemplo:  {totalItems: 100, items: [elemento1, elemento2, elemento3, ...]}
    public virtual async Task<(int totalItems, IEnumerable<T> items)> GetAllAsync(int pageIndex, int pageSize, string search) {

        var totalItems = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
                            .Skip(pageIndex * pageSize) //esto devuelve los elementos que se van a saltar, ejemplo si pageIndex es 1 y pageSize es 10, se saltara 10 elementos
                            .Take(pageSize) //esto devuelve los elementos que se van a tomar, ejemplo si pageIndex es 1 y pageSize es 10, se tomara 10 elementos
                            .ToListAsync();// esto convierte el resultado en una lista porque el metodo devuelve un IQueryable

        return (totalItems, items);
    }

    // Ejemplo con una clase que tiene dos propiedades, una para la cantidad de elementos y otra para los elementos
    public async Task<MiClase<T>> GetAllAsync2(int pageIndex, int pageSize) {
        var totalItems = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return new MiClase<T> {
            totalItems = totalItems,
            items = items
        };
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression) {
        return _context.Set<T>().Where(expression);
    }

    public void Add(T entity) {
        _context.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entitiesList) {
        _context.Set<T>().AddRange(entitiesList);
    }

    public void Remove(T entity) {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entitiesList) {
        _context.Set<T>().RemoveRange(entitiesList);
    }

    public void Update(T entity) {
        _context.Set<T>().Update(entity);
    }


}
