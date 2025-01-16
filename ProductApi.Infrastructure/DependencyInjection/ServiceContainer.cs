
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Database;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            /*Here we have to add Database connectivity and authentication scheme, becouse we have done this in 
             SharedLibrary.DependencyInjection.SharedServiceContainer 
             we dont have to do it again, insted we can just register it in here*/

            // Register shared infrastructure services (database, logging, etc.)
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            // Register local infrastructure services like repositories
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as GlobalException for handling external errors and Listen to only API Gateway/block all outside calls
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
