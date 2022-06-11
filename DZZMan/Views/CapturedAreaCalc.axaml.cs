using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DZZMan.Views;

public partial class CapturedAreaCalc : Window
{
    public CapturedAreaCalc()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}