using CfgComparator.Enums;

namespace CfgComparator.Models
{
    /// <summary>
    /// Holds data of source and target <see cref="Parameter"/> difference.
    /// </summary>
    public class ParameterDifference
    {
        public string Id { get; set; }
        public Parameter Source { get; set; }
        public Parameter Target { get; set; }
        public ParameterStatus Status { get; set; }

        public ParameterDifference(string id, Parameter source, Parameter target, ParameterStatus status)
        {
            Id = id;
            Source = source;
            Target = target;
            Status = status;
        }
    }
}
