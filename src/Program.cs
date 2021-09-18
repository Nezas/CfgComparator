using System;
using CfgComparator.Models;

namespace CfgComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = Reader.Read("../../../CfgData/FMB001-default.cfg");
            var target = Reader.Read("../../../CfgData/FMB920-default.cfg");
        }
    }
}
