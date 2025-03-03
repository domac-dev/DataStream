using DataStream.Exceptions;

namespace DataStream.Serialization
{
    public static class LogicalOperatorTypeParser
    {
        private static readonly Dictionary<LogicalOperatorType, string> OperatorToString = new()
        {
            { LogicalOperatorType.AND, "and" },
            { LogicalOperatorType.OR, "or" }
        };

        private static readonly Dictionary<string, LogicalOperatorType> StringToOperator = [];

        static LogicalOperatorTypeParser()
        {
            foreach (var kvp in OperatorToString)
            {
                StringToOperator[kvp.Value] = kvp.Key;
            }
        }

        public static string ToShortString(LogicalOperatorType logicalOperator) =>
            OperatorToString.TryGetValue(logicalOperator, out var result) ? result : throw new SerializationExceptionDS("Invalid logical operator");

        public static LogicalOperatorType FromShortString(string shortString) =>
            StringToOperator.TryGetValue(shortString.ToLower(), out var result) ? result : throw new SerializationExceptionDS("Invalid short string for logical operator");
    }
}
