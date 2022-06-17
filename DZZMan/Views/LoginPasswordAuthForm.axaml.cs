using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;

namespace DZZMan.Views;

public partial class LoginPasswordAuthForm : ReactiveWindow<LoginPasswordAuthFormViewModel>
{
    public LoginPasswordAuthForm(bool previousAttemptFailed = false)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        ViewModel = new LoginPasswordAuthFormViewModel(previousAttemptFailed);
    }
    
    public LoginPasswordAuthForm()
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