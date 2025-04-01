using NuvoDb;

namespace NuvoWorkoutPersistance.Models;

public class NwConfig
{
    public long Id { get; set; }
    public bool? Inactive { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Version { get; set; }

    public static void DefineModelMap(NuvoDatabaseMap databaseMap)
    {
        var modelMap = new NuvoModelMap(nameof(NwConfig), "nw_user");
        modelMap.DefinePropertyMap<NwConfig, long>(nameof(Id), "id", "bigint", true);
        modelMap.DefinePropertyMap<NwConfig, bool?>(nameof(Inactive), "inactive", "boolen");
        modelMap.DefinePropertyMap<NwConfig, string?>(nameof(Name), "name", "varchar(128)");
        modelMap.DefinePropertyMap<NwConfig, string?>(nameof(Description), "description", "varchar(256)");
        modelMap.DefinePropertyMap<NwConfig, string?>(nameof(Version), "version", "varchar(128)");
        databaseMap.DefineModelMap(modelMap);
    }
}