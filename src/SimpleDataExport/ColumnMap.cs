using System;

namespace SimpleDataExport
{
    public struct ColumnMap
    {
        public string Header;

        public object Value;

        public ColumnType ColumnType;

        public float Width;

        public string MaskFormat;

        public Func<object, string> Func;
    }
}
