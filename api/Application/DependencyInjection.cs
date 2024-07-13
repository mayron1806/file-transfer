using Application.Jobs;
using Application.Services.PlanService;
using Application.UseCases.ActiveAccount;
using Application.UseCases.ConfirmFileReceive;
using Application.UseCases.CreateAccount;
using Application.UseCases.CreateOrganization;
using Application.UseCases.ForgetPassword;
using Application.UseCases.Login;
using Application.UseCases.CreateSendTransfer;
using Application.UseCases.ResetPassword;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Application.UseCases.CreateReceiveTransfer;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services
        services.AddScoped<IPlanService, PlanService>();
        // validations
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<CreateAccountInputDto>, CreateAccountValidator>();
        services.AddScoped<IValidator<LoginInputDto>, LoginValidator>();
        services.AddScoped<IValidator<ResetPasswordInputDto>, ResetPasswordValidator>();
        services.AddScoped<IValidator<ForgetPasswordInputDto>, ForgetPasswordValidator>();
        services.AddScoped<IValidator<ActiveAccountInputDto>, ActiveAccountValidator>();
        services.AddScoped<IValidator<CreateSendTransferInputDto>, CreateSendTransferValidator>();
        services.AddScoped<IValidator<CreateReceiveTransferInputDto>, CreateReceiveTransferValidator>();
        services.AddScoped<IValidator<ConfirmFileReceiveInputDto>, ConfirmFileReceiveValidator>();
        
        // use cases
        services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IActiveAccountUseCase, ActiveAccountUseCase>();
        services.AddScoped<IForgetPasswordUseCase, ForgetPasswordUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
        services.AddScoped<ICreateOrganizationUseCase, CreateOrganizationUseCase>();
        services.AddScoped<ICreateSendTransfer, CreateSendTransferUseCase>();
        services.AddScoped<ICreateReceiveTransfer, CreateReceiveTransferUseCase>();
        services.AddScoped<IConfirmFileReceive, ConfirmFileReceiveUseCase>();

        services.AddQuartz(q =>
        {
            var resetUploadDayCountKey = new JobKey("ResetUploadDayCount");
            var deleteExpiredFilesKey = new JobKey("deleteExpiredFiles");

            q.AddJob<ResetUploadDayCount>(opts => opts.WithIdentity(resetUploadDayCountKey));
            q.AddJob<DeleteExpiredFile>(opts => opts.WithIdentity(deleteExpiredFilesKey));

            q.AddTrigger(opts => opts
                .ForJob(resetUploadDayCountKey)
                .WithIdentity("ResetUploadDayCount-trigger")
                .WithCronSchedule("0 0 0 * * ? *") // todo dia meia noite
            );
            q.AddTrigger(opts => opts
                .ForJob(deleteExpiredFilesKey)
                .WithIdentity("deleteExpiredFiles-trigger")
                .WithCronSchedule("0 0 * * * ? *") // todo dia a cada hora
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
