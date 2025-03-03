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
    }
}