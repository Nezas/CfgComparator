using System;
using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Writers;
using CfgComparator.Enums;

namespace CfgComparator
{
    /// <summary>
    /// Writes <see cref="ConfigurationData"/> parameters using given <see cref="IWriter"/>.
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
            _writer.Write("Content\n");
            _writer.Write(String.Format("{0,-40}", $"\nSource: {configurationsCompareResult.Source.Name}"));
            _writer.Write(" |   ");
            _writer.Write($"Target: {configurationsCompareResult.Target.Name}\n");
            for(int i = 0; i < configurationsCompareResult.Source.InfoParameters.Count; i++)
            {
                _writer.Write(String.Format("{0,-40}", $"{configurationsCompareResult.Source.InfoParameters[i].Id}: {configurationsCompareResult.Source.InfoParameters[i].Value}"));
                _writer.Write("|   ");
                _writer.Write($"{configurationsCompareResult.Target.InfoParameters[i].Id}: {configurationsCompareResult.Target.InfoParameters[i].Value}\n");
            }
            _writer.Write($"\nComparison statistics:" +
                $" U:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Unchanged).Count}" +
                $" M:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Modified).Count}" +
                $" R:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Removed).Count}" +
                $" A:{configurationsCompareResult.Differences.FindAll(x => x.Status == ParameterStatus.Added).Count}\n");
        }

        /// <summary>
        /// Writes compared parameters without given parameter status.
        /// </summary>
        /// <param name="configurationsCompareResult"></param>
        public void Parameters(List<ParameterDifference> parameterDifferences)
        {
            _writer.Write(String.Format("{0,-10}   {1,-30}   {2,-30}   {3,6}", "ID", "Source Value", "Target Value", "Status\n"));
            foreach(var difference in parameterDifferences)
            {
                Console.BackgroundColor = GetBackgroundColor(difference.Status);
                Console.ForegroundColor = ConsoleColor.Black;
                _writer.Write(String.Format("{0,-10} | {1,-30} | {2,-30} | {3,6}", $"{difference.Id}", $"{difference.Source.Value}", $"{difference.Target.Value}", $"{difference.Status}\n"));
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Writes compared parameters.
        /// </summary>
        /// <param name="configurationsCompareResult"></param>
        /// <param name="parameterStatus"></param>
        public void Parameters(ConfigurationsCompareResult configurationsCompareResult, ParameterStatus parameterStatus)
        {
            _writer.Write(String.Format("{0,-10}   {1,-30}   {2,-30}   {3,6}", "ID", "Source Value", "Target Value", "Status\n"));
            foreach(var difference in configurationsCompareResult.Differences.FindAll(x => x.Status == parameterStatus))
            {
                Console.BackgroundColor = GetBackgroundColor(difference.Status);
                Console.ForegroundColor = ConsoleColor.Black;
                _writer.Write(String.Format("{0,-10} | {1,-30} | {2,-30} | {3,6}", $"{difference.Id}", $"{difference.Source.Value}", $"{difference.Target.Value}", $"{difference.Status}\n"));
                Console.ResetColor();
            }
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
