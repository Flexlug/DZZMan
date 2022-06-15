using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DZZMan.Models;
using DZZMan.Models.Sensors;
using MessageBox.Avalonia.Enums;

namespace DZZMan.AdminClient
{
    public partial class SatEditor : Window
    {
        public Satellite Satellite { get; set; } 
        public bool Result { get; set; }

        public SatEditor()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            Satellite = new Satellite();
            SwitchSensorType(SensorType.Scanner);
        }

        public SatEditor(Satellite satelite)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Satellite = satelite;

            NameTB.Text = Satellite.Name;
            CosparIDTB.Text = Satellite.CosparId;
            SCNTB.Text = Satellite.SCN.ToString();
            SwitchSensorType(Satellite.Sensor.SensorType);
            switch (Satellite.Sensor.SensorType)
            {
                case SensorType.Scanner:
                    Scanner scanner = Satellite.Sensor as Scanner;
                    SwathTB.Text = scanner.Swath.ToString();
                    break;

                case SensorType.Frame:
                    Frame frame = Satellite.Sensor as Frame;
                    WidthTB.Text = frame.Width.ToString();
                    LengthTB.Text = frame.Length.ToString();
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            SensorTypeComboBox = this.Find<ComboBox>("SensorTypeComboBox");
            ScannerSensorParamsGrid = this.Find<Grid>("ScannerSensorParamsGrid");
            FrameSensorParamsGrid = this.Find<Grid>("FrameSensorParamsGrid");

            NameTB = this.Find<TextBox>("NameTB");
            CosparIDTB = this.Find<TextBox>("CosparIDTB");
            SCNTB = this.Find<TextBox>("SCNTB");

            SwathTB = this.Find<TextBox>("SwathTB");
            WidthTB = this.Find<TextBox>("WidthTB");
            LengthTB = this.Find<TextBox>("LengthTB");

            ImageSourcesComboBox = this.Find<ComboBox>("ImageSourcesComboBox");
        }

        private void SensorType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (SensorTypeComboBox is null || SensorTypeComboBox?.SelectedIndex == -1)
                return;

            SwitchSensorType((SensorType)SensorTypeComboBox.SelectedIndex);
        }

        private void SwitchSensorType(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Scanner:
                    SensorTypeComboBox.SelectedIndex = 0;
                    ScannerSensorParamsGrid.IsVisible = true;
                    FrameSensorParamsGrid.IsVisible = false;
                    break;

                case SensorType.Frame:
                    SensorTypeComboBox.SelectedIndex = 1;
                    ScannerSensorParamsGrid.IsVisible = false;
                    FrameSensorParamsGrid.IsVisible = true;
                    break;
            }
        }

        private void OKButton_OnClick(object? sender, RoutedEventArgs e)
        {
            double swath = 0;
            double width = 0;
            double length = 0;

            int scn = 0;

            if (!int.TryParse(SCNTB.Text, out scn))
            {
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Ошибка", 
                    "Значение SCN не является целым числом", 
                    ButtonEnum.Ok, 
                    MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this);
                return;
            }
            
            if (SensorTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Ошибка", 
                    $"Не выбран тип сенсора", 
                    ButtonEnum.Ok, 
                    MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this);
                return;
            }

            if (SensorTypeComboBox.SelectedIndex == (int)SensorType.Scanner)
            {
                if (string.IsNullOrEmpty(SwathTB.Text))
                {
                    MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        "Ошибка", 
                        $"Не введена ширина полосы съемки (Swath)", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this);
                    return;
                }
                
                if (!double.TryParse(SwathTB.Text, out swath))
                {
                    MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        "Ошибка", 
                        "Значение Swath не является дробным числом", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this);
                    return;
                }
            }
            
            if (SensorTypeComboBox.SelectedIndex == (int)SensorType.Frame)
            {
                if (string.IsNullOrEmpty(WidthTB.Text) || 
                    string.IsNullOrEmpty(LengthTB.Text))
                {
                    MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        "Ошибка", 
                        $"Не введены размеры кадра (Width и Length)", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this);
                    return;
                }
                
                if (!double.TryParse(WidthTB.Text, out width) ||
                    !double.TryParse(LengthTB.Text, out length))
                {                
                    MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        "Ошибка", 
                        "Значение Width или Length не является дробным числом", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this);
                    return;
                }
            }
            
            Satellite = new Satellite()
            {
                Name = NameTB.Text,
                CosparId = CosparIDTB.Text,
                SCN = scn,
                Sensor = SensorTypeComboBox.SelectedIndex switch
                {
                    0 => new Scanner()
                    {
                        Swath = swath
                    },
                    1 => new Frame()
                    {
                        Width = width,
                        Length = length
                    }
                }
            };

            Result = true;
            Close();
        }

        private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void ImageSourceTypeComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (ImageSourcesComboBox is null || ImageSourcesComboBox?.SelectedIndex == -1)
                return;

            switch (ImageSourcesComboBox?.SelectedIndex)
            {
                case 0:
                    SensorTypeComboBox.SelectedIndex = 0;
                    ImageSourcesGrid.IsVisible = false;
                    break;
                
                case 1:
                    SensorTypeComboBox.SelectedIndex = 1;
                    ImageSourcesGrid.IsVisible = true;
                    break;
            }
        }

        private void AddImageSource_Click(object? sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveImageSource_Click(object? sender, RoutedEventArgs e)
        {
            
        }
    }
}

