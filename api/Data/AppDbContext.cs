using FitStack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FitStack.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}