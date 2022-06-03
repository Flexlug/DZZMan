
using DZZMan.Models.Sensors;

namespace DZZMan.Models
{
    public partial class Satellite
    {
        /// <summary>
        /// Наименование спутника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cospar ID
        /// </summary>
        public string CosparId { get; set; }

        /// <summary>
        /// Номер спутника по каталогу NORAD
        /// </summary>
        public string SCN { get; set; }

        /// <summary>
        /// Информация о сенсоре спутника
        /// </summary>
        public Sensor Sensor { get; set; }

        /// <summary>
        /// Сердняя высота спутника
        /// </summary>
        public double Height { get; set; }
    }
}
