using System.Globalization;
using Calculator.Core;

namespace Calculator.Parser;

public class ExpressionParser
{
    public static ParseResult Parse(string expression)
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

        if (!double.TryParse(leftSpan, CultureInfo.InvariantCulture, out double leftNumber))
            return ParseResult.Failure("Invalid Expression, use only numbers");
        if (!double.TryParse(rightSpan, CultureInfo.InvariantCulture, out double rightNumber))
            return ParseResult.Failure("Invalid Expression, use only numbers");

        Operation? op = OperationExtensions.FromSymbol(opChar);

        if (op is null)
            return ParseResult.Failure("Invalid Expression, unrecognize operatetor");

        Command command = new Command(leftNumber, rightNumber, op.GetValueOrDefault());

        return ParseResult.Success(command);
    }
}
