using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.Sensors
{
    public class Scanner : Sensor
    {
        /// <summary>
        /// Ширина полосы сканирования на Земле в км
        /// </summary>
        public double Swath { get; set; }

        public Scanner()
        {
            SensorType = SensorType.Scanner;
        }
    }
}
