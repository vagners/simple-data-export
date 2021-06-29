using System;
using System.Collections.Generic;

namespace SimpleDataExport
{
    public sealed class ReportBuilder
    {
        public string ReportName { get; private set; }

        public List<ColumnMap> Headers { get; private set; }

        public List<List<ColumnMap>> Rows { get; private set; }

        public List<ColumnMap> Images { get; private set; }

        public List<ColumnMap> Footers { get; private set; }


        public static ReportBuilder Initialize(string reportName)
        {
            return new ReportBuilder(reportName);
        }

        private ReportBuilder(string reportName)
        {
            ReportName = reportName;
            Headers = new List<ColumnMap>();
            Images = new List<ColumnMap>();
            Footers = new List<ColumnMap>();
            Rows = new List<List<ColumnMap>>();
        }

        public ReportBuilder AddImage(byte[] value)
        {
            Images.Add(new ColumnMap
            {
                Value = value,
                ColumnType = ColumnType.Image
            });

            return this;
        }

        public ReportBuilder AddFooter(params Func<object>[] selectors)
        {
            if (selectors == null || selectors.Length != Headers.Count)
            {
                throw new ArgumentException("O número de seletores deve ser igual ao número de Headers", nameof(selectors));
            }

            if (Footers.Count > 0)
            {
                throw new InvalidOperationException("O Footer já foi adicionado.");
            }

            for (int i = 0; i < selectors.Length; i++)
            {
                Footers.Add(new ColumnMap
                {
                    Value = selectors[i](),
                    ColumnType = Headers[i].ColumnType
                });
            }

            return this;
        }

        public ReportBuilder AddCells<T>(IEnumerable<T> source, params Func<T, object>[] selectors)
        {
            if (selectors == null || selectors.Length != Headers.Count)
            {
                throw new ArgumentException("O número de seletores deve ser igual ao número de Headers", nameof(selectors));
            }

            List<ColumnMap> Cells;
            foreach (T item in source)
            {
                Cells = new List<ColumnMap>();
                for (int i = 0; i < selectors.Length; i++)
                {
                    Cells.Add(new ColumnMap
                    {
                        Value = selectors[i](item),
                        ColumnType = Headers[i].ColumnType
                    });
                }
                Rows.Add(Cells);
            }

            return this;
        }

        public ReportBuilder AddHeader(string header, string maskFormat = null)
        {
            AddHeader(header, ColumnType.Undefined, 0, maskFormat);
            return this;
        }

        public ReportBuilder AddHeader(string header, ColumnType columnType, string maskFormat = null)
        {
            AddHeader(header, columnType, 0, maskFormat);
            return this;
        }
		
		public ReportBuilder AddHeader(string header, Func<object, string> func)
        {
            AddHeader(header, func, 0);
            return this;
        }

        public ReportBuilder AddHeader(string header, Func<object, string> func, float width)
        {
            AddHeader(header, ColumnType.Function, width, null, func);
            return this;
        }

        public ReportBuilder AddHeader(string header, ColumnType columnType, float width, string maskFormat = null)
        {
            AddHeader(header, columnType, width, maskFormat = null, null);
            return this;
        }

        private ReportBuilder AddHeader(string header, ColumnType columnType, float width, string maskFormat, Func<object, string> func = null)
        {
            Headers.Add(new ColumnMap
            {
                Header = header,
                ColumnType = columnType,
                Width = width,
                MaskFormat = maskFormat,
                Func = func
            });

            return this;
        }

        public ReportBase UsingReportType(ReportBase reportType)
        {
            reportType.Builder = this;
            return reportType;
        }
    }
}
