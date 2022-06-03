
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.TLEManager
{
    /// <summary>
    /// Обертка поверх TLE для окна TLEManager
    /// </summary>
    public class TLEWrapper
    {
        /// <summary>
        /// Отметка о выборе пользователя
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Полная версия TLE
        /// </summary>
        public Tle TLE { get; set; }
    }
}
