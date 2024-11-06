using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : class {

    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> Find(Expression<Func<T, bool>> match);

    void Add(T entity);
    void AddRange(IEnumerable<T> entitiesList);

    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entitiesList);

    void Update(T entity);
}
