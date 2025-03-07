using DataStream.Exceptions;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    /// <summary>
    /// Specifies a sorting rule for a data query, defining the property to sort by and the sort direction (ascending or descending).
    /// </summary>
    public class SortFilterDS
    {
        /// <summary>
        /// Backing field for the PropertyName property, storing the name of the property to sort.
        /// </summary>
        private string _propertyName;

        /// <summary>
        /// Backing field for the Ascending property, storing the sort direction.
        /// </summary>
        private bool _ascending;

        /// <summary>
        /// Gets or sets the name of the property to sort by (e.g., "Name", "Salary"). Cannot be null or empty.
        /// </summary>
        [JsonPropertyName("property")]
        public string PropertyName
        {
            get => _propertyName;
            set => _propertyName = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("PropertyName cannot be null or empty")
                : value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sort is ascending (true) or descending (false).
        /// </summary>
        [JsonPropertyName("ascending")]
        public bool Ascending
        {
            get => _ascending;
            set => _ascending = value;
        }

        /// <summary>
        /// Initializes a new instance of SortFilterDS with the specified property name and sort direction.
        /// </summary>
        /// <param name="propertyName">The name of the property to sort by.</param>
        /// <param name="ascending">True for ascending order, false for descending. Defaults to true.</param>
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