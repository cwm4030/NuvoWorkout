using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NuvoWorkoutDbHelper.Context;

public static class UniversalDateTimeConverter
{
    public static readonly ValueConverter<DateTime, DateTime> DateTimeConverter = new(
        v => v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
    );

    public static readonly ValueConverter<DateTime?, DateTime?> NullableDateTimeConverter = new(
        v => v.HasValue ? v.Value.ToUniversalTime() : v,
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
    );
}
