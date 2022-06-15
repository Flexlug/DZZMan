
using SGPdotNET.TLE;

using DZZMan.Models;
using DZZMan.Models.MainWindow;
using DZZMan.Models.SatelliteManager;

namespace DZZMan.Models.SatelliteManager;

public class SatelliteViewModel
{
    /// <summary>
    /// Название спутника
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Номер по каталогу NORAD
    /// </summary>
    public string SCN { get; set; }
    
    /// <summary>
    /// Отметка о выборе пользователя
    /// </summary>
    public bool IsChecked { get; set; }
    
    /// <summary>
    /// Отметка о наличии в БД информации о сенсоре спутника
    /// </summary>
    public bool HasSensorInfo { get; set; }
    
    /// <summary>
    /// Отметка о наличи в БД источников изображений
    /// </summary>
    public bool HasImageSources { get; set; }
    
    /// <summary>
    /// Информация о спутнике
    /// </summary>
    public SatelliteWrapper SatelliteWrapper { get; set; }
    
    public SatelliteViewModel(SatelliteWrapper satelliteWrapper)
    {
        SatelliteWrapper = satelliteWrapper;

        Name = SatelliteWrapper.Tle.Name;
        SCN = satelliteWrapper.Tle.NoradNumber.ToString();
        HasSensorInfo = satelliteWrapper.HasSatelliteInfo;
        HasImageSources = satelliteWrapper.HasImageSources;
        
        IsChecked = false;
    }
}