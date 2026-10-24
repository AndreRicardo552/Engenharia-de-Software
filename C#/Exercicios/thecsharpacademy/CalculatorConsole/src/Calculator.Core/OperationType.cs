namespace Calculator.Core;

public enum Operation
{
    Addition,
    Subtraction,
    Division,
    Multiplication,
}

public static class OperationExtensions
{
    public static string ToSymbol(this Operation op) =>
        op switch
        {
            Operation.Addition => "+",
            Operation.Subtraction => "-",
            Operation.Division => "/",
            Operation.Multiplication => "*",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, "Operation not found"),
        };

    public static Operation? FromSymbol(char symbol) =>
        symbol switch
        {
            '+' => Operation.Addition,
            '-' => Operation.Subtraction,
            '*' => Operation.Multiplication,
            '/' => Operation.Division,
            _ => null,
        };
}
