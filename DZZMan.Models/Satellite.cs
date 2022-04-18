
namespace DZZMan.Models
{
    public class Satellite
    {
        /// <summary>
        /// ID спутника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public SatelliteType Type { get; set; }

        /// <summary>
        /// Угол зрения для кадровый систем (в градусах) и ширина полосы съемки для сканерных систем (в км)
        /// </summary>
        public double FOV { get; set; }


        /// <summary>
        /// Тип аэрофотосъемочной аппаратуры спутника
        /// </summary>
        public enum SatelliteType
        {
            /// <summary>
            /// Сканерная
            /// </summary>
            Scanner,

            /// <summary>
            /// Кадровая
            /// </summary>
            Photo
        }
    }
}
