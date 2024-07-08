using Application.UseCases.ActiveAccount;
using Application.UseCases.CreateAccount;
using Application.UseCases.ForgetPassword;
using Application.UseCases.Login;
using Application.UseCases.ResetPassword;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // validations
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<CreateAccountInputDto>, CreateAccountValidator>();
        services.AddScoped<IValidator<LoginInputDto>, LoginValidator>();
        services.AddScoped<IValidator<ResetPasswordInputDto>, ResetPasswordValidator>();
        services.AddScoped<IValidator<ForgetPasswordInputDto>, ForgetPasswordValidator>();
        services.AddScoped<IValidator<ActiveAccountInputDto>, ActiveAccountValidator>();
        
        // use cases
        services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IActiveAccountUseCase, ActiveAccountUseCase>();
        services.AddScoped<IForgetPasswordUseCase, ForgetPasswordUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
        return services;
    }
}
