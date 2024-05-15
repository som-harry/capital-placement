



namespace CapitalReplacement.Persistence.Extensions.Repository;
public static class Extension
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncRepository<Programme>), typeof(AsyncRepository<Programme>));
        services.AddScoped(typeof(IAsyncRepository<CandidateApplication>), typeof(AsyncRepository<CandidateApplication>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
