using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NuvoWorkoutDbHelper.Context;
using NuvoWorkoutDbHelper.Models;

namespace NuvoWorkoutDbHelper.Repositories;

public static class NwUserRepository
{
    public static async Task<IEnumerable<NwUser>> GetAll()
    {
        using var context = new NuvoWorkoutContext();
        return await context.NwUsers.AsNoTracking().ToListAsync();
    }

    public static async Task<IEnumerable<NwUser>> Find(Expression<Func<NwUser, bool>> predicate)
    {
        using var context = new NuvoWorkoutContext();
        return await context.NwUsers.AsNoTracking().Where(predicate).ToListAsync();
    }
}
