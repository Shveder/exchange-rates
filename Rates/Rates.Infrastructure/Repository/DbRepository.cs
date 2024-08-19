using System.Linq.Expressions;
using Rates.Core.Models.Interface;
using Rates.Infrastructure.Data;

namespace Rates.Infrastructure.Repository;

public class DbRepository : IDbRepository
{
    private readonly DataContext _context;

    public DbRepository(DataContext context)
    {
        _context = context;
    }

    public IQueryable<T> Get<T>() where T : class, IModels
    {
        return _context.Set<T>().AsQueryable();
    }

    public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IModels
    {
        return _context.Set<T>().Where(selector).AsQueryable();
    }

    public async Task<Guid> Add<T>(T newEntity) where T : class, IModels
    {
        var entity = await _context.Set<T>().AddAsync(newEntity);
        Console.WriteLine(entity.Entity.Id);
        return entity.Entity.Id;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
}