using NuvoWorkoutDbHelper.Services;

namespace NuvoWorkoutDbHelper;

public static class Program
{
    public static async Task Main()
    {
        await NwUserService.Testing();
    }
}