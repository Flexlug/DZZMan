using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZZMan.Models.TLEManager;

namespace DZZMan.Models.MainWindow
{
    public class MainWindowModel
    {
        public SateliteLayer CreateSateliteLayer(Tle tle) => new(tle);
    }
}
