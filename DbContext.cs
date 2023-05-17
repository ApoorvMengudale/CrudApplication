using Crud_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud_Application
{
    public class AppDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Audit> Audit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
              .EnableSensitiveDataLogging()
              .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                  builder => builder.EnableRetryOnFailure());
        }
    }
}
