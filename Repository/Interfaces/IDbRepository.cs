using System.Linq.Expressions;
using exchange_rates.Models.Interface;

namespace exchange_rates.Repository.Interfaces;

public interface IDbRepository
{
    IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IModels;
    IQueryable<T> Get<T>() where T : class, IModels;
    Task<Guid> Add<T>(T newEntity) where T : class, IModels;
    Task<int> SaveChangesAsync();
}