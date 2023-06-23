using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZenDays.Core.Configuration;
using ZenDays.Core.Options;

namespace ZenDays.API.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var signingConfigurations = new SigningConfiguration();
            services.AddSingleton(signingConfigurations);

            // Reading TokenOptions config
            var tokenOptions = new TokenOptions();
            tokenOptions = configuration.GetSection(TokenOptions.TokenSettingsSection).Get<TokenOptions>();

            // Adding TokenOptions into IoC container.
            services.AddOptions<TokenOptions>().BindConfiguration(TokenOptions.TokenSettingsSection).ValidateDataAnnotations().ValidateOnStart();

            var key = Encoding.ASCII.GetBytes(tokenOptions.Key);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature).Key;
                paramsValidation.ValidAudience = tokenOptions.Audience;
                paramsValidation.ValidIssuer = tokenOptions.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            return services;
        }
    }
}
