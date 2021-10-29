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
        /// Compares two lists of parameters.
        /// </summary>
        /// <param name="sourceParameters">Source file parameters.</param>
        /// <param name="targetParameters">Target file parameters.</param>
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
                    parameterDifferences.Add(GetParameterDifference(parameter, sourceParameter, targetParameter));
                    validatedParameters.Add(parameter);
                }
            }
            return parameterDifferences;
        }

        private static ParameterDifference GetParameterDifference(Parameter parameter, Parameter sourceParameter, Parameter targetParameter)
        {
            if(targetParameter == null) 
            {
                return new ParameterDifference(parameter.Id, sourceParameter.Value, null, ParameterStatus.Removed);
            }
            else if(sourceParameter == null)
            {
                return new ParameterDifference(parameter.Id, null, targetParameter.Value, ParameterStatus.Added);
            }

            if(sourceParameter.Value == targetParameter.Value)
            {
                return new ParameterDifference(parameter.Id, sourceParameter.Value, targetParameter.Value, ParameterStatus.Unchanged);
            }
            return new ParameterDifference(parameter.Id, sourceParameter.Value, targetParameter.Value, ParameterStatus.Modified);
        }
    }
}
