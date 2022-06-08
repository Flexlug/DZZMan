using System;
using DZZMan.API;
using DZZMan.Models.TLEManager;
using DZZMan.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SGPdotNET.TLE;

namespace DZZMan.Services;

public class ServiceProvider
{
    private static ServiceProvider _instance;

    protected IServiceProvider _services;

    private ServiceProvider()
    {
        _services = new ServiceCollection()
            .AddSingleton(new DZZManApi("http://flexlug.ru"))
            .AddSingleton<ISatelliteRepositoryService, SatelliteRepositoryService>()
            .AddTransient<SatelliteManagerModel>()
            .BuildServiceProvider();
    }

    /// <summary>
    /// Получить сервис по его типу
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Get<T>() => _instance._services.GetService<T>();

    public static ServiceProvider Configure()
    {
        _instance = new();
        return _instance;
    }
}