﻿using System;
using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Writers;

namespace CfgComparator
{
    public class Output
    {
        private readonly IWriter _writer;

        public Output(IWriter writer)
        {
            _writer = writer;
        }

        public void InfoParameters(Record source, Record target, List<string> unchanged, List<string> modified, List<string> removed, List<string> added)
        {
            _writer.Write("Content\n");
            _writer.Write(String.Format("{0,-40}", $"\nSource: {source.Name}"));
            _writer.Write(" |   ");
            _writer.Write($"Target: {target.Name}\n");
            for(int i = 0; i < source.InfoParameters.Count; i++)
            {
                _writer.Write(String.Format("{0,-40}", $"{source.InfoParameters[i].Id}: {source.InfoParameters[i].Value}"));
                _writer.Write("|   ");
                _writer.Write($"{target.InfoParameters[i].Id}: {target.InfoParameters[i].Value}\n");
            }
            _writer.Write($"\nComparison statistics: U:{unchanged.Count} M:{modified.Count} R:{removed.Count} A:{added.Count}\n");
        }

        public void Parameters(List<string> status)
        {
            _writer.Write(String.Format("{0,-10}   {1,-30}   {2,-30}   {3,6}", "ID", "Source Value", "Target Value", "Status\n"));
            foreach(var result in status)
            {
                var splittedResult = result.Split(',');
                Console.BackgroundColor = GetBackgroundColor(splittedResult[3]);
                Console.ForegroundColor = ConsoleColor.Black;
                _writer.Write(String.Format("{0,-10} | {1,-30} | {2,-30} | {3,6}", $"{splittedResult[0]}", $"{splittedResult[1]}", $"{splittedResult[2]}", $"{splittedResult[3]}\n"));
                Console.ResetColor();
            }
        }

        private ConsoleColor GetBackgroundColor(string status)
        {
            if(status == "Unchanged") return ConsoleColor.Gray;
            else if(status == "Added") return ConsoleColor.Green;
            else if(status == "Removed") return ConsoleColor.Red;
            else if(status == "Modified") return ConsoleColor.Yellow;
            return ConsoleColor.Black;
        }
    }
}
