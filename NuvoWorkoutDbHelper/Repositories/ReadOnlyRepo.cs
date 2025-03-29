using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace NuvoWorkoutDbHelper.Repositories;

public static class ReadOnlyRepo<TContext, TModel>
    where TContext : DbContext
    where TModel : class
{
    public static async Task<List<TModel>> Query(Func<IQueryable<TModel>, IQueryable<TModel>>? queryFuc = null)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = queryFuc != null ? queryFuc(context.Set<TModel>().AsNoTracking()) : context.Set<TModel>().AsNoTracking();
        return await models.ToListAsync();
    }

    public static async Task<TModel?> QuerySingle(Func<IQueryable<TModel>, IQueryable<TModel>>? queryFuc = null)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = queryFuc != null ? queryFuc(context.Set<TModel>().AsNoTracking()) : context.Set<TModel>().AsNoTracking();
        return await models.FirstOrDefaultAsync();
    }

    public static async Task<List<TModel>> QuerySql(string sql)
    {
        using var context = Activator.CreateInstance<TContext>();
        return await context.Set<TModel>().FromSqlRaw(sql).AsNoTracking().ToListAsync();
    }

    public static async Task<TModel?> QuerySqlSingle(string sql)
    {
        using var context = Activator.CreateInstance<TContext>();
        return await context.Set<TModel>().FromSqlRaw(sql).AsNoTracking().FirstOrDefaultAsync();
    }

    public static async Task<List<TModel>> Find(Expression<Func<TModel, bool>> predicate)
    {
        using var context = Activator.CreateInstance<TContext>();
        return await context.Set<TModel>().AsNoTracking().Where(predicate).ToListAsync();
    }

    public static async Task<TModel?> FindSingle(Expression<Func<TModel, bool>> predicate)
    {
        using var context = Activator.CreateInstance<TContext>();
        return await context.Set<TModel>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public static async Task<List<TModel>> JoinedFind(Func<IQueryable<TModel>, IQueryable<TModel>> joinFunc, Expression<Func<TModel, bool>> predicate)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = joinFunc(context.Set<TModel>().AsNoTracking());
        return await models.Where(predicate).ToListAsync();
    }

    public static async Task<TModel?> JoinedFindSingle(Func<IQueryable<TModel>, IQueryable<TModel>> joinFunc, Expression<Func<TModel, bool>> predicate)
    {
        using var context = Activator.CreateInstance<TContext>();
        var models = joinFunc(context.Set<TModel>().AsNoTracking());
        return await models.FirstOrDefaultAsync(predicate);
    }
}
