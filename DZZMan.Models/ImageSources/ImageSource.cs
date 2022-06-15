namespace DZZMan.Models.ImageSources;

public class ImageSource
{
    public string SourceName { get; init; }
    public Dictionary<string, string> DownloadParametes { get; set; }
}