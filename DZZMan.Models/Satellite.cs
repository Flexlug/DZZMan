
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
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public SensorType SensorType { get; set; }

        /// <summary>
        /// Ширина полосы сканирования на Земле в км
        /// </summary>
        public double Swath { get; set; }

        /// <summary>
        /// Сердняя высота спутника
        /// </summary>
        public double Height { get; set; }
    }
}
