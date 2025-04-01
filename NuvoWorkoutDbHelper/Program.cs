using Npgsql;
using NuvoWorkoutPersistance;
using NuvoWorkoutPersistance.Models;

namespace NuvoWorkoutDbHelper;

public static class Program
{
    public static async Task Main()
    {
        var connection = new NuvoWorkoutConnection();
        var nwConfig = await connection.Query<NwConfig>("select * from nw_config where coalesce(inactive, false) = false limit 1");
        var nwUsers = await connection.Query<NwUser>("select * from nw_user");
        if (nwConfig.Count == 0 || nwUsers.Count == 0)
        {
        }

        var birthDate = new DateTime(1999, 2, 10, 0, 0, 0, DateTimeKind.Utc);
        var parameter = new NpgsqlParameter { Value = birthDate };
        nwUsers = await connection.Query<NwUser>("select * from nw_user where cast(birth_date as date) = cast($1 as date)", [parameter]);
        if (nwUsers.Count == 0)
        {
        }
    }
}