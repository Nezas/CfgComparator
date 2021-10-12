using CfgComparator.Enums;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds data of source and target <see cref="Parameter"/> difference.
    /// </summary>
    public class ParameterDifference
    {
        public string Id { get; }
        public Parameter Source { get; }
        public Parameter Target { get; }
        public ParameterStatus Status { get; }

        public ParameterDifference(string id, Parameter source, Parameter target, ParameterStatus status)
        {
            Id = id;
            Source = source ?? new Parameter(id, "");
            Target = target ?? new Parameter(id, "");
            Status = status;
        }
    }
}
