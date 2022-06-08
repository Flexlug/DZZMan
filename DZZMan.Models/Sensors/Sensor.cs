using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.Sensors
{
    public class Sensor
    {
        /// <summary>s
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public virtual SensorType SensorType { get; set; }
    }
}
