
using CapitalPlacement.Application;

using System.Reflection;


namespace CapitalReplacement.Application.Extensions
{
    public static class Extension
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IProgrammeService, ProgrammeService>();
            services.AddScoped<ICandidateApplicationService, CandidateApplicationService>();
        }
    }
}
