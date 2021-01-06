using System;
using System.IO;
using System.Linq;

namespace SimpleDataExport.ConsoleTest.ReportFileTypes
{
    public class Delimited : ReportBase
    {
        private string _delimiter { get; set; }

        public Delimited(string delimiter)
        {
            _delimiter = delimiter;
        }

        public override ReportBase Generate(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(string.Join(_delimiter, Builder.Headers.Select(x => x.Header)));

            foreach (var row in Builder.Rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var cell = row[i];
                    var headerConfig = Builder.Headers[i];

                    var value = string.Empty;
                    switch (headerConfig.ColumnType)
                    {
                        case ColumnType.Number:
                        case ColumnType.Decimal:
                        case ColumnType.Currency:
                            value = Convert.ToString(cell.Value).Replace(',', '.');
                            break;

                        case ColumnType.String:
                            value = Convert.ToString(cell.Value);
                            break;

                        case ColumnType.Date:
                        case ColumnType.DateTime:
                            if (cell.Value != null)
                                value = Convert.ToDateTime(cell.Value).ToString(headerConfig.MaskFormat);
                            break;

                        case ColumnType.Function:
                            value = headerConfig.Func(cell.Value);
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    value = value.Replace("\r", string.Empty).Replace("\n", "|").Replace(_delimiter, string.Empty);
                    value += _delimiter;
                    streamWriter.Write(value);
                }
                streamWriter.WriteLine();
            }

            return this;
        }
    }
}
