using Microsoft.EntityFrameworkCore;
using NuvoWorkoutDbHelper.Context;
using NuvoWorkoutDbHelper.Helpers;
using NuvoWorkoutDbHelper.Models;
using NuvoWorkoutDbHelper.Repositories;

namespace NuvoWorkoutDbHelper.Services;

public static class NwUserService
{
    public static async Task<bool> Testing()
    {
        try
        {
            var nwUsers = await GenericRepository<NuvoWorkoutContext, NwUser>.Query(q => q.Include(u => u.NwUserPrograms).Where(u => u.Username == "johndoe"));
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
            Console.WriteLine(ExceptionHelper.GetInnerExceptionMessage(ex));
            return false;
        }
    }
}
