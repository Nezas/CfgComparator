using System;
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

        public void InfoParameters(Record source, Record target, Comparator comparator)
        {
            _writer.Write("Content\n");
            _writer.Write(String.Format("{0,-40}", $"\n{source.Name}"));
            _writer.Write(" |   ");
            _writer.Write($"{target.Name}\n");
            for(int i = 0; i < source.InfoParameters.Count; i++)
            {
                _writer.Write(String.Format("{0,-40}", $"{source.InfoParameters[i].Id}: {source.InfoParameters[i].Value}"));
                _writer.Write("|   ");
                _writer.Write($"{target.InfoParameters[i].Id}: {target.InfoParameters[i].Value}\n");
            }
            _writer.Write($"\nComparison statistics: U:{comparator.Unchanged.Count} M:{comparator.Modified.Count} R:{comparator.Removed.Count} A:{comparator.Added.Count}\n");
        }
    }
}
