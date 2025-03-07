using DataStream.Exceptions;
using DataStream.Serialization;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    /// <summary>
    /// Defines a search filter that applies a search term across multiple properties (e.g., "Name starts with 'Jo'").
    /// Supports various operands and logical operators for combining search conditions.
    /// </summary>
    public class SearchFilterDS
    {
        /// <summary>
        /// Backing field for the SearchTerm property, storing the term to search for.
        /// </summary>
        private string _searchTerm;

        /// <summary>
        /// Backing field for the LogicalOperator property, storing the operator for combining search conditions.
        /// </summary>
        private LogicalOperatorType _logicalOperator;

        /// <summary>
        /// Backing field for the OperandType property, storing the comparison operator for the search.
        /// </summary>
        private OperandType _operandType;

        /// <summary>
        /// Backing field for the Properties property, storing the list of properties to search.
        /// </summary>
        private HashSet<string> _properties;

        /// <summary>
        /// Gets or sets the term to search for (e.g., "Jo"). Cannot be null or empty.
        /// </summary>
        [JsonPropertyName("term")]
        public string SearchTerm
        {
            get => _searchTerm;
            set => _searchTerm = string.IsNullOrEmpty(value)
                ? throw new ArgumentExceptionDS("SearchTerm cannot be null or empty")
                : value;
        }

        /// <summary>
        /// Gets or sets the logical operator (AND/OR) used to combine search conditions across properties.
        /// </summary>
        [JsonPropertyName("operator")]
        [JsonConverter(typeof(LogicalOperatorJsonConverter))]
        public LogicalOperatorType LogicalOperator
        {
            get => _logicalOperator;
            set => _logicalOperator = value;
        }

        /// <summary>
        /// Gets or sets the comparison operator (e.g., Contains, StartsWith) for the search condition.
        /// </summary>
        [JsonPropertyName("operand")]
        [JsonConverter(typeof(OperandJsonConverter))]
        public OperandType OperandType
        {
            get => _operandType;
            set => _operandType = value;
        }

        /// <summary>
        /// Gets or sets the collection of property names to apply the search to (e.g., ["Name", "Company.Name"]).
        /// </summary>
        [JsonPropertyName("properties")]
        public HashSet<string> Properties
        {
            get => _properties;
            set => _properties = value ?? [];
        }

        /// <summary>
        /// Initializes a new instance of SearchFilterDS with the specified search term, properties, operand, and logical operator.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <param name="properties">The properties to search across.</param>
        /// <param name="operandType">The comparison operator for the search. Defaults to Contains.</param>
        /// <param name="logicalOperator">The logical operator for combining conditions. Defaults to OR.</param>
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