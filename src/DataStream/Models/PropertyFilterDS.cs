using DataStream.Exceptions;
using DataStream.Serialization;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    public class PropertyFilterDS
    {
        private string _propertyName;
        private string _value;
        private LogicalOperatorType _logicalOperator;
        private OperandType _operandType;

        [JsonPropertyName("property")]
        public string PropertyName
        {
            get => _propertyName;
            set => _propertyName = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("PropertyName cannot be null or empty")
                : value;
        }

        [JsonPropertyName("value")]
        public string Value
        {
            get => _value;
            set => _value = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("Value cannot be null or empty")
                : value;
        }

        [JsonPropertyName("operator")]
        [JsonConverter(typeof(LogicalOperatorJsonConverter))]
        public LogicalOperatorType LogicalOperator
        {
            get => _logicalOperator;
            set => _logicalOperator = value;
        }

        [JsonPropertyName("operand")]
        [JsonConverter(typeof(OperandJsonConverter))]
        public OperandType OperandType
        {
            get => _operandType;
            set => _operandType = value;
        }

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