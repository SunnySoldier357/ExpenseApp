using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExpenseApp.Extensions
{
    public static class AzureAdAuthenticationBuilderExtensions
    {
        // Public Methods
        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder)
            => builder.AddAzureAd(_ => { });

        public static AuthenticationBuilder AddAzureAd(
            this AuthenticationBuilder builder,
            Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, ConfigureAzureOptions>();
            builder.AddOpenIdConnect();
            return builder;
        }

        private class ConfigureAzureOptions : IConfigureNamedOptions<OpenIdConnectOptions>
        {
            // Private Properties
            private readonly AzureAdOptions _azureOptions;

            // Constructors
            public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
            {
                _azureOptions = azureOptions.Value;
            }

            // Public Methods
            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }

            public void Configure(string name, OpenIdConnectOptions options)
            {
                options.ClientId = _azureOptions.ClientId;
                
                // V2 specific
                options.Authority = $"{_azureOptions.Instance}{_azureOptions.TenantId}/v2.0";   
                options.UseTokenLifetime = true;
                options.RequireHttpsMetadata = false;
                // Accept several tenants
                options.TokenValidationParameters.ValidateIssuer = false;
            }
        }
    }
}