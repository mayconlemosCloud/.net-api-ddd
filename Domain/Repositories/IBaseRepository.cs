using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        // Métodos opcionais com implementações padrão
        Task<T?> GetByIdAsync(Guid id) => Task.FromResult<T?>(null);
        Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(new List<T>());
        Task AddAsync(T entity) => Task.CompletedTask;
        Task UpdateAsync(T entity) => Task.CompletedTask;
        Task DeleteAsync(Guid id) => Task.CompletedTask;

    }
}