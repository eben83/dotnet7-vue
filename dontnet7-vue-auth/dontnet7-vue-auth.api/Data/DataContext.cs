using Microsoft.EntityFrameworkCore;

namespace dontnet7_vue_auth.api.Data;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) :base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // var keySting = _configuration.GetSection("AppSettings:ConnectionString");
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=.\\localhost; Database=superPeopleDb;Trusted_Connection=true;TrustServerCertificate=true");
        // optionsBuilder.UseSqlServer("Server=.\\localhost; Database=superPeopleDb;Trusted_Connection=true;TrustServerCertificate=true");
        // optionsBuilder.UseSqlServer(keySting.ToString());
    }
    
    public DbSet<User> Users { get; set; }
}