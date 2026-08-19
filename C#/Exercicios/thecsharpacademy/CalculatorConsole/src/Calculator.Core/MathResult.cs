namespace Calculator.Core;

readonly record struct MathResult
{
    public bool IsSuccess { get; init; }
    public double Result { get; init; }
    public string ErrorMessage { get; init; }
}
