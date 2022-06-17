using System;
using CopernicusAPI;
using DZZMan.API;
using DZZMan.Models.CapturedAreaCalc;
using DZZMan.Models.ImageSearcher;
using DZZMan.Models.MainWindow;
using DZZMan.Models.SatelliteManager;
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
            .AddSingleton(new CopernicusApi())
            .AddSingleton<ISatelliteProvider, SatelliteProvider>()
            .AddSingleton<ICapturedAreaTasksProvider, CapturedAreaTaskProvider>()
            .AddSingleton<IDownloadManager, DownloadManager>()
            .AddScoped<SatelliteManagerModel>()
            .AddScoped<CapturedAreaCalcModel>()
            .AddScoped<MainWindowModel>()
            .AddScoped<ImageSearcherModel>()
            .BuildServiceProvider();
    }

    /// <summary>
    /// Получить сервис по его типу
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T? Get<T>() => _instance._services.GetService<T>() 
                                 ?? throw new NullReferenceException($"Couldn't get service {typeof(T)}");

    public static ServiceProvider Configure()
    {
        _instance = new();
        return _instance;
    }
}