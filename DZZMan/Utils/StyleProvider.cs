using Mapsui.Styles;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Styles.Thematics;

namespace DZZMan.Utils
{
    public static class StyleProvider
    {
        public static Style DefaultVectorStyle() => new VectorStyle()
        {
            Fill = new Brush(Color.FromArgb(50, 255, 0, 0)),
            Outline = new Pen()
            {
                Color = Color.Black,
                Width = 0.5
            },
            Line = new Pen()
            {
                Color = Color.Black,
                Width = 2,
                StrokeJoin = StrokeJoin.Round
            }
        };

        public static Style SatellitePointStyle() => new CalloutStyle()
        {
            Color = Color.Green
        };
    }
}
