using DataStream.Exceptions;

namespace DataStream
{
    internal static class OperandValidator
    {
        internal static bool ValidateOperand(OperandType operand, Type propertyType)
        {
            if (propertyType.IsEnum)
                return operand == OperandType.Equals || operand == OperandType.NotEquals;

            if (ValidOperands.TryGetValue(propertyType, out OperandType[]? validOperands))
                return validOperands.Contains(operand);

            throw new ArgumentExceptionDS($"Unsupported property type '{propertyType.Name}' for filter operand '{operand}'.");
        }

        private static readonly Dictionary<Type, OperandType[]> ValidOperands = new()
        {
            { typeof(string), new[]
                {
                    OperandType.Contains,
                    OperandType.StartsWith,
                    OperandType.EndsWith,
                    OperandType.Equals,
                    OperandType.NotEquals
                }
            },
            { typeof(int), NumericOperands() },
            { typeof(long), NumericOperands() },
            { typeof(double), NumericOperands() },
            { typeof(decimal), NumericOperands() },
            { typeof(float), NumericOperands() },
            { typeof(DateTime), new[]
                {
                    OperandType.Equals,
                    OperandType.NotEquals,
                    OperandType.GreaterThan,
                    OperandType.GreaterThanOrEqual,
                    OperandType.LessThan,
                    OperandType.LessThanOrEqual
                }
            },
            { typeof(bool), new[]
                {
                    OperandType.Equals,
                    OperandType.NotEquals
                }
            }
        };

        private static OperandType[] NumericOperands() =>
        [
            OperandType.Equals,
            OperandType.NotEquals,
            OperandType.GreaterThan,
            OperandType.GreaterThanOrEqual,
            OperandType.LessThan,
            OperandType.LessThanOrEqual
        ];
    }
}
