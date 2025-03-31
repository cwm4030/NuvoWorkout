using Npgsql;
using NuvoWorkoutPersistance.Models;

namespace NuvoWorkoutPersistance;

public class NuvoWorkoutConnection
{
    private readonly ModelMapper _modelMapper = new();
    private readonly static string s_connectionString = "Host=localhost;Port=5432;Database=nuvo_workout;Username=nuvo_workout;Password=SuperSecret;";
    public NpgsqlDataSource? DataSource { get; set; }
    
    public NuvoWorkoutConnection()
    {
        DataSource = NpgsqlDataSource.Create(s_connectionString);
        NwConfig.DefineModelMap(_modelMapper);
        NwUser.DefineModelMap(_modelMapper);
    }

    public async Task<List<TModel>> Query<TModel>(string sql)
    {
        using var dataSource = NpgsqlDataSource.Create(s_connectionString);
        await using var cmd = dataSource.CreateCommand(sql);
        await using var reader = await cmd.ExecuteReaderAsync();
        return await _modelMapper.MapModels<TModel>(reader);
    }
}
