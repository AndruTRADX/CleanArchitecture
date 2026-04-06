using System;
using System.Text;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity.Settings;
using CleanArchitecture.Identity.Models;
using CleanArchitecture.Identity.Persistence;
using CleanArchitecture.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Identity;

public static class IdentityServiceRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddDbContext<CAIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Identity"), 
                b => b.MigrationsAssembly(typeof(CAIdentityDbContext).Assembly.FullName));
        });
        /* TODO: Check whether leaving the ApplicationUser instead of the IdentityUser affects something,
        if not, leave it as is */
        services.AddIdentity<IdentityUser, IdentityRole>() 
            .AddEntityFrameworkStores<CAIdentityDbContext>();

        // Services
        services.AddTransient<IAuthService, AuthService>();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["JwtSettings:Key"] 
                    ?? throw new InvalidOperationException("JwtSettings:Key is not configured"))
            ),
        };

        services.AddSingleton(tokenValidationParameters);

        // Token config
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options. SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
        });


        return services;
    }
}
