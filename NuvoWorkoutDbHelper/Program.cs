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
        if (nwUsers.Any())
        {
        }
    }
}