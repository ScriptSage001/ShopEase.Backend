using Microsoft.EntityFrameworkCore;
using ShopEase.Backend.AuthService.Core.Entities;

namespace ShopEase.Backend.AuthService.Infrastructure
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
    {
        public DbSet<User> User { get; set; }

        public DbSet<UserCredentials> UserCredentials { get; set; }
    }
}
