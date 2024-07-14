using Infrastructure.Services.Email;
using Infrastructure.Services.JWT;
using Infrastructure.Services.Storage;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");
        if (string.IsNullOrEmpty(connectionString)) throw new Exception("Database connection string not found");

        // db
        services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<JWTService>();
        
        return services;
    }
}
