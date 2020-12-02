using System;
using Microsoft.EntityFrameworkCore;

namespace CarvedRockSoftware.Seeder.AzureSql
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<ProductEntity> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_connectionString, options =>
            {
                options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            });

            options.AddInterceptors(new FakeTransientErrorsInterceptor());
        }
    }
}
