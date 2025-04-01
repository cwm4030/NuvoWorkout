using NuvoDb;

namespace NuvoWorkoutPersistance.Models;

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
    public DateTime? BirthDate { get; set; }
    public string? Sex { get; set; }
    public string? WeightMetric { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BodyFatPercentage { get; set; }

    public static void DefineModelMap(NuvoDatabaseMap databaseMap)
    {
        var modelMap = new NuvoModelMap(nameof(NwUser), "nw_user");
        modelMap.DefinePropertyMap<NwUser, long>(nameof(Id), "id", "bigint", true);
        modelMap.DefinePropertyMap<NwUser, bool?>(nameof(Inactive), "inactive", "boolean");
        modelMap.DefinePropertyMap<NwUser, DateTime?>(nameof(DateCreated), "date_created", "timestamp with time zone");
        modelMap.DefinePropertyMap<NwUser, DateTime?>(nameof(DateUpdated), "date_updated", "timestamp with time zone");
        modelMap.DefinePropertyMap<NwUser, string>(nameof(Username), "username", "varchar(128)", true);
        modelMap.DefinePropertyMap<NwUser, string>(nameof(PasswordHash), "password_hash", "varchar(512)", true);
        modelMap.DefinePropertyMap<NwUser, string?>(nameof(FirstName), "first_name", "varchar(128)");
        modelMap.DefinePropertyMap<NwUser, string?>(nameof(MiddleName), "middle_name", "varchar(128)");
        modelMap.DefinePropertyMap<NwUser, string?>(nameof(LastName), "last_name", "varchar(128)");
        modelMap.DefinePropertyMap<NwUser, DateTime?>(nameof(BirthDate), "birth_date", "timestamp with time zone");
        modelMap.DefinePropertyMap<NwUser, string?>(nameof(Sex), "sex", "varchar(32)");
        modelMap.DefinePropertyMap<NwUser, string?>(nameof(WeightMetric), "weight_metric", "varchar(32)");
        modelMap.DefinePropertyMap<NwUser, decimal?>(nameof(Weight), "weight", "decimal");
        modelMap.DefinePropertyMap<NwUser, decimal?>(nameof(BodyFatPercentage), "body_fat_percentage", "decimal");
        databaseMap.DefineModelMap(modelMap);
    }
}
