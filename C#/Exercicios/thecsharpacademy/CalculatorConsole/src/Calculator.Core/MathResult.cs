namespace Calculator.Core;

public readonly record struct MathResult
{
    public bool IsSuccess { get; }
    public double Result { get; }
    public string? ErrorMessage { get; }

    private MathResult(bool success, double value, string? error)
    {
        IsSuccess = success;
        Result = value;
        ErrorMessage = error;
    }

    public static MathResult Success(double value) => new(true, value, null);

    public static MathResult Failure(string error) => new(false, 0.0, error);
}
