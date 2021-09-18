using System.Collections.Generic;

namespace CfgComparator.Models
{
    public class Record
    {
        public string Name { get; }
        public List<Parameter> Parameters { get; } = new();

        public Record(string name)
        {
            Name = name;
        }
    }
}
