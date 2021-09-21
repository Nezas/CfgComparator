namespace CfgComparator.Models
{
    /// <summary>
    /// Holds id and value of the configuration file property.
    /// </summary>
    public class Parameter
    {
        public string Id { get; }
        public string Value { get; }

        public Parameter(string id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
