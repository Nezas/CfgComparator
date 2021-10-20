using System.Collections.Generic;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds source and target <see cref="ConfigurationFile"/> and list of <see cref="ParameterDifference"/>.
    /// </summary>
    public class ConfigurationsCompareResult
    {
        public ConfigurationFile Source { get; }
        public ConfigurationFile Target { get; }
        public List<ParameterDifference> Differences { get; set; } = new();

        public ConfigurationsCompareResult(ConfigurationFile source, ConfigurationFile target)
        {
            Source = source;
            Target = target;
        }
    }
}
