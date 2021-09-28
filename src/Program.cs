using System;
using System.IO;
using CfgComparator.Models;
using CfgComparator.Writers;

namespace CfgComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationsComparator configurationsComparator = new();
            Output output = new(new ConsoleWriter());

            try
            {
                Console.Write("Enter full SOURCE configuration file path (with .cfg): ");
                string sourceFile = Console.ReadLine();
                ConfigurationData source = ConfigurationFileReader.ReadFromFile($"{sourceFile}");

                Console.Write("Enter full TARGET configuration file path (with .cfg): ");
                string targetFile = Console.ReadLine();
                ConfigurationData target = ConfigurationFileReader.ReadFromFile($"{targetFile}");

                ConfigurationsCompareResult configurationsCompareResult = configurationsComparator.Compare(source, target);

                Menu menu = new(configurationsCompareResult, output);
                menu.MainMenu();
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
