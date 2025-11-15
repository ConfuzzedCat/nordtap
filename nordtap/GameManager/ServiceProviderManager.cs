using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nordtap.LocalStorage;

namespace Nordtap.GameManager;

public static class ServiceProviderManager
{

    private static readonly IServiceProvider ServiceProvider;

    static  ServiceProviderManager()
    {
        ServiceProvider = new ServiceCollection()
            .AddLogging(opt =>
            {
                opt.AddSimpleConsole()
                    .AddDebug()
                    .SetMinimumLevel(LogLevel.Debug);
            })
            .AddScoped<ILocalStorageService, RamLocalStorageServiceImpl>()
            .BuildServiceProvider();
    }
    public static IServiceProvider GetServiceProvider()
    {
        return ServiceProvider;
    }
}