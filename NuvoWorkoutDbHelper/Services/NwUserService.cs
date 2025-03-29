using System.Security.Cryptography;
using NuvoWorkoutDbHelper.Context;
using NuvoWorkoutDbHelper.Helpers;
using NuvoWorkoutDbHelper.Models;
using NuvoWorkoutDbHelper.Repositories;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NuvoWorkoutDbHelper.Services;

public static class NwUserService
{
    public static async Task<NwUser?> CreateUser(NwUser nwUser)
    {
        try
        {
            var matchingUserName = await ReadOnlyRepo<NuvoWorkoutContext, NwUser>.JoinedFindSingle(m => m.Include(u => u.NwUserPrograms), u => u.Username == nwUser.Username && u.Inactive != true);
            if (matchingUserName != null) return null;

            var context = new NuvoWorkoutContext(true);
            nwUser.Id = 0;
            nwUser.PasswordHash = HashPassword(nwUser.PasswordHash);
            var newNwUser = await context.AddAsync(nwUser);
            await context.SaveChangesAsync();
            return newNwUser.Entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ExceptionHelper.GetInnerExceptionMessage(ex));
            return null;
        }
    }

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
                PasswordHash = HashPassword("SuperSecret123!"),
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

    private static string HashPassword(string password, byte[]? saltBytes = null)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        saltBytes ??= RandomNumberGenerator.GetBytes(16);
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, 800000, HashAlgorithmName.SHA3_512, 64);
        return Convert.ToBase64String([.. saltBytes, .. hashBytes]);
    }

    private static bool IsCorrectPassword(string password, string passwordHash)
    {
        var passwordHashBytes = Convert.FromBase64String(passwordHash);
        var saltBytes = passwordHashBytes.Take(16).ToArray();
        return HashPassword(password, saltBytes) == passwordHash;
    }
}
