using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;

namespace DZZMan.Views;

public partial class CapturedAreaCalc : ReactiveWindow<CapturedAreaCalcViewModel>
{
    public CapturedAreaCalc()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        var viewModel = new CapturedAreaCalcViewModel();
        ViewModel = viewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}