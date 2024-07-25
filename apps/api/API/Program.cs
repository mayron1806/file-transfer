using Infrastructure;
using Application;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Settings;
using Microsoft.OpenApi.Models;
using API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

// set app settings file
builder.Configuration.SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

Console.WriteLine("Environment: " + env.EnvironmentName);

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("../Logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Services.AddLogging(log => log.AddSerilog(dispose: true));

// settings
var smtp = builder.Configuration.GetSection("SMTP").Get<SMTPSettings>() ?? throw new Exception("SMTP settings not found");
var jwt = builder.Configuration.GetSection("JWT").Get<JWTSettings>() ?? throw new Exception("JWT settings not found");
var aws = builder.Configuration.GetSection("AWS").Get<AWSSettings>() ?? throw new Exception("AWS settings not found");
builder.Services.AddSingleton(smtp);
builder.Services.AddSingleton(jwt);
builder.Services.AddSingleton(aws);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Tranfer", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.RegisterCors();
builder.Services.AddControllers(options => { 
    options.Filters.Add<CustomExceptionFilter>();
    options.Filters.Add<ValidateModelStateAttribute>();
}).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = env.IsProduction();
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Key)),
    };
    x.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.Headers.Append("Token-Expired", "true");
            await context.Response.WriteAsJsonAsync(new { error = "Não autorizado" });
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction()) app.UseHttpsRedirection();

app.UseMiddleware<CustomAuthorizationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<OrganizationMiddleware>();
app.UseResponseCaching();
app.MapControllers();

app.Run();
