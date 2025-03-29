namespace NuvoWorkoutDbHelper.Helpers;

public static class ExceptionHelper
{
    public static string GetInnerExceptionMessage(Exception ex)
    {
        while (ex.InnerException != null)
            ex = ex.InnerException;
        return ex.Message;
    }
}