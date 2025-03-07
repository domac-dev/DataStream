using DataStream.Exceptions;
using DataStream.Serialization;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    /// <summary>
    /// Represents a single filter condition applied to a specific property in a data query (e.g., "Salary > 2500").
    /// Supports various operands and logical operators for combining with other filters.
    /// </summary>
    public class PropertyFilterDS
    {
        /// <summary>
        /// Backing field for the PropertyName property, storing the name of the property to filter.
        /// </summary>
        private string _propertyName;

        /// <summary>
        /// Backing field for the Value property, storing the value to compare against.
        /// </summary>
        private string _value;

        /// <summary>
        /// Backing field for the LogicalOperator property, storing the operator for combining with other filters.
        /// </summary>
        private LogicalOperatorType _logicalOperator;

        /// <summary>
        /// Backing field for the OperandType property, storing the comparison operator.
        /// </summary>
        private OperandType _operandType;

        /// <summary>
        /// Gets or sets the name of the property to filter (e.g., "Salary", "Company.Name"). Cannot be null or empty.
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
        /// Gets or sets the value to compare the property against (e.g., "2500"). Cannot be null or empty.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value
        {
            get => _value;
            set => _value = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("Value cannot be null or empty")
                : value;
        }

        /// <summary>
        /// Gets or sets the logical operator (AND/OR) used to combine this filter with others.
        /// </summary>
        [JsonPropertyName("operator")]
        [JsonConverter(typeof(LogicalOperatorJsonConverter))]
        public LogicalOperatorType LogicalOperator
        {
            get => _logicalOperator;
            set => _logicalOperator = value;
        }

        /// <summary>
        /// Gets or sets the comparison operator (e.g., Equals, GreaterThan) for the filter condition.
        /// </summary>
        [JsonPropertyName("operand")]
        [JsonConverter(typeof(OperandJsonConverter))]
        public OperandType OperandType
        {
            get => _operandType;
            set => _operandType = value;
        }

        /// <summary>
        /// Initializes a new instance of PropertyFilterDS with the specified property, operand, value, and logical operator.
        /// </summary>
        /// <param name="propertyName">The name of the property to filter.</param>
        /// <param name="operandType">The comparison operator for the filter.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="logicalOperator">The logical operator for combining with other filters. Defaults to AND.</param>
        public PropertyFilterDS(string propertyName, OperandType operandType, string value, LogicalOperatorType logicalOperator = LogicalOperatorType.AND)
        {
            ArgumentExceptionDS.ThrowIfNullOrEmpty(propertyName);
            ArgumentExceptionDS.ThrowIfNullOrEmpty(value);

            _propertyName = propertyName;
            _operandType = operandType;
            _value = value;
            _logicalOperator = logicalOperator;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PropertyFilterDS)obj;
            return PropertyName == other.PropertyName &&
                   Value == other.Value &&
                   LogicalOperator == other.LogicalOperator &&
                   OperandType == other.OperandType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (PropertyName?.GetHashCode() ?? 0);
                hash = hash * 23 + (Value?.GetHashCode() ?? 0);
                hash = hash * 23 + LogicalOperator.GetHashCode();
                hash = hash * 23 + OperandType.GetHashCode();
                return hash;
            }
        }
    }
}