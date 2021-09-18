using System;
using CfgComparator.Models;

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
        }
    }
}
