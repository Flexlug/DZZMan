using Mapsui.Styles;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Utils
{
    public static class StyleProvider
    {
        public static Style DefaultVectorStyle() => new VectorStyle()
        {
            Fill = null,
            Outline = new(Color.Black, 5),
            Line = {
                Color = Color.Black
            }
        };
    }
}
