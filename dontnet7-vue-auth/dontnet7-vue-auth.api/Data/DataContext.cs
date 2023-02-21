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
        //ToDo- find why last statement not working
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=.\\localhost; Database=superPeopleDb;Trusted_Connection=true;TrustServerCertificate=true");
        // optionsBuilder.UseSqlServer(_configuration.GetSection("ConnectionString: LocalServer").Value);
    }
    
    public DbSet<User> Users { get; set; }
}