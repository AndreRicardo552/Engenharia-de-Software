namespace Calculator.Core;

readonly record struct Command
{
    public double Num1 { get; init; }
    public double Num2 { get; init; }
    public Operation Op { get; init; }
}
