using System;

namespace DZZMan.Models.CapturedAreaCalc;

public class CapturedAreaTask
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool SkipDark { get; set; }
}