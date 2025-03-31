namespace NuvoWorkoutPersistance.Models;

public class NwConfig
{
    public long Id { get; set; }
    public bool? Inactive { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Version { get; set; }

    public static void DefineModelMap(ModelMapper modelMapper)
    {
        modelMapper.AddPropertyMap<NwConfig, long>(nameof(Id), "id", "bigint", true);
        modelMapper.AddPropertyMap<NwConfig, bool?>(nameof(Inactive), "inactive", "boolean");
        modelMapper.AddPropertyMap<NwConfig, string?>(nameof(Name), "name", "varchar(128)");
        modelMapper.AddPropertyMap<NwConfig, string?>(nameof(Description), "description", "varchar(256)");
        modelMapper.AddPropertyMap<NwConfig, string?>(nameof(Version), "version", "varchar(128)");
    }
}