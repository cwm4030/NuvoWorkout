using Microsoft.EntityFrameworkCore;
using NuvoWorkoutDbHelper.Models;

namespace NuvoWorkoutDbHelper.Context;

public class NuvoWorkoutContext : DbContext
{
    public NuvoWorkoutContext() : base(new DbContextOptions<NuvoWorkoutContext>())
    {
        _hasWriteAccess = false;
    }

    public NuvoWorkoutContext(bool hasWriteAccess) : base(new DbContextOptions<NuvoWorkoutContext>())
    {
        _hasWriteAccess = hasWriteAccess;
    }

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
        NwUserProgram.ConfigureModel(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private readonly bool _hasWriteAccess;
    public DbSet<NwUser> NwUsers { get; set; } = null!;
    public DbSet<NwUserProgram> NwUserPrograms { get; set; } = null!;
}
