using Microsoft.EntityFrameworkCore;
using NuvoWorkoutDbHelper.Models;

namespace NuvoWorkoutDbHelper.Context;

public class NuvoWorkoutContext(bool hasWriteAccess = false) : DbContext(new DbContextOptions<NuvoWorkoutContext>())
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _hasWriteAccess ? ConnectionString.NuvoWorkoutConnectionString : ConnectionString.NuvoWorkoutReadonlyConnectionString;
        optionsBuilder.UseNpgsql(connectionString, opts => opts.CommandTimeout(600));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        NwUser.ConfigureModel(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private readonly bool _hasWriteAccess = hasWriteAccess;
    public DbSet<NwUser> NwUsers { get; set; } = null!;
}
