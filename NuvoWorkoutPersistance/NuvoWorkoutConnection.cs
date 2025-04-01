using Npgsql;
using NuvoDb;
using NuvoWorkoutPersistance.Models;

namespace NuvoWorkoutPersistance;

public class NuvoWorkoutConnection
{
    private readonly NuvoDatabaseMap _databaseMap = new("nuvo_workout");
    private readonly static string s_connectionString = "Host=localhost;Port=5432;Database=nuvo_workout;Username=nuvo_workout;Password=SuperSecret;";
    public NpgsqlDataSource? DataSource { get; set; }
    
    public NuvoWorkoutConnection()
    {
        DataSource = NpgsqlDataSource.Create(s_connectionString);
        NwConfig.DefineModelMap(_databaseMap);
        NwUser.DefineModelMap(_databaseMap);
    }

    public async Task<List<TModel>> Query<TModel>(string sql, IEnumerable<NpgsqlParameter>? parameters = null)
    {
        using var dataSource = NpgsqlDataSource.Create(s_connectionString);
        await using var cmd = dataSource.CreateCommand(sql);
        if (parameters != null)
        {
            foreach (var parameter in parameters)
                cmd.Parameters.Add(parameter);
        }
        await using var reader = await cmd.ExecuteReaderAsync();
        return await _databaseMap.MapModels<TModel>(reader);
    }
}
