using System.Text;
using gambling.Application.Common.Interfaces;
using gambling.Infrastructure.Context;
using gambling.Infrastructure.Services;
using gambling.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace gambling.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("gamblingDb"));
      
        services.AddOptions();

        services.Configure<JwtAppSettings>(configuration.GetSection(nameof(JwtAppSettings)));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddJWTAuthentication();


        return services;
    }

    private static void AddJWTAuthentication(this IServiceCollection services)
    {
        var jwtAppSettings = services.BuildServiceProvider().GetRequiredService<IOptions<JwtAppSettings>>();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(x =>
        {
            x.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var userService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
                    var id = context.Principal.Identity.Name;
                    if (string.IsNullOrEmpty(id))
                    {
                        // return unauthorized if user no longer exists
                        context.Fail("Unauthorized");
                    }
                    id = userService.UserId;
                    return Task.CompletedTask;
                }
            };
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettings.Value.Secret)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtAppSettings.Value.ValidAudience,
                ValidIssuer = jwtAppSettings.Value.ValidIssuer,
            };
        });
        services.AddAuthorization();


    }

}
