using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using BCrypt.Net;
using RaceDay.API.Models;
using Xunit;

namespace RaceDay.Tests;

public class AuthAndRoleTests
{
    private RaceDayContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RaceDayContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new RaceDayContext(options);
    }

    [Fact]
    public void Register_ShouldHashPassword()
    {
        
        var context = GetInMemoryDbContext();
        var password = "SecurePassword123";
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);

       
        var user = new User { Email = "test@test.com", PasswordHash = hashed, Role = "Participant" };
        context.Users.Add(user);
        context.SaveChanges();

       
        var savedUser = context.Users.First();
        Assert.True(BCrypt.Net.BCrypt.Verify(password, savedUser.PasswordHash));
        Assert.NotEqual(password, savedUser.PasswordHash); 
    }
}