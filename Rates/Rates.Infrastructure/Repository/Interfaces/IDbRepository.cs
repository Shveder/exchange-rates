using System.Linq.Expressions;
using Rates.Core.Models.Interface;

namespace Rates.Infrastructure.Repository.Interfaces;

public interface IDbRepository
{
    IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IModels;
    IQueryable<T> Get<T>() where T : class, IModels;
    Task<Guid> Add<T>(T newEntity) where T : class, IModels;
    Task<int> SaveChangesAsync();
}