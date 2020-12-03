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
            options.UseSqlServer(_connectionString);

            options.AddInterceptors(new FakeTransientErrorsInterceptor());
        }
    }
}
