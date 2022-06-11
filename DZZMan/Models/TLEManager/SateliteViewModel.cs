using SGPdotNET.TLE;

using DZZMan.Models;
using DZZMan.Models.TLEManager;

namespace DZZMan.Models.TLEManager;

public class SateliteViewModel
{
    /// <summary>
    /// Название спутника
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Cospar ID
    /// </summary>
    public string CosparID { get; set; }
    
    /// <summary>
    /// Данные о спутнике из БД 
    /// </summary>
    public Satellite Satellite { get; set; }
    
    /// <summary>
    /// Слой, отвечающий за отображение спутника на карте
    /// </summary>
    public SateliteLayer Layer { get; set; }
}