namespace Calculator.Core;

public enum Operation
{
    Addition,
    Subtraction,
    Division,
    Multiplication,
}

public static class ExtensionOperations
{
    public static string ToSymbol(this Operation op) =>
        op switch
        {
            Operation.Addition => "+",
            Operation.Subtraction => "-",
            Operation.Division => "/",
            Operation.Multiplication => "*",
            _ => " ",
        };

    public static Operation? FromSymbol(this char symbol) =>
        symbol switch
        {
            '+' => Operation.Addition,
            '-' => Operation.Subtraction,
            '*' => Operation.Multiplication,
            '/' => Operation.Division,
            _ => null,
        };
}
