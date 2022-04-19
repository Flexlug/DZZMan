using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DZZMan.SGPNET_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Remote URL
            var url = new Uri("https://celestrak.com/NORAD/elements/gp.php?GROUP=resource&FORMAT=tle");

            // Create a provider
            var provider = new RemoteTleProvider(true, url);

            if (!int.TryParse(SDCTB.Text, out int sdc))
            {
                MessageBox.Show("Wrong format", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Alternatively get a specific satellite's TLE
            var issTle = provider.GetTle(sdc);

            var sgp4 = new Sgp4(issTle);
            TLETB.AppendText(issTle.Line1);
            TLETB.AppendText("\n");
            TLETB.AppendText(issTle.Line2);

            OrbitParamsList.Items.Clear();
            OrbitParamsList.Items.Add($"Apogee - {sgp4.Orbit.Apogee}");
            OrbitParamsList.Items.Add($"Argument Perigee - {sgp4.Orbit.ArgumentPerigee}");
            OrbitParamsList.Items.Add($"Ascending node - {sgp4.Orbit.AscendingNode}");
            OrbitParamsList.Items.Add($"BStar - {sgp4.Orbit.BStar}");
            OrbitParamsList.Items.Add($"Eccentricity - {sgp4.Orbit.Eccentricity}");
            OrbitParamsList.Items.Add($"Epoch - {sgp4.Orbit.Epoch}");
            OrbitParamsList.Items.Add($"Inclination - {sgp4.Orbit.Inclination}");
            OrbitParamsList.Items.Add($"Mean Anomoly - {sgp4.Orbit.MeanAnomoly}");
            OrbitParamsList.Items.Add($"Mean Motion - {sgp4.Orbit.MeanMotion}");
            OrbitParamsList.Items.Add($"Perigee - {sgp4.Orbit.Perigee}");
            OrbitParamsList.Items.Add($"Recovered Mean Motion - {sgp4.Orbit.RecoveredMeanMotion}");
            OrbitParamsList.Items.Add($"Recovered Semi-Major Axis - {sgp4.Orbit.RecoveredSemiMajorAxis}");
        }
    }
}
