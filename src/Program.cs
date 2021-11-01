using System;
using System.IO;
using Spectre.Console;
using CfgComparator.Writers;
using CfgComparator.Models;
using CfgComparator.Readers;
using CfgComparator.UI;

namespace CfgComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationFileReader = new ConfigurationFileReader();
            var output = new Output(new ConsoleWriter());
            try
            {
                string sourceFilePath = AnsiConsole.Ask<string>("Enter full [red]SOURCE[/] configuration file path (with .cfg): ");
                var source = configurationFileReader.ReadFromFile(Path.GetFileName(sourceFilePath), File.Open(sourceFilePath, FileMode.Open));

                string targetFilePath = AnsiConsole.Ask<string>("Enter full [red]TARGET[/] configuration file path (with .cfg): ");
                var target = configurationFileReader.ReadFromFile(Path.GetFileName(targetFilePath), File.Open(targetFilePath, FileMode.Open));

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
