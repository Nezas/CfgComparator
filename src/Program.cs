using System;
using System.IO;
using Spectre.Console;
using CfgComparator.Writers;
using CfgComparator.Models;

namespace CfgComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            Output output = new(new ConsoleWriter());
            try
            {
                string sourceFile = AnsiConsole.Ask<string>("Enter full [red]SOURCE[/] configuration file path (with .cfg): ");
                var source = ConfigurationFileReader.ReadFromFile($"{sourceFile}");

                string targetFile = AnsiConsole.Ask<string>("Enter full [red]TARGET[/] configuration file path (with .cfg): ");
                var target = ConfigurationFileReader.ReadFromFile($"{targetFile}");

                var configurationsCompareResult = new ConfigurationsCompareResult(source, target);
                configurationsCompareResult.Differences = ConfigurationsComparator.Compare(source.Parameters, target.Parameters);

                var menu = new Menu(configurationsCompareResult, output);
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
            catch(InvalidDataException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
