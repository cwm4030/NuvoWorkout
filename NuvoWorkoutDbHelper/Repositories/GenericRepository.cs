using Microsoft.EntityFrameworkCore;

namespace NuvoWorkoutDbHelper.Repositories;

public static class GenericRepository<TContext, TModel>
    where TContext : DbContext
    where TModel : class
{
    public static async Task<IEnumerable<TModel>> Query(Func<IQueryable<TModel>, IQueryable<TModel>>? queryFuc = null)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = queryFuc != null ? queryFuc(context.Set<TModel>().AsNoTracking()) : context.Set<TModel>().AsNoTracking();
        return await models.ToListAsync();
    }

    public static async Task<IEnumerable<TModel>> QuerySql(string sql)
    {
        using var context = Activator.CreateInstance<TContext>();
        return await context.Set<TModel>().FromSqlRaw(sql).AsNoTracking().ToListAsync();
    }
}
