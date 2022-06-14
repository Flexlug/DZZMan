using DZZMan.Models.CapturedAreaCalc;

namespace DZZMan.Services;

public interface ICapturedAreaTasksProvider
{
    public void AddTask(CapturedAreaTask task);
    public CapturedAreaTask GetTask();
}