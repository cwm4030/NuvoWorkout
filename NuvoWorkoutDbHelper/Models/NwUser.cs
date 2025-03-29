using Microsoft.EntityFrameworkCore;
using NuvoWorkoutDbHelper.Context;

namespace NuvoWorkoutDbHelper.Models;

public class NwUser
{
    public long Id { get; set; }
    public bool? Inactive { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Sex { get; set; }
    public int? Age { get; set; }
    public string? WeightMetric { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BodyFatPercentage { get; set; }
    public IEnumerable<NwUserProgram> NwUserPrograms { get; set; } = [];

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NwUser>(entity =>
        {
            entity.ToTable("nw_user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").IsRequired();
            entity.Property(e => e.Inactive).HasColumnName("inactive").HasColumnType("boolean").IsRequired(false);
            entity.Property(e => e.DateCreated).HasColumnName("date_created").HasColumnType("timestamp with time zone").HasConversion(UniversalDateTimeConverter.NullableDateTimeConverter).IsRequired(false);
            entity.Property(e => e.DateUpdated).HasColumnName("date_updated").HasColumnType("timestamp with time zone").HasConversion(UniversalDateTimeConverter.NullableDateTimeConverter).IsRequired(false);
            entity.Property(e => e.Username).HasColumnName("username").HasColumnType("varchar(128)").IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasColumnType("varchar(512)").IsRequired();
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasColumnType("varchar(128)").IsRequired(false);
            entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasColumnType("varchar(128)").IsRequired(false);
            entity.Property(e => e.LastName).HasColumnName("last_name").HasColumnType("varchar(128)").IsRequired(false);
            entity.Property(e => e.Sex).HasColumnName("sex").HasColumnType("varchar(32)").IsRequired(false);
            entity.Property(e => e.Age).HasColumnName("age").HasColumnType("int").IsRequired(false);
            entity.Property(e => e.WeightMetric).HasColumnName("weight_metric").HasColumnType("varchar(32)").IsRequired(false);
            entity.Property(e => e.Weight).HasColumnName("weight").HasColumnType("decimal").IsRequired(false);
            entity.Property(e => e.BodyFatPercentage).HasColumnName("body_fat_percentage").HasColumnType("decimal").IsRequired(false);
            entity.HasMany(e => e.NwUserPrograms).WithOne().HasForeignKey(e => e.NwUserId).HasPrincipalKey(e => e.Id);
        });
    }
}
