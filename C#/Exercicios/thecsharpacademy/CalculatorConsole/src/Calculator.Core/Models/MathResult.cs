namespace Calculator.Core;

public readonly record struct OperationResult
{
    public bool IsSuccess { get; }
    public double Result { get; }
    public string? ErrorMessage { get; }

    private OperationResult(bool success, double value, string? error)
    {
        IsSuccess = success;
        Result = value;
        ErrorMessage = error;
    }

    public static OperationResult Success(double value) => new(true, value, null);

    public static OperationResult Failure(string error) => new(false, 0.0, error);
}
