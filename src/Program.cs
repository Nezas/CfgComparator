using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Writers;

namespace CfgComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            Record source = Reader.Read("../../../CfgData/FMB001-default.cfg");
            Record target = Reader.Read("../../../CfgData/FMB920-default.cfg");

            Comparator comparator = new();
            comparator.Compare(source, target);

            List<string> unchanged = comparator.Unchanged;
            List<string> modified = comparator.Modified;
            List<string> removed = comparator.Removed;
            List<string> added= comparator.Added;

            Output output = new(new ConsoleWriter());
            Menu menu = new(source, target, unchanged, modified, removed, added, output);
            menu.Start();
        }
    }
}
