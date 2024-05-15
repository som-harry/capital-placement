
namespace CapitalReplacement.Persistence.Configuration;

public class DataContext : DbContext
{
    private static bool _ensureCreated = false;
    public DataContext()
    {
    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Programme> Programmes { get; set; }
    public DbSet<CandidateApplication> CandidateApplications { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer(nameof(Programmes));

        modelBuilder.Entity<Programme>()
         .ToContainer(nameof(Programmes))
         .HasPartitionKey(c => c.Id)
         .HasNoDiscriminator();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CandidateApplication>()
       .ToContainer(nameof(CandidateApplications))
       .HasPartitionKey(c => c.Id)
       .HasNoDiscriminator();
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}

