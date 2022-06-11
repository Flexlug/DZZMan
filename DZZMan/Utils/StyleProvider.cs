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
            Fill = new Brush(Color.FromArgb(50, 255, 0, 0)),
            Outline = null,
            Line = {
                Color = Color.Black
            }
        };
    }
}
