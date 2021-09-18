namespace CfgComparator.Models
{
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
