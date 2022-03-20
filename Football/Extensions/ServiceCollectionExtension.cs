namespace Football.Extensions
{
    using Football.Core.Contracts;
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Repositories;
    using Football.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    public static class ServiceCollectionExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder services)
        {
            using var scopedServices = services.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<FootballDbContext>();

            //data.Database.Migrate();


            return services;
        }

        public static IServiceCollection ApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFootballDbRepository, FootballDbRepository>();
            services.AddScoped<IUserService, UserService>();

           return services;
        }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<FootballDbContext>(options =>
                 options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();


            return services;
        }
    }
}