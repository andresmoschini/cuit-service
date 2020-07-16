using CuitService.DopplerSecurity;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CuitService.Test
{
    public static class WebApplicationFactoryHelper
    {
        // TODO: review these helpers, it seems the some test resources are not
        // being disposed after debuging tests.
        // info: Microsoft.Hosting.Lifetime[0]
        //       Application is shutting down...
        // info: Microsoft.Hosting.Lifetime[0]
        //       Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.
        // Microsoft.Hosting.Lifetime: Information: Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.

        public static WebApplicationFactory<Startup> Create()
            => new WebApplicationFactory<Startup>();

        public static WebApplicationFactory<Startup> WithBypassAuthorization(this WebApplicationFactory<Startup> factory)
            => factory.WithWebHostBuilder(
                builder => builder.ConfigureTestServices(
                    // TODO: review if this is the best way to bypass the authentication
                    services => services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>()));

        public static WebApplicationFactory<Startup> ConfigureService<TOptions>(this WebApplicationFactory<Startup> factory, Action<TOptions> configureOptions) where TOptions : class
            => factory.WithWebHostBuilder(
                builder => builder.ConfigureTestServices(
                    services => services.Configure(configureOptions)));

        public static WebApplicationFactory<Startup> ConfigureSecurityOptions(this WebApplicationFactory<Startup> factory, Action<DopplerSecurityOptions> configureOptions)
            => factory.ConfigureService(configureOptions);

        public static WebApplicationFactory<Startup> WithDisabledLifeTimeValidation(this WebApplicationFactory<Startup> factory)
            => factory.ConfigureSecurityOptions(
                o => o.SkipLifetimeValidation = true);
    }
}
