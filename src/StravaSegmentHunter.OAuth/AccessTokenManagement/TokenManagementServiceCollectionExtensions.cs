using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StravaSegmentHunter.OAuth.AccessTokenManagement;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for IServiceCollection to register the token management services
    /// </summary>
    public static class TokenManagementServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the token management services to DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TokenManagementBuilder AddAccessTokenManagement(this IServiceCollection services, Action<AccessTokenManagementOptions> options = null)
        {
            if (options != null)
            {
                services.Configure(options);
            }

            services.AddHttpContextAccessor();
            services.AddAuthentication();
            services.AddDistributedMemoryCache();

            services.TryAddTransient<IAccessTokenManagementService, AccessTokenManagementService>();
            services.TryAddTransient<ITokenClientConfigurationService, DefaultTokenClientConfigurationService>();
            services.TryAddTransient<ITokenEndpointService, TokenEndpointService>();

            // TODO does Strava support this?
            services.AddHttpClient(AccessTokenManagementDefaults.BackChannelHttpClientName);

            services.AddTransient<UserAccessTokenHandler>();

            services.TryAddTransient<IUserTokenStore, AuthenticationSessionUserTokenStore>();

            return new TokenManagementBuilder(services);
        }

        /// <summary>
        /// Adds a named HTTP client for the factory that automatically sends the current user access token
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">The name of the client.</param>
        /// <param name="configureClient">Additional configuration.</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddUserAccessTokenClient(this IServiceCollection services, string name, Action<HttpClient> configureClient = null)
        {
            if (configureClient != null)
            {
                return services.AddHttpClient(name, configureClient)
                    .AddUserAccessTokenHandler();
            }

            return services.AddHttpClient(name)
                .AddUserAccessTokenHandler();
        }

        /// <summary>
        /// Adds the user access token handler to an HttpClient
        /// </summary>
        /// <param name="httpClientBuilder"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddUserAccessTokenHandler(this IHttpClientBuilder httpClientBuilder)
        {
            return httpClientBuilder.AddHttpMessageHandler<UserAccessTokenHandler>();
        }
    }
}