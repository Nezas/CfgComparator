using System.Collections.Generic;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds source and target <see cref="ConfigurationData"/> and list of <see cref="ParameterDifference"/>.
    /// </summary>
    public class ConfigurationsCompareResult
    {
        public ConfigurationData Source { get; }
        public ConfigurationData Target { get; }
        public List<ParameterDifference> Differences { get; set; } = new();

        public ConfigurationsCompareResult(ConfigurationData source, ConfigurationData target)
        {
            Source = source;
            Target = target;
        }
    }
}
