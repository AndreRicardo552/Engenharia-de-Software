using System.Text;

public enum Operator
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}

public static class ExtensionOperators
{
    public static string ToSymbol(this Operator op) =>
        op switch
        {
            Operator.Addition => "+",
            Operator.Subtraction => "-",
            Operator.Multiplication => "*",
            Operator.Division => "/",
            _ => " ",
        };
}

class Utils { }
