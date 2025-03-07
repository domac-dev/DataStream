using DataStream.Exceptions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream.Expressions
{
    internal static class ExpressionHelper
    {
        public static MemberExpression PropertyExpression(ParameterExpression param, string propertyName) =>
            propertyName.Split('.').Aggregate((Expression)param, Expression.PropertyOrField) as MemberExpression
            ?? throw new ArgumentExceptionDS($"Invalid property path: {propertyName}");

        public static Expression FilterExpression(MemberExpression prop, PropertyFilterDS filter) =>
            filter.OperandType switch
            {
                OperandType.Equals => Expression.Equal(prop, Constant(prop.Type, filter.Value)),
                OperandType.NotEquals => Expression.NotEqual(prop, Constant(prop.Type, filter.Value)),
                OperandType.GreaterThan => Expression.GreaterThan(prop, Constant(prop.Type, filter.Value)),
                OperandType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(prop, Constant(prop.Type, filter.Value)),
                OperandType.LessThan => Expression.LessThan(prop, Constant(prop.Type, filter.Value)),
                OperandType.LessThanOrEqual => Expression.LessThanOrEqual(prop, Constant(prop.Type, filter.Value)),
                OperandType.Contains => Expression.Call(prop, "Contains", null, Constant(prop.Type, filter.Value)),
                OperandType.StartsWith => Expression.Call(prop, "StartsWith", null, Constant(prop.Type, filter.Value)),
                OperandType.EndsWith => Expression.Call(prop, "EndsWith", null, Constant(prop.Type, filter.Value)),
                _ => throw new ArgumentExceptionDS($"Unsupported operand: {filter.OperandType}")
            };

        public static ConstantExpression Constant(Type type, string value) =>
            TypeParser.TryParse(type, value, out var parsed)
                ? Expression.Constant(parsed, type)
                : throw new ArgumentExceptionDS($"Cannot parse '{value}' to {type.Name}");
    }
}
