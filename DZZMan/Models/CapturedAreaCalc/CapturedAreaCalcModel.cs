using DZZMan.Services;

namespace DZZMan.Models.CapturedAreaCalc;

public class CapturedAreaCalcModel
{
    private ICapturedAreaTasksProvider _provider;
    
    public CapturedAreaCalcModel(ICapturedAreaTasksProvider provider)
    {
        _provider = provider;
    }

    public void RegisterTask(CapturedAreaTask task) => _provider.AddTask(task);
}