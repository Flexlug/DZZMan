using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;

namespace DZZMan.Views;

public partial class DateChanger : ReactiveWindow<DateChangerViewModel>
{
    public DateChanger()
    {
        InitializeComponent();
    }
    
    public DateChanger(DateTime dateTime)
    {
        InitializeComponent();
        
        DataContext = new DateChangerViewModel(dateTime);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}