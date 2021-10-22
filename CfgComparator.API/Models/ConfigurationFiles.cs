namespace CfgComparator.API.Models
{
    public class ConfigurationFiles
    {
        public string SourceName { get; }
        public string TargetName { get; }

        public ConfigurationFiles(string sourceName, string targetName)
        {
            SourceName = sourceName;
            TargetName = targetName;
        }
    }
}
