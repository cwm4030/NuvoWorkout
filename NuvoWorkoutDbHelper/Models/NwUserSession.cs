using Microsoft.EntityFrameworkCore;
using NuvoWorkoutDbHelper.Context;

namespace NuvoWorkoutDbHelper.Models;

public class NwUserSession
{
    public long Id { get; set; }
    public bool? Inactive { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? NwUserId { get; set; }

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NwUserSession>(entity =>
        {
            entity.ToTable("nw_user_program");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").IsRequired();
            entity.Property(e => e.Inactive).HasColumnName("inactive").HasColumnType("boolean").IsRequired(false);
            entity.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(128)").IsRequired(false);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("varchar(256)").IsRequired(false);
            entity.Property(e => e.StartDate).HasColumnName("start_date").HasColumnType("timestamp with time zone").HasConversion(UniversalDateTimeConverter.NullableDateTimeConverter).IsRequired(false);
            entity.Property(e => e.EndDate).HasColumnName("end_date").HasColumnType("timestamp with time zone").HasConversion(UniversalDateTimeConverter.NullableDateTimeConverter).IsRequired(false);
            entity.Property(e => e.Description).HasColumnName("nw_user_id").HasColumnType("bigint").IsRequired(false);
        });
    }
}