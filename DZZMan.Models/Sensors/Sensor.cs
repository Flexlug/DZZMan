using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonKnownTypes;
using Newtonsoft.Json;

namespace DZZMan.Models.Sensors
{
    [JsonConverter(typeof(JsonKnownTypesConverter<Sensor>))]
    [JsonKnownType(typeof(Scanner), "scanner")]
    [JsonKnownType(typeof(Frame), "frame")]
    public class Sensor
    {
        /// <summary>
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public SensorType SensorType { get; set; }
    }
}
