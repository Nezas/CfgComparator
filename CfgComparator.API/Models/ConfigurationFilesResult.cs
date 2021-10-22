using System.Collections.Generic;
using CfgComparator.Models;

namespace CfgComparator.API.Models
{
    public class ConfigurationFilesResult
    {
        public string SourceName { get; }
        public string TargetName { get; }
        public List<ParameterDifference> InfoParameters { get; set; } = new();
        public List<ParameterDifference> Parameters { get; set; } = new();

        public ConfigurationFilesResult(string sourceName, string targetName)
        {
            SourceName = sourceName;
            TargetName = targetName;
        }
    }
}
