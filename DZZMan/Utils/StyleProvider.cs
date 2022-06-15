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
        public static Style DefaultSatelliteLayerStyle() => new VectorStyle()
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

        public static Style DefaultSatellitePointStyle() => new CalloutStyle()
        {
            Fill = new Brush(Color.Orange)
        };
        
        public static Style DefaultCapturedAreaLayerStyle() => new VectorStyle()
        {
            Fill = new Brush(Color.FromArgb(50, 0, 255, 0)),
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
    }
}
