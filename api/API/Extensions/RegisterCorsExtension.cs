namespace API.Extensions;

public static class RegisterCorsExtension
{
    public static IServiceCollection RegisterCors(this IServiceCollection services) {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.
                    WithOrigins("*").
                    AllowAnyHeader().
                    AllowAnyMethod();
            });
        });
        return services;
    }       
}