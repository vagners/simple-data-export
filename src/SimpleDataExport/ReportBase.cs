using System.IO;

namespace SimpleDataExport
{
    public abstract class ReportBase
    {
        public ReportBuilder Builder { get; internal set; }
        public abstract ReportBase Generate(StreamWriter streamWriter);
    }
}
