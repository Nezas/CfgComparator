using System.Linq;
using System.Collections.Generic;
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
            var allParameters = source.Parameters.Concat(target.Parameters).ToList();
            List<Parameter> validatedParameters = new();

            foreach(var parameter in allParameters)
            {
                var sourceParameter = source.Parameters.Find(x => x.Id == parameter.Id);
                var targetParameter = target.Parameters.Find(x => x.Id == parameter.Id);

                if(validatedParameters.Find(p => p.Id == parameter.Id) == null)
                {
                    var parameterStatus = GetStatus(parameter, sourceParameter, targetParameter);
                    var parameterDifference = new ParameterDifference(parameter.Id, sourceParameter, targetParameter, parameterStatus);
                    configurationsCompareResult.Differences.Add(parameterDifference);
                    validatedParameters.Add(parameter);
                }
            }
            return configurationsCompareResult;
        }

        private ParameterStatus GetStatus(Parameter parameter, Parameter sourceParameter, Parameter targetParameter)
        {
            if(targetParameter == null) 
            {
                return ParameterStatus.Removed;
            }
            else if(sourceParameter == null)
            {
                return ParameterStatus.Added;
            }

            if(sourceParameter.Value == targetParameter.Value)
            {
                return ParameterStatus.Unchanged;
            }
            return ParameterStatus.Modified;
        }
    }
}
