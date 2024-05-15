




namespace CapitalReplacement.Persistence.Extensions.Persistence;

public static class Extension
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("CosmosDbEmulator");

        services.AddDbContext<DataContext>(options => options
        .UseCosmos(configuration.GetConnectionString("CosmosDbEmulator"),
            RegularConstants.CapitalApplication)
        .EnableSensitiveDataLogging());

        using (var serviceScope = services.BuildServiceProvider().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
        }
    }



}

