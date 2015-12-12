using MaxLifx.Processors.ProcessorSettings;

namespace MaxLifx.Threads
{
    public interface IProcessor
    {
        bool ShowUI { get; set; }
        ISettings Settings { get; set; }
        string SettingsAsXml { get; set; }
        bool TerminateThread { get; set; }
    }
}