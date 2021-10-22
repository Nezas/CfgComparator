using System.Linq;
using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Enums;

namespace CfgComparator
{
    /// <summary>
    /// Compares two configuration files.
    /// </summary>
    public static class ConfigurationsComparator
    {
        /// <summary>
        /// Compares two given <see cref="ConfigurationFile"/>.
        /// </summary>
        /// <param name="source">Source configuration file.</param>
        /// <param name="target">Target configuration file.</param>
        /// <returns>Returns <see cref="ConfigurationsCompareResult"/></returns>
        public static List<ParameterDifference> Compare(List<Parameter> sourceParameters, List<Parameter> targetParameters)
        {
            List<ParameterDifference> parameterDifferences = new();
            List<Parameter> validatedParameters = new();
            var allParameters = sourceParameters.Concat(targetParameters);

            foreach(var parameter in allParameters)
            {
                var sourceParameter = sourceParameters.Find(x => x.Id == parameter.Id);
                var targetParameter = targetParameters.Find(x => x.Id == parameter.Id);

                if(validatedParameters.Find(p => p.Id == parameter.Id) == null)
                {
                    var parameterStatus = GetStatus(parameter, sourceParameter, targetParameter);
                    var parameterDifference = new ParameterDifference(parameter.Id, sourceParameter, targetParameter, parameterStatus);
                    parameterDifferences.Add(parameterDifference);
                    validatedParameters.Add(parameter);
                }
            }
            return parameterDifferences;
        }

        private static ParameterStatus GetStatus(Parameter parameter, Parameter sourceParameter, Parameter targetParameter)
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
