using NuvoWorkoutDbHelper.Context;
using NuvoWorkoutDbHelper.Models;

namespace NuvoWorkoutDbHelper.Services;

public static class NwUserService
{
    public static async Task<bool> Testing()
    {
        try
        {
            var date = DateTime.Now;
            var user = new NwUser()
            {
                Id = 0,
                Inactive = false,
                DateCreated = date,
                DateUpdated = date,
                Username = "johndoe",
                PasswordHash = string.Empty,
                FirstName = "John",
                MiddleName = "M",
                LastName = "Doe",
                Sex = "Male",
                Age = 26,
                WeightMetric = "lbs",
                Weight = 210,
                BodyFatPercentage = 27
            };
            var context = new NuvoWorkoutContext(true);
            _ = await context.NwUsers.AddAsync(user);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, ex.InnerException?.Message);
            return false;
        }
    }
}
