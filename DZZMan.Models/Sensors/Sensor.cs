using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSubTypes;
using Newtonsoft.Json;

namespace DZZMan.Models.Sensors
{
    [JsonConverter(typeof(JsonSubtypes), "SensorType")]
    [JsonSubtypes.KnownSubTypeAttribute(typeof(Scanner), Models.SensorType.Scanner)]
    [JsonSubtypes.KnownSubTypeAttribute(typeof(Frame), Models.SensorType.Frame)]
    public class Sensor
    {
        /// <summary>s
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public virtual SensorType SensorType { get; set; }
    }
}
