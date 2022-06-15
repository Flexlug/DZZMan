using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData;
using DZZMan.Models.ImageSources;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DZZMan.AdminClient;

public partial class ImageSourceEditor : Window
{
    public ImageSource Source { get; set; } = null;

    public Dictionary<string, string> Parameters { get; set; } = new();

    public ImageSourceEditor()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    public ImageSourceEditor(ImageSource source)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        Source = source;

        var selectedIndex = int.MinValue;
        
        if (Source is USGSEESource)
            selectedIndex = 0;

        if (Source is CopernicusSource)
            selectedIndex = 1;

        ImageSourceType.SelectedIndex = selectedIndex;

        foreach (var param in Source.DownloadParametes)
        {
            Parameters.Add(param.Key, param.Value);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        ImageSourceType = this.Find<ComboBox>("ImageSourceType");

        NewParameterNameTB = this.Find<TextBox>("NewParameterNameTB");
        NewParameterValueTB = this.Find<TextBox>("NewParameterValueTB");
        
        ParametersGrid = this.Find<DataGrid>("ParametersGrid");
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NewParameterNameTB.Text))
            return;
        
        if (string.IsNullOrEmpty(NewParameterValueTB.Text))
            return;
        
        Parameters.Add(NewParameterNameTB.Text, NewParameterValueTB.Text);
    }

    private void Remove_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ParametersGrid.SelectedIndex == -1)
            return;

        Parameters.Remove(Parameters.ElementAt(ParametersGrid.SelectedIndex).Key);
    }

    private void OK_OnClick(object? sender, RoutedEventArgs e)
    {
        switch (ImageSourceType.SelectedIndex)
        {
            case 0:
                Source = new USGSEESource()
                {
                    DownloadParametes = Parameters
                };
                break;
            
            case 1:
                Source = new CopernicusSource()
                {
                    DownloadParametes = Parameters
                };
                break;
        }

        Close();
    }

    private void Cancel_OnClick(object? ssender, RoutedEventArgs e)
    {
        Close();
    }
}