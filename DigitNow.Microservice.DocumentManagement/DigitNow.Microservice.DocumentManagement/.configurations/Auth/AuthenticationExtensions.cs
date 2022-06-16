using System;
using System.Text;
using HTSS.Platform.Infrastructure.Security;
using HTSS.Platform.Infrastructure.Security.MicrosoftExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DigitNow.Microservice.DocumentManagement.configurations.Auth;

internal static class AuthenticationExtensions
{
    public const string Authorization = "Authorization";
    public const string AuthenticationScheme = "HTSS.Security.Key";

    public static IServiceCollection AddAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection(Authorization);

        if (!section.Exists())
        {
            throw new ApplicationException($"Configurations not found: {Authorization}");
        }

        var options = section.Get<AuthorizationOptions>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = CreateTokenValidationParameters(options);
        });
        services.AddSecurityServices(config =>
        {
            config.AccessTokenSecretKey = options.AccessTokenSecretKey;
            config.RefreshTokenSecretKey = options.RefreshTokenSecretKey;
            config.Issuer = options.Issuer;
            config.Audience = options.Audience;
            config.AccessTokenExpirationTime = options.AccessTokenExpirationTime;
            config.RefreshTokenAddTime = options.RefreshTokenAddTime;
        });

        return services;
    }

    private static TokenValidationParameters CreateTokenValidationParameters(AuthorizationOptions options)
    {
        var signingKey =
            new SymmetricSecurityKey(Encoding.Default.GetBytes(options.AccessTokenSecretKey));
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = true,
            ValidAudience = options.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };
    }
}