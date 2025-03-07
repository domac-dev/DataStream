using DataStream.Exceptions;

namespace DataStream
{
    internal static class TypeParser
    {
        public static bool TryParse(Type type, string value, out object? result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(value)) return false;

            if (type.IsEnum) return Enum.TryParse(type, value, true, out result);

            return Type.GetTypeCode(type) switch
            {
                TypeCode.Int32 => int.TryParse(value, out var r) && (result = r) is not null,
                TypeCode.Double => double.TryParse(value, out var r) && (result = r) is not null,
                TypeCode.Int64 => long.TryParse(value, out var r) && (result = r) is not null,
                TypeCode.Single => float.TryParse(value, out var r) && (result = r) is not null,
                TypeCode.Decimal => decimal.TryParse(value, out var r) && (result = r) is not null,
                TypeCode.Boolean => bool.TryParse(value, out var r) && (result = r) is not null,
                TypeCode.String => (result = value) is not null,
                TypeCode.DateTime => DateTime.TryParse(value, out var r) && (result = r) is not null,
                _ => throw new ArgumentExceptionDS($"Unsupported type: {type.Name}")
            };
        }
    }
}
