using System.Collections.Generic;
using System.Threading.Tasks;
using CopernicusAPI.Model;
using DZZMan.Models;

namespace DZZMan.Services;

public interface IDownloadManager
{
    public void Auth(string login, string password);
    public Task AddDownload(Product product);
    public List<DownloadItem> GetDownloadQueue();
}