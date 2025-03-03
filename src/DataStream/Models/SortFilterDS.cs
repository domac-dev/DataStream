using DataStream.Exceptions;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    public class SortFilterDS
    {
        private string _propertyName;
        private bool _ascending;

        [JsonPropertyName("property")]
        public string PropertyName
        {
            get => _propertyName;
            set => _propertyName = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("PropertyName cannot be null or empty")
                : value;
        }

        [JsonPropertyName("ascending")]
        public bool Ascending
        {
            get => _ascending;
            set => _ascending = value;
        }

        public SortFilterDS(string propertyName, bool ascending = true)
        {
            ArgumentExceptionDS.ThrowIfNullOrEmpty(propertyName);
            _propertyName = propertyName;
            _ascending = ascending;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (SortFilterDS)obj;
            return PropertyName == other.PropertyName &&
                   Ascending == other.Ascending;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (PropertyName?.GetHashCode() ?? 0);
                hash = hash * 23 + Ascending.GetHashCode();
                return hash;
            }
        }
    }
}