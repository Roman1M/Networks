using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class SampleContextFactory : IDesignTimeDbContextFactory<SocialNetworkDbContext>
    {
        public SocialNetworkDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SocialNetworkDbContext>();

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            string? connectionString = config.GetConnectionString("LocalDb");
            optionsBuilder.UseSqlServer(connectionString);
            return new SocialNetworkDbContext(optionsBuilder.Options);
        }
    }
}
