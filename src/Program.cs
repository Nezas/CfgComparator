using System;
using System.Collections.Generic;
using System.IO;
using CfgComparator.Models;
using CfgComparator.Writers;

namespace CfgComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            Comparator comparator = new();
            Output output = new(new ConsoleWriter());

            try
            {
                Console.Write("Enter SOURCE configuration file name (with .cfg): ");
                string sourceFile = Console.ReadLine();
                Record source = Reader.Read($"../../../CfgData/{sourceFile}");
                Console.Write("Enter TARGET configuration file name (with .cfg): ");
                string targetFile = Console.ReadLine();
                Record target = Reader.Read($"../../../CfgData/{targetFile}");
                comparator.Compare(source, target);

                List<string> unchanged = comparator.Unchanged;
                List<string> modified = comparator.Modified;
                List<string> removed = comparator.Removed;
                List<string> added = comparator.Added;

                Menu menu = new(source, target, unchanged, modified, removed, added, output);
                menu.Start();
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
