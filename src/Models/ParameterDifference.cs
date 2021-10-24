using CfgComparator.Enums;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds data of source and target <see cref="Parameter"/> difference.
    /// </summary>
    public class ParameterDifference
    {
        public string Id { get; }
        public string SourceValue { get; }
        public string TargetValue { get; }
        public ParameterStatus Status { get; }

        public ParameterDifference(string id, Parameter source, Parameter target, ParameterStatus status)
        {
            Id = id;
            SourceValue = source == null ? "" : source.Value;
            TargetValue = target == null ? "" : target.Value;
            Status = status;
        }
    }
}
