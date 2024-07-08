using Infrastructure.Services.Email;
using Infrastructure.Services.JWT;
using Infrastructure.Settings;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // db
        services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=../database.db"));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<JWTService>();
        
        return services;
    }
}
