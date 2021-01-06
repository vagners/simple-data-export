using SimpleDataExport.ConsoleTest.ReportFileTypes;
using System;
using System.IO;
using System.Text;

namespace SimpleDataExport.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new[] {
              new {
                Nome = "Teste1",
                Idade = 20
              },
              new {
                Nome = "Teste2",
                Idade = 22
              }
            };

            using var streamWriter = new StreamWriter(File.Create(@"c:\temp\test.txt"), Encoding.GetEncoding("ISO-8859-1"));
            ReportBuilder.Initialize(reportName: "Teste")
                .AddHeader(header: "Nome", columnType: ColumnType.String, width: 40f)
                .AddHeader(header: "Idade", columnType: ColumnType.Number, width: 3f)
                .AddHeader(header: "Idade2", func: TestFunc, width: 6f)
                .AddCells(test,
                    c => c.Nome,
                    c => c.Idade,
                    c => c.Idade)
                //.UsingReportType(new Positional())
                .UsingReportType(new Delimited(";"))
                .Generate(streamWriter);
        }

        public static string TestFunc(object obj)
        {
            return obj + "666";
        }
    }
}
