using Microsoft.EntityFrameworkCore;

namespace dontnet7_vue_auth.api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) :base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=.\\localhost; Database=superPeopleDb;Trusted_Connection=true;TrustServerCertificate=true");
    }
    
    public DbSet<User> Users { get; set; }
}