using Calculator.Core;

namespace Calculator.Parser;

class Parser
{
    public static ParseResult ParserExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return ParseResult.Failure("Empty expression");
        }

        ReadOnlySpan<char> trimmed = expression.AsSpan().Trim();

        int opIndex = trimmed.IndexOfAny("+-*/");

        if (opIndex == -1)
            return ParseResult.Failure("No Operator found");
        if (opIndex == 0)
            return ParseResult.Failure("Invalid Expression, Missing left number");
        if (opIndex == trimmed.Length - 1)
            return ParseResult.Failure("Invalid Expression. Missing right number");

        ReadOnlySpan<char> leftSpan = trimmed[..opIndex].Trim();
        ReadOnlySpan<char> rightSpan = trimmed[(opIndex + 1)..].Trim();
        char opChar = trimmed[opIndex];

        if (!double.TryParse(leftSpan, out double leftNumber))
            return ParseResult.Failure("Invalid Expression, use only numbers");
        if (!double.TryParse(rightSpan, out double rightNumber))
            return ParseResult.Failure("Invalid Expression, use only numbers");

        if (!Enum.TryParse(Operation.FromSymbol(opChar), out Operation op))
            return ParseResult.Failure("Invalid Expression, unrecognize operatetor");
    }
}
