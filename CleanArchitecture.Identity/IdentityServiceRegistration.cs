using System;
using System.Text;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity.Settings;
using CleanArchitecture.Identity.Models;
using CleanArchitecture.Identity.Persistence;
using CleanArchitecture.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<CAIdentityDbContext>()
            .AddDefaultTokenProviders();

        // Services
        services.AddTransient<IAuthService, AuthService>();

        // Token config
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] 
                        ?? throw new InvalidOperationException("JwtSettings:Key is not configured"))
                ),
            };
        });


        return services;
    }
}
