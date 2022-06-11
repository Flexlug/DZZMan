using DZZMan.Utils;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using NetTopologySuite.Geometries;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.TLEManager
{
    public class SateliteLayer : Layer
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
                GenerateFootprintBoundry();
                UpdateMemoryProvider();
                OnPropertyChanged(nameof(CurrentPoint));
            }
        }
        private DateTime _currentPoint;

        /// <summary>
        /// Период обращения спутника вокруг орбиты
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// Рассчитанная орбита для спутника
        /// </summary>
        public Orbit Orbit { get; set; }

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
        private int _density = 200;

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
        private int _footprintBoundDensity = 120;

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

        /// <summary>
        /// TLE спутника
        /// </summary>
        private Tle _tle;

        /// <summary>
        /// Элементы орбиты
        /// </summary>
        private Sgp4 _sgp;

        private IEnumerable<IFeature> _boundFeature = null;
        private IEnumerable<IFeature> _traceFeature = null;

        /// <summary>
        /// Создать слой на основе TLE
        /// </summary>
        /// <param name="tle">TLE спутника</param>
        /// <returns></returns>
        public SateliteLayer(Tle tle)
        {
            _tle = tle;
            _sgp = new Sgp4(_tle);
            
            Period = TimeSpan.FromMinutes(_sgp.Orbit.Period);

            _currentPoint = DateTime.Now;
            _traceStartPoint = _currentPoint - Period;
            _traceEndPoint = _currentPoint + Period;

            Name = _tle.Name;
            Style = StyleProvider.DefaultVectorStyle();
            
            GenerateTrace();
            GenerateFootprintBoundry();
            UpdateMemoryProvider();
        }

        /// <summary>
        /// Сгенерировать маршрут спутника
        /// </summary>
        private void GenerateTrace()
        {
            var trace = new List<List<Coordinate>>()
            {
                new()
            };

            var traceTime = _traceEndPoint - _traceStartPoint;
            var timeDelta = default(TimeSpan);
            
            var periodCount = traceTime / Period;
            if (periodCount > 2)
            {
                timeDelta = (traceTime / periodCount) / Density;
            }
            else
            {
                timeDelta = traceTime / Density;
            }

            var iterCount = traceTime / timeDelta;

            var i = _traceStartPoint;
            var traceSegmentsIter = 0;

            // Просчитаем первую точку, чтобы в цикле работало сравнение с предыдущей
            var prevPoint = _sgp.FindPosition(i).ToGeodetic();
            i += timeDelta;

            for (int ii = 1; ii < iterCount; i += timeDelta, ii++)
            {
                var geodeticCoordinate = _sgp.FindPosition(i).ToGeodetic();

                if (prevPoint.Longitude.Degrees * geodeticCoordinate.Longitude.Degrees < 0 && (prevPoint.Longitude > 90 || geodeticCoordinate.Longitude > 90))
                {
                    // // Обрабатываем разрыв трассы.
                    // // 
                    // // Вычисление примерного положения граничащей точки
                    // var borderDist = geodeticCoordinate.Longitude.Degrees > 0
                    //     ? 180 - geodeticCoordinate.Longitude.Degrees
                    //     : -180 - geodeticCoordinate.Longitude.Degrees;
                    //
                    // var avgPointsLongDist = Math.Abs(prevPoint.Longitude.Degrees - geodeticCoordinate.Longitude.Degrees);
                    // var avgPointsLatDist = Math.Abs(prevPoint.Longitude.Degrees - geodeticCoordinate.Longitude.Degrees);
                    //
                    // var latApprox = avgPointsLatDist * (borderDist / avgPointsLongDist);
                    //
                    // // Замыкание сегмента с одной стороны
                    // trace[traceSegmentsIter].Add(SphericalMercator
                    //     .FromLonLat(geodeticCoordinate.Longitude.Degrees > 0 ? 179.999999 : -179.999999, prevPoint.Latitude.Degrees + latApprox)
                    //     .ToCoordinate());
                    
                    trace.Add(new List<Coordinate>());
                    traceSegmentsIter++;
                }

                trace[traceSegmentsIter].Add(SphericalMercator
                    .FromLonLat(geodeticCoordinate.Longitude.Degrees, geodeticCoordinate.Latitude.Degrees)
                    .ToCoordinate());

                prevPoint = geodeticCoordinate;
            }

            var lineList = trace.Select(x => new LineString(x.ToArray())).ToList();

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
            var position = _sgp.FindPosition(_currentPoint); 
            var bound = position.GetFootprintBoundary(_footprintBoundDensity);

            
            var boundSegments = new List<List<Coordinate>>()
            {
                new()
            };

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

            DataSource = new MemoryProvider(features);
            DataHasChanged();
        }
    }
}
