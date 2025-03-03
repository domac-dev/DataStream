using System.Runtime.CompilerServices;

namespace DataStream.Exceptions
{
    public class ArgumentExceptionDS : ArgumentException
    {
        public ArgumentExceptionDS() { }
        public ArgumentExceptionDS(string message) : base(message) { }
        public ArgumentExceptionDS(string message, Exception inner) : base(message, inner) { }

        new internal static void ThrowIfNullOrEmpty(string argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentExceptionDS($"{argumentName ?? "Value"} cannot be null or empty.");
        }

        internal static void ThrowIfNull(object argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        {
            if (argument == null)
                throw new ArgumentExceptionDS($"{argumentName ?? "Value"} cannot be null.");
        }

        internal static void ThrowIfInvalidEnum<TEnum>(TEnum value, string? argumentName = null) where TEnum : struct, Enum
        {
            if (!Enum.IsDefined(value))
                throw new ArgumentExceptionDS(argumentName is null ? $"The value '{value}' is not a valid {typeof(TEnum).Name}."
                    : string.Format("{0}: The value '{1}' is invalid for enum {2}.", argumentName, value, typeof(TEnum).Name));
        }

        internal static ArgumentExceptionDS ThrowIfInvalidType(Type type, string value) => new($"Value '{value}' cannot be parsed to type {type.Name}.");
    }
}
