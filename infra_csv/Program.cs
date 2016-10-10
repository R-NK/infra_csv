using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using System.Diagnostics;

namespace infra_csv
{
    class Program
    {
        static void Main(string[] args)
        {
            if (System.IO.File.Exists(args[0]))
            {
                using (var r = new StreamReader(args[0]))
                {
                    var csv = new CsvReader(r);
                    csv.Configuration.HasHeaderRecord = false;
                    csv.Configuration.RegisterClassMap<CsvMapper>();

                    // データを読み出し
                    var records = csv.GetRecords<Record>();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("\"lang\"\n{\n\t\"Language\" \"japanese\"\n\t\"Tokens\"\n\t{\n");

                    // 出力
                    foreach (var record in records)
                    {
                        if (record.en.Length == 0)
                        {
                            sb.Append("\t\t").Append(record.tag).Append("\n");
                        }
                        else
                        {
                            if (record.jp.Length == 0)
                            {
                                sb.Append("\t\t").Append("\"").Append(record.tag).Append("\"").Append("\t").Append("\"").Append(record.en).Append("\"").Append("\n");
                            }
                            else
                            {
                                sb.Append("\t\t").Append("\"").Append(record.tag).Append("\"").Append("\t").Append("\"").Append(record.jp).Append("\"").Append("\n");
                            }
                        }
                    }
                    sb.Append("\t}\n}");
                    using (StreamWriter sw = new StreamWriter("subtitles_japanese.txt", false, Encoding.GetEncoding("utf-16")))
                    {
                        sw.Write(sb.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine("File Not Found");
            }        
        }
    }
}

class Record
{
    public string tag { get; set; }
    public string en { get; set; }
    public string jp { get; set; }
    public string go { get; set; }
}
class CsvMapper : CsvClassMap<Record>
{
    public CsvMapper()
    {
        Map(m => m.tag).Index(0);
        Map(m => m.en).Index(1);
        Map(m => m.jp).Index(2);
        Map(m => m.go).Index(3);
    }
}