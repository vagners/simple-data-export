using System;
using System.IO;

namespace SimpleDataExport.ConsoleTest.ReportFileTypes
{
    public class Positional : ReportBase
    {
        public override ReportBase Generate(StreamWriter streamWriter)
        {
            foreach (var row in Builder.Rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var cell = row[i];
                    var headerConfig = Builder.Headers[i];
                    var width = Convert.ToInt32(headerConfig.Width);

                    var value = string.Empty;
                    switch (headerConfig.ColumnType)
                    {
                        case ColumnType.Number:
                            value = Convert.ToString(cell.Value).Truncate(width).PadLeft(width, '0');
                            break;
                        case ColumnType.Decimal:
                        case ColumnType.Currency:
                            var formatedValue = Convert.ToInt32(Convert.ToDecimal(cell.Value) * 100M);
                            value = Convert.ToString(formatedValue).Truncate(width).PadLeft(width, '0');
                            break;

                        case ColumnType.String:
                            value = Convert.ToString(cell.Value).Truncate(width).PadRight(width, ' ').Replace("\n", string.Empty).Replace("\r", string.Empty);
                            break;

                        case ColumnType.Date:
                        case ColumnType.DateTime:
                            if (cell.Value == null)
                                value = string.Empty.PadLeft(width, ' ');
                            else
                                value = Convert.ToDateTime(cell.Value).ToString(headerConfig.MaskFormat).Truncate(width).PadLeft(width, ' ');
                            break;
                        case ColumnType.Function:
                            value = headerConfig.Func(cell.Value).Truncate(width).PadRight(width, ' ');
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    streamWriter.Write(value);
                }
                streamWriter.WriteLine();
            }

            return this;
        }
    }
}
