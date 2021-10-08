using CfgComparator.Models;
using CfgComparator.Enums;

namespace CfgComparator
{
    /// <summary>
    /// Compares two configuration files.
    /// </summary>
    public class ConfigurationsComparator
    {
        /// <summary>
        /// Compares two given <see cref="ConfigurationData"/>.
        /// </summary>
        /// <param name="source">Source configuration file.</param>
        /// <param name="target">Target configuration file.</param>
        /// <returns>Returns <see cref="ConfigurationsCompareResult"/></returns>
        public ConfigurationsCompareResult Compare(ConfigurationData source, ConfigurationData target)
        {
            var configurationsCompareResult = new ConfigurationsCompareResult(source, target);

            foreach(var sourceParameter in source.Parameters)
            {
                var targetParameter = target.Parameters.Find(x => x.Id == sourceParameter.Id);
                if(targetParameter == null)
                {
                    var parameterDifference = new ParameterDifference(sourceParameter.Id, sourceParameter, null, ParameterStatus.Removed);
                    configurationsCompareResult.Differences.Add(parameterDifference);
                }
                else
                {
                    if(sourceParameter.Value == targetParameter.Value)
                    {
                        var parameterDifference = new ParameterDifference(sourceParameter.Id, sourceParameter, targetParameter, ParameterStatus.Unchanged);
                        configurationsCompareResult.Differences.Add(parameterDifference);
                    }
                    else
                    {
                        var parameterDifference = new ParameterDifference(sourceParameter.Id, sourceParameter, targetParameter, ParameterStatus.Modified);
                        configurationsCompareResult.Differences.Add(parameterDifference);
                    }
                }
            }

            foreach(var targetParameter in target.Parameters)
            {
                var sourceParameter = source.Parameters.Find(x => x.Id == targetParameter.Id);
                if(sourceParameter == null)
                {
                    var parameterDifference = new ParameterDifference(targetParameter.Id, null, targetParameter, ParameterStatus.Added);
                    configurationsCompareResult.Differences.Add(parameterDifference);
                }
            }
            return configurationsCompareResult;
        }
    }
}
