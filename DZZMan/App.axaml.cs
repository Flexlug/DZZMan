using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BruTile.Wms;
using DZZMan.Services;
using DZZMan.ViewModels;
using DZZMan.Views;

namespace DZZMan
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            ServiceProvider.Configure();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
