using System.Collections.Generic;

namespace MaxLifx.Processors.ProcessorSettings
{
    public interface ISettings
    {
        List<string> SelectedLabels { get; set; }
        string FileExtension { get; }
        string OnTimes { get; set; }
        string OffTimes { get; set; }
        bool OffOrOn();
    }
}