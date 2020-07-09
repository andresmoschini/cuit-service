using CuitService.Security;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CuitService.Test
{
    public static class WebApplicationFactoryHelper
    {
        public static WebApplicationFactory<Startup> Create()
            => new WebApplicationFactory<Startup>();

        public static WebApplicationFactory<Startup> ConfigureService<TOptions>(this WebApplicationFactory<Startup> factory, Action<TOptions> configureOptions) where TOptions : class
            => factory.WithWebHostBuilder(
                builder => builder.ConfigureTestServices(
                    services => services.Configure(configureOptions)));

        public static WebApplicationFactory<Startup> ConfigureSecurityOptions(this WebApplicationFactory<Startup> factory, Action<SecurityOptions> configureOptions)
            => factory.ConfigureService(configureOptions);

        public static WebApplicationFactory<Startup> WithDisabledLifeTimeValidation(this WebApplicationFactory<Startup> factory)
            => factory.ConfigureSecurityOptions(
                o => o.SkipLifetimeValidation = true);
    }
}
