using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public ObservableCollection<string> ParametersStrings { get; set; } = new();

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
        
        if (Source is USGSSource)
            selectedIndex = 0;
        
        if (Source is USGSEESource)
            selectedIndex = 1;

        if (Source is CopernicusSource)
            selectedIndex = 2;

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
        
        ParametersListBox = this.Find<ListBox>("ParametersListBox");

        ParametersListBox.Items = ParametersStrings;
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NewParameterNameTB.Text))
            return;
        
        if (string.IsNullOrEmpty(NewParameterValueTB.Text))
            return;
        
        if (Parameters.TryAdd(NewParameterNameTB.Text, NewParameterValueTB.Text))
            ParametersStrings.Add($"{NewParameterNameTB.Text} - {NewParameterValueTB.Text}");
    }

    private void Remove_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ParametersListBox.SelectedIndex == -1)
            return;

        var elementIndex = ParametersListBox.SelectedIndex; 
        var element = Parameters.ElementAt(ParametersListBox.SelectedIndex);
        
        Parameters.Remove(element.Key);
        ParametersStrings.RemoveAt(elementIndex);
    }

    private void OK_OnClick(object? sender, RoutedEventArgs e)
    {
        switch (ImageSourceType.SelectedIndex)
        {
            case 0:
                Source = new USGSSource()
                {
                    DownloadParametes = Parameters
                };
                break;
            
            case 1:
                Source = new USGSEESource()
                {
                    DownloadParametes = Parameters
                };
                break;
            
            case 2:
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