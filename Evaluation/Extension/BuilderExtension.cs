using Evaluation.Context;
using Evaluation.Log.Interface;
using Evaluation.Log;
using Microsoft.EntityFrameworkCore;
using NLog;
using Evaluation.Services.Interface;
using Evaluation.Services;

namespace Evaluation.Extension
{
    public static class BuilderExtension
    {
        public static IServiceCollection ServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            InjectServices(services); 
            InjectBdd(services,configuration);
            InjectLog(services);
            LogManager.Setup().LoadConfigurationFromFile("./nlog.config");
            return services;
        }
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IClientService,ClientService>();
            services.AddScoped<IAdminService, AdminService>();
        }


        public static void InjectBdd(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EvaluationsContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("Psql")!));
        }

        public static void InjectLog(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
