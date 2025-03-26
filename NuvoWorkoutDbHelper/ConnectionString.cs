namespace NuvoWorkoutDbHelper;

public static class ConnectionString
{
    public static readonly string NuvoWorkoutConnectionString = "Host=localhost; Port=5432; Database=nuvo_workout; Username=nuvo_workout; Password=SuperSecret;";
    public static readonly string NuvoWorkoutReadonlyConnectionString = "Host=localhost; Port=5432; Database=nuvo_workout; Username=nuvo_workout_readonly; Password=SuperSecret;";
}
