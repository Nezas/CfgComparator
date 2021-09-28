using System.Collections.Generic;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds source and target <see cref="ConfigurationData"/> and list of <see cref="ParameterDifference"/>.
    /// </summary>
    public class ConfigurationsCompareResult
    {
        public ConfigurationData Source { get; set; }
        public ConfigurationData Target { get; set; }
        public List<ParameterDifference> Differences { get; set; } = new();
    }
}
