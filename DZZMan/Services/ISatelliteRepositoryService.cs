using System.Collections.Generic;
using SGPdotNET.TLE;

namespace DZZMan.Services;

public interface ISatelliteRepositoryService
{
    /// <summary>
    /// Получить список всех доступных TLE из списка resources
    /// </summary>
    /// <param name="onlyFromDb">Получить TLE только тех спутников, которые есть в БД</param>
    /// <returns></returns>
    public List<Tle> GetTLEs(bool onlyFromDb);
}