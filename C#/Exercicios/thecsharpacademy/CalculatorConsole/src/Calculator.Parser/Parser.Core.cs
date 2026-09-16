using Calculator.Core;

namespace Calculator.Parser;

class Parser
{
    public static ParseResult Parser(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return ParseResult.Failure("Empty expression");
        }

        ReadOnlySpan<char> trimmed = expression.AsSpan().Trim();

        int opIndex = trimmed.IndexOfAny("+", "-", "*", "/");
    }
}
