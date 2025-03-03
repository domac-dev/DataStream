using DataStream.Exceptions;
using DataStream.Serialization;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    public class SearchFilterDS
    {
        private string _searchTerm;
        private LogicalOperatorType _logicalOperator;
        private OperandType _operandType;
        private HashSet<string> _properties;

        [JsonPropertyName("term")]
        public string SearchTerm
        {
            get => _searchTerm;
            set => _searchTerm = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("SearchTerm cannot be null or empty")
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

        [JsonPropertyName("properties")]
        public HashSet<string> Properties
        {
            get => _properties;
            set => _properties = value ?? [];
        }

        public SearchFilterDS(string searchTerm, HashSet<string> properties, OperandType operandType = OperandType.Contains,
            LogicalOperatorType logicalOperator = LogicalOperatorType.OR)
        {
            ArgumentExceptionDS.ThrowIfNullOrEmpty(searchTerm);

            _searchTerm = searchTerm;
            _logicalOperator = logicalOperator;
            _operandType = operandType;
            _properties = properties ?? [];
        }
    }
}