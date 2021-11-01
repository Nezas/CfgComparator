using System.IO;
using CfgComparator.Models;

namespace CfgComparator.Readers
{
    public interface IFileReader
    {
        ConfigurationFile ReadFromFile(string fileName, Stream stream);
    }
}
