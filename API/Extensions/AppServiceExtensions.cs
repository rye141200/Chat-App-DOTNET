using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class AppServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(
                config.GetConnectionString("DefaultConnection")
                );
        });

        //
        /*
         * We have three types of "custom" services to be added:
         * 1) Singleton (you definitely know what that is)
         * 2) Transient, created each time they are requested from service container
         * and it is lightweight
         * 3) Scoped, created once per client request, and this is the most used
         */
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
