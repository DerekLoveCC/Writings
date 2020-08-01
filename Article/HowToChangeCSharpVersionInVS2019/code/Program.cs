using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace DebugWithoutSourceCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var textReader = new StreamReader(@".\test.csv"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false
                };
                var csvReader = new CsvReader(textReader, config);
                var csvDataReader = new CsvDataReader(csvReader);
                while (csvDataReader.Read())
                {
                    for (int i = 0; i < csvDataReader.FieldCount; i++)
                    {
                        Console.Write(csvDataReader.GetString(i) + " ");
                    }
                    Console.WriteLine(" ");
                }
            }

            Console.Read();
        }
    }
}