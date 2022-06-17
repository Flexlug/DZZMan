using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CopernicusAPI.Model;
using DZZMan.Models;

namespace DZZMan.Services;

public class DownloadManager: IDownloadManager
{
    private string _login;
    private string _password;

    private List<DownloadItem> _downloads = new();
    private List<Thread> _threads = new();
    
    public DownloadManager() {  }
    
    public void Auth(string login, string password)
    {
        _login = login;
        _password = password;
    }

    public async Task AddDownload(Product product)
    {
        var newItem = new DownloadItem(_login, _password, product);

        var thread = new Thread(async () => await newItem.DownloadAsync());
        _threads.Add(thread);
        thread.Start();
        
        _downloads.Add(newItem);
    }

    public List<DownloadItem> GetDownloadQueue()
    {
        return _downloads;
    }
}