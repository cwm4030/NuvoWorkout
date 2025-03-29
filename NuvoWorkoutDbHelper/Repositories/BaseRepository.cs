using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace NuvoWorkoutDbHelper.Repositories;

public class BaseRepository<TContext, TModel>
    where TContext : DbContext
    where TModel : class
{
    public static async Task<IEnumerable<TModel>> GetAll(Func<IQueryable<TModel>, IQueryable<TModel>>? joinsFunc = null)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = joinsFunc != null ? joinsFunc(context.Set<TModel>().AsNoTracking()) : context.Set<TModel>().AsNoTracking();
        return await models.ToListAsync();
    }

    public static async Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> predicate, Func<IQueryable<TModel>, IQueryable<TModel>>? joinsFunc = null)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = joinsFunc != null ? joinsFunc(context.Set<TModel>().AsNoTracking()) : context.Set<TModel>().AsNoTracking();
        return await models.Where(predicate).ToListAsync();
    }
}
