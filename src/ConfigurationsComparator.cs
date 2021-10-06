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
            ConfigurationsCompareResult configurationsCompareResult = new();
            configurationsCompareResult.Source = source;
            configurationsCompareResult.Target = target;

            foreach(var sourceDataPair in source.Parameters)
            {
                var sourceId = sourceDataPair.Id;
                var sourceValue = sourceDataPair.Value;

                var targetData = target.Parameters.Find(x => x.Id == sourceId);
                if(targetData == null)
                {
                    Parameter targetParameter = new(sourceId, null);
                    ParameterDifference parameterDifference = new ParameterDifference(sourceId, sourceDataPair, targetParameter, ParameterStatus.Removed);
                    configurationsCompareResult.Differences.Add(parameterDifference);
                }
                else
                {
                    if(sourceValue == targetData.Value)
                    {
                        ParameterDifference parameterDifference = new ParameterDifference(sourceId, sourceDataPair, targetData, ParameterStatus.Unchanged);
                        configurationsCompareResult.Differences.Add(parameterDifference);
                    }
                    else
                    {
                        ParameterDifference parameterDifference = new ParameterDifference(sourceId, sourceDataPair, targetData, ParameterStatus.Modified);
                        configurationsCompareResult.Differences.Add(parameterDifference);
                    }
                }
            }

            foreach(var targetDataPair in target.Parameters)
            {
                var targetId = targetDataPair.Id;
                var targetValue = targetDataPair.Value;

                var sourceData = source.Parameters.Find(x => x.Id == targetId);
                if(sourceData == null)
                {
                    Parameter sourceParameter = new(targetId, null);
                    ParameterDifference parameterDifference = new ParameterDifference(targetId, sourceParameter, targetDataPair, ParameterStatus.Added);
                    configurationsCompareResult.Differences.Add(parameterDifference);
                }
            }
            return configurationsCompareResult;
        }
    }
}
