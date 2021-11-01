using System;
using System.Collections.Generic;
using Spectre.Console;
using CfgComparator.Models;
using CfgComparator.Writers;
using CfgComparator.Enums;

namespace CfgComparator.UI
{
    /// <summary>
    /// Writes <see cref="ConfigurationFile"/> parameters using given <see cref="IWriter"/>.
    /// </summary>
    public class Output
    {
        private readonly IWriter _writer;

        public Output(IWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Writes information parameters of the source and target configuration files.
        /// </summary>
        /// <param name="configurationsCompareResult">Source configuration file.</param>
        public void InfoParameters(ConfigurationsCompareResult configurationsCompareResult)
        {
            var table = new Table();
            table.AddColumns("Name", configurationsCompareResult.Source.Name, configurationsCompareResult.Target.Name);           
            for(int i = 0; i < configurationsCompareResult.Source.InfoParameters.Count; i++)
            {
                table.AddRow(configurationsCompareResult.Source.InfoParameters[i].Id, configurationsCompareResult.Source.InfoParameters[i].Value,
                  configurationsCompareResult.Target.InfoParameters[i].Value);
            }
            AnsiConsole.Render(table);

            _writer.Write($"\nComparison statistics:" +
                $" U:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Unchanged).Count}" +
                $" M:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Modified).Count}" +
                $" R:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Removed).Count}" +
                $" A:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Added).Count}\n");
        }

        /// <summary>
        /// Writes compared parameters without given parameter status.
        /// </summary>
        /// <param name="parameterDifferences"></param>
        public void Parameters(List<ParameterDifference> parameterDifferences)
        {
            _writer.Write(String.Format("{0,-10}   {1,-30}   {2,-30}   {3,6}", "ID", "Source Value", "Target Value", "Status\n"));
            foreach(var difference in parameterDifferences)
            {
                PrintParameterDifference(difference);
            }
        }

        /// <summary>
        /// Writes compared parameters.
        /// </summary>
        /// <param name="parameterDifferences"></param>
        /// <param name="parameterStatus"></param>
        public void Parameters(List<ParameterDifference> parameterDifferences, List<ParameterStatus> parameterStatus)
        {
            _writer.Write(String.Format("{0,-10}   {1,-30}   {2,-30}   {3,6}", "ID", "Source Value", "Target Value", "Status\n"));
            foreach(var difference in parameterDifferences)
            {
                foreach(var status in parameterStatus)
                {
                    if(status == difference.Status)
                    {
                        PrintParameterDifference(difference);
                    }
                }
            }
        }

        /// <summary>
        /// Prints parameter difference.
        /// </summary>
        /// <param name="difference"></param>
        private void PrintParameterDifference(ParameterDifference difference)
        {
            Console.BackgroundColor = GetBackgroundColor(difference.Status);
            Console.ForegroundColor = ConsoleColor.Black;
            _writer.Write(String.Format("{0,-10} | {1,-30} | {2,-30} | {3,6}", $"{difference.Id}", $"{difference.SourceValue}", $"{difference.TargetValue}", $"{difference.Status}\n"));
            Console.ResetColor();
        }

        /// <summary>
        /// Get background color of the given parameter status.
        /// </summary>
        /// <param name="parameterStatus"></param>
        /// <returns>Returns <see cref="ConsoleColor"/></returns>
        private ConsoleColor GetBackgroundColor(ParameterStatus parameterStatus) => parameterStatus switch
        {
            ParameterStatus.Unchanged => ConsoleColor.Gray,
            ParameterStatus.Added => ConsoleColor.Green,
            ParameterStatus.Removed => ConsoleColor.Red,
            ParameterStatus.Modified => ConsoleColor.Yellow,
            _ => ConsoleColor.Black,
        };
    }
}
