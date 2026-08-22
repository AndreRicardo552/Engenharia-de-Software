namespace Calculator.Core;

public static class CalculatorEngine
{
    public static MathResult DoOperation(Command command) =>
        command switch
        {
            (var n1, var n2, Operation.Addition) => MathResult.Success(n1 + n2),
            (var n1, var n2, Operation.Subtraction) => MathResult.Success(n1 - n2),
            (var n1, var n2, Operation.Multiplication) => MathResult.Success(n1 * n2),
            (_, 0, Operation.Division) => MathResult.Failure("Can't divide by 0"),
            (var n1, var n2, Operation.Division) => MathResult.Success(n1 / n2),
            _ => MathResult.Failure("Can't recognize the this Math Operation"),
        };
}
