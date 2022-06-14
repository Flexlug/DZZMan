using System.Collections.Generic;
using DZZMan.Models.CapturedAreaCalc;

namespace DZZMan.Services;

public class CapturedAreaTaskProvider : ICapturedAreaTasksProvider
{
    private Queue<CapturedAreaTask> _queue = new();

    public void AddTask(CapturedAreaTask task) => _queue.Enqueue(task);

    public CapturedAreaTask GetTask() => _queue.Count == 0 ? null : _queue.Dequeue();
}