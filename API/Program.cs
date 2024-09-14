using API.Data;
using API.Extensions;
using API.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //! Middlewares (order is important)
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        //! we creapted a scope, so that we add services that dont rely on 
        //! dependency injection, and even HTTP requests, thats why we created it down here
        //! because it will be disposed of once we are done and we dont seed data within every
        //! request right??
        /* using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>(); //! throws an exception if service is not found
            //var contextTwo = services.GetService<DataContext>(); //! returns null if not found
            context.Database.Migrate();
            Seed.SeedUsers(context);
        }
        catch (Exception e)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(e, "An error occured during migration");
        } */
        app.Run();
    }
}
