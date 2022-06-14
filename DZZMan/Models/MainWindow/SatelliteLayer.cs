using System;
using System.Collections.Generic;
using System.Linq;
using DZZMan.Utils;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using NetTopologySuite.Geometries;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using Coordinate = NetTopologySuite.Geometries.Coordinate;

namespace DZZMan.Models.MainWindow
{
    public class SatelliteLayer : Layer
    {   
        /// <summary>
        /// Timestamp первой точки маршрута спутника
        /// </summary>
        public DateTime TraceStartPoint
        {
            get
            {
                return _traceStartPoint;
            }
            set
            {
                _traceStartPoint = value;
                GenerateTrace();
                UpdateMemoryProvider();
                OnPropertyChanged(nameof(TraceStartPoint));
            }
        }

        private DateTime _traceStartPoint;

        /// <summary>
        /// Timestamp последней точки маршрута спутника
        /// </summary>
        public DateTime TraceEndPoint
        {
            get
            {
                return _traceEndPoint;
            }
            set
            {
                _traceEndPoint = value;
                GenerateTrace();
                UpdateMemoryProvider();
                OnPropertyChanged(nameof(TraceEndPoint));
            }
        }
        private DateTime _traceEndPoint;

        public DateTime CurrentPoint
        {
            get
            {
                return _currentPoint;
            }
            set
            {
                _currentPoint = value;
                GenerateCurrentPoint();
                GenerateFootprintBoundry();
                UpdateMemoryProvider();
                OnPropertyChanged(nameof(CurrentPoint));
            }
        }
        private DateTime _currentPoint;

        /// <summary>
        /// Плотность точек на маршруте (количество точек на один виток)
        /// </summary>
        public int Density
        {
            get
            {
                return _density;
            }
            set
            {
                _density = value;
                GenerateTrace();
                UpdateMemoryProvider();
            }
        }
        private int _density = SatelliteMath.DEFAULT_TRACE_DENSITY;

        /// <summary>
        /// Плотность точек при отображении области видимости
        /// </summary>
        public int FootprintBoundDensity
        {
            get
            {
                return _footprintBoundDensity;
            }
            set
            {
                _footprintBoundDensity = value;
                GenerateFootprintBoundry();
                UpdateMemoryProvider();
            }
        }
        private int _footprintBoundDensity = SatelliteMath.DEFAULT_FOOTPRINT_DINSITY;

        public bool ShowFootprint
        {
            get
            {
                return _showFootprint;
            }
            set
            {
                _showFootprint = value;
                UpdateMemoryProvider();
            }
        }
        private bool _showFootprint = true;

        private IEnumerable<IFeature> _boundFeature = null;
        private IEnumerable<IFeature> _traceFeature = null;
        private IFeature _positionFeature = null;

        private SatelliteWrapper _satellite;

        /// <summary>
        /// Создать слой на основе TLE
        /// </summary>
        /// <param name="tle">TLE спутника</param>
        /// <returns></returns>
        public SatelliteLayer(SatelliteWrapper satellite)
        {
            Name = satellite.Name;
            Style = StyleProvider.DefaultVectorStyle();
            
            _satellite = satellite;

            _currentPoint = DateTime.Now.ToLocalTime();
            _traceStartPoint = _currentPoint - _satellite.Period;
            _traceEndPoint = _currentPoint + _satellite.Period;
            
            GenerateTrace();
            GenerateCurrentPoint();
            GenerateFootprintBoundry();
            UpdateMemoryProvider();
        }
        /// <summary>
        /// Сгенерировать маршрут спутника
        /// </summary>
        private void GenerateTrace()
        {
            var visibleTrace = new List<List<Coordinate>>()
            {
                new()
            };

            var trace = SatelliteMath.CalculateTraceGeod(
                _satellite.Tle,
                _traceStartPoint,
                _traceEndPoint,
                _density);

            var prevPoint = trace[0];
            var traceSegmentsIter = 0;

            for (int i = 1; i < trace.Count; i++)
            {
                var currentPoint = trace[i];
                
                if (prevPoint.Longitude.Degrees * currentPoint.Longitude.Degrees < 0 && (prevPoint.Longitude > 90 || currentPoint.Longitude > 90))
                {
                    visibleTrace.Add(new List<Coordinate>());
                    traceSegmentsIter++;
                }

                var coordinate = SphericalMercator
                    .FromLonLat(currentPoint.Longitude.Degrees, currentPoint.Latitude.Degrees)
                    .ToCoordinate();
                
                visibleTrace[traceSegmentsIter].Add(coordinate);
                
                prevPoint = currentPoint;
            }
            
            var lineList = visibleTrace.Select(x => new LineString(x.ToArray())).ToList();

            _traceFeature = lineList.ToFeatures();
        }

        enum FootprintType
        {
            /// <summary>
            /// Область видимости спутника находится ближе к экватору и/или центру карты и представляет собой обычный круг
            /// (в редких случаях сегментрированный на 2 части из-за близости к краю карты)
            /// </summary>
            Circular,
            
            /// <summary>
            /// Область видимости находится близко к полюсу, из-за чего область видимости занимает весь полюс
            /// </summary>
            Polar
        }

        /// <summary>
        /// Сгенерировать границы видимости спутника
        /// </summary>
        private void GenerateFootprintBoundry()
        {
            var boundSegments = new List<List<Coordinate>>()
            {
                new()
            };

            var bound = SatelliteMath.CalculateFoorptintBounds(_satellite.Tle, _currentPoint, _density);

            var prevPoint = bound.First();
            var segmentIter = 0;

            boundSegments[segmentIter].Add(SphericalMercator
                    .FromLonLat(prevPoint.Longitude.Degrees, prevPoint.Latitude.Degrees)
                    .ToCoordinate());

            for (int i = 1; i < bound.Count; i++)
            {
                var currPoint = bound[i];

                if (prevPoint.Longitude.Degrees * currPoint.Longitude.Degrees < 0 && (prevPoint.Longitude.Degrees > 90 || currPoint.Longitude.Degrees > 90))
                {
                    if (segmentIter == 0)
                    {
                        boundSegments.Add(new List<Coordinate>());
                        segmentIter++;
                    }
                    else
                    {
                        segmentIter = 0;
                    }
                }

                boundSegments[segmentIter].Add(SphericalMercator
                    .FromLonLat(currPoint.Longitude.Degrees, currPoint.Latitude.Degrees)
                    .ToCoordinate());

                prevPoint = currPoint;
            }

            var linearRings = new List<LinearRing>();
            for (int i = boundSegments.Count - 1; i >= 0; i--)
            {
                var currSegment = boundSegments[i];
                
                if (currSegment.Count < 2)
                {
                    boundSegments.Remove(currSegment);
                    continue;
                }
                
                currSegment.Add(currSegment.First());
                linearRings.Add(new LinearRing(currSegment.ToArray()));
            }

            _boundFeature = linearRings
                .Select(x => new Polygon(x).ToFeature())
                .ToList();
        }

        /// <summary>
        /// Создать на слое точку, которая обозначает положение спутника в данный момент
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void GenerateCurrentPoint()
        {
            var currPoint = SatelliteMath.GetSatellitePosition(_satellite.Tle, _currentPoint);
            
            var coord = SphericalMercator
                .FromLonLat(currPoint.Longitude.Degrees, currPoint.Latitude.Degrees)
                .ToCoordinate();

            _positionFeature = new PointFeature(coord.X, coord.Y);
            _positionFeature.Styles.Add(StyleProvider.SatellitePointStyle());
        }


        /// <summary>
        /// Обновить features слоя. Следует вызывать после генерации 
        /// </summary>
        private void UpdateMemoryProvider()
        {
            ClearCache();
            var features = new List<IFeature>();

            if (_traceFeature is not null)
                features.AddRange(_traceFeature);

            if (_showFootprint && _boundFeature is not null)
                features.AddRange(_boundFeature);
            
            if (_positionFeature is not null)
                features.Add(_positionFeature);

            DataSource = new MemoryProvider(features);
            DataHasChanged();
        }
    }
}
