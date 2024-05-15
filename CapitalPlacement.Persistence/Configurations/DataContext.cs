using Microsoft.EntityFrameworkCore;

namespace CapitalReplacement.Persistence.Configuration;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        var result = base.SaveChanges();
        return result;
    }
}

