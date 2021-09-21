using System.Collections.Generic;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds configuration file data.
    /// </summary>
    public class Record
    {
        public string Name { get; }
        public List<Parameter> InfoParameters { get; } = new();
        public List<Parameter> Parameters { get; } = new();

        public Record(string name)
        {
            Name = name;
        }
    }
}
