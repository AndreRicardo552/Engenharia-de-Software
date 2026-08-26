using Calculator.Core;

namespace Calculator.Parser;

public readonly record struct ParseResult
{
    public bool IsSuccess { get; }
    public Command? Command { get; }
    public string? Error { get; }

    private ParseResult(bool isSuccess, Command value, string? error)
    {
        IsSuccess = isSuccess;
        Command = value;
        Error = error;
    }

    public static ParseResult Success(Command value) => new(true, value, null);

    public static ParseResult Failure(string? err) => new(false, null, err);
}
