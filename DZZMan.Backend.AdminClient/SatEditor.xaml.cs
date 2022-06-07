using DZZMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DZZMan.Models.Sensors;

namespace DZZMan.Backend.AdminClient
{
    /// <summary>
    /// Логика взаимодействия для SatEditor.xaml
    /// </summary>
    public partial class SatEditor : Window
    {
        public Satellite Satellite { get; set; } = null;

        public SatEditor()
        {
            InitializeComponent();
        }

        public SatEditor(Satellite satellite)
        {
            InitializeComponent();
            Satellite = satellite;

            NameTB.Text = satellite.Name;
            CosparIDTB.Text = satellite.CosparId;
            SCNTB.Text = satellite.SCN;
            SensorType.SelectedIndex = (int)satellite.Sensor.SensorType;
            //SwathTB.Text = satellite.Swath.ToString();
            HeightTB.Text = satellite.Height.ToString();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTB.Text) ||
                string.IsNullOrEmpty(SwathTB.Text) ||
                string.IsNullOrEmpty(HeightTB.Text) ||
                string.IsNullOrEmpty(CosparIDTB.Text) ||
                string.IsNullOrEmpty(SCNTB.Text) ||
                SensorType.SelectedIndex == -1)
            {
                MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(SwathTB.Text, out double swath))
            {
                MessageBox.Show("Значение Swath не является дробным числом", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(HeightTB.Text, out double height))
            {
                MessageBox.Show("Значение Swath не является дробным числом", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (Satellite is not null)
            {
                bool equals = (int)Satellite.Sensor.SensorType == SensorType.SelectedIndex;
                
                if (equals)
                {
                    switch (Satellite.Sensor.SensorType)
                    {
                        case Models.SensorType.Scanner:
                            Scanner scanner = Satellite.Sensor as Scanner;
                            if (scanner.Swath != swath)
                                
                            break;
                        
                        case Models.SensorType.Frame:
                            
                            break;
                    }
                }
                
                
                
                if ((int)Satellite.Sensor.SensorType != SensorType.SelectedIndex ||
                    Satellite.Name != NameTB.Text ||
                    Satellite.Swath != swath ||
                    Satellite.Height != height ||
                    Satellite.SCN != SCNTB.Text ||
                    Satellite.CosparId != CosparIDTB.Text)
                {
                    Satellite = new Satellite()
                    {
                        Name = NameTB.Text,
                        CosparId = CosparIDTB.Text,
                        SCN = SCNTB.Text,
                        SensorType = (SensorType)SensorType.SelectedIndex,
                        Swath = swath,
                        Height = height
                    };
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
                Close();
            }
            else
            {
                Satellite = new Satellite()
                {
                    Name = NameTB.Text,
                    CosparId = CosparIDTB.Text,
                    SCN = SCNTB.Text, 
                    SensorType = (SensorType)SensorType.SelectedIndex,
                    Swath = swath,
                    Height = height
                };
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
