using System;
using System.IO;
using Spectre.Console;
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
                string sourceFile = AnsiConsole.Ask<string>("Enter full [red]SOURCE[/] configuration file path (with .cfg): ");
                ConfigurationData source = ConfigurationFileReader.ReadFromFile($"{sourceFile}");

                string targetFile = AnsiConsole.Ask<string>("Enter full [red]TARGET[/] configuration file path (with .cfg): ");
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
