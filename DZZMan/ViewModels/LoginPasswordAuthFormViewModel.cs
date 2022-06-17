using System.ComponentModel;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;

namespace DZZMan.ViewModels;

public class LoginPasswordAuthFormViewModel : ViewModelBase
{    
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }
    private string _login;

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    private string _password;
    
    public bool PreviousAttemptFailed { get; }
    
    public bool IsCanceled { get; set; }
    
    public LoginPasswordAuthFormViewModel(bool previousAttemptFailed = false)
    {
        PreviousAttemptFailed = previousAttemptFailed;
        
        Ok = ReactiveCommand.Create<Window>(x =>
        {
            IsCanceled = false;
            x.Close();
        });

        Cancel = ReactiveCommand.Create<Window>(x =>
        {
            IsCanceled = true;
            x.Close();
        });
    }
    
    public ReactiveCommand<Window, Unit> Ok { get; }
    public ReactiveCommand<Window, Unit> Cancel { get; }
}