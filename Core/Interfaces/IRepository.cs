using System.Linq.Expressions;
namespace Core.Interfaces;


public interface IRepository<T> where T : class {

    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<(int totalItems, IEnumerable<T> items)> GetAllAsync(int pageIndex, int pageSize, string search);
    
    //Otra forma de retornar el total de items y los items en vez de una tupla
    Task<MiClase<T>> GetAllAsync2(int pageIndex, int pageSize);

    IEnumerable<T> Find(Expression<Func<T, bool>> match);

    void Add(T entity);
    void AddRange(IEnumerable<T> entitiesList);

    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entitiesList);

    void Update(T entity);

}

//Para retornar el total de items y los items en vez de una tupla
public class MiClase<T> {
    public int totalItems { get; set; }
    public required IEnumerable<T> items { get; set; }
}
