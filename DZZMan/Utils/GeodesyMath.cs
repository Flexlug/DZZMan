using System;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace DZZMan.Utils;

public class GeodesyMath
{
    /// <summary>
    /// Реализация прямой геодезической задачи
    /// </summary>
    /// <param name="cord1">Координаты первой точки (в прямоугольной системе координат)</param>
    /// <param name="angle">Угол в градусах</param>
    /// <param name="distance">Расстояние в метрах</param>
    /// <returns></returns>
    public static Coordinate DirectGeodTask(Coordinate cord1, double angle, double distance)
    {
        var angleRad = AngleUtility.ToRadians(angle);

        var cord2X = cord1.X + distance * Math.Cos(angleRad);
        var cord2Y = cord1.Y + distance * Math.Sin(angleRad);

        return new Coordinate(cord2X, cord2Y);
    }
}