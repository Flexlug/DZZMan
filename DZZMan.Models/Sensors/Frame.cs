

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.Sensors
{
    public class Frame : Sensor
    {        
        /// <summary>
        /// Ширина кадра
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Длина кадра
        /// </summary>
        public double Length{ get; set; }

        /// <summary>s
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public override SensorType SensorType { get; set; } = SensorType.Frame;
    }
}
