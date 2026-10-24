namespace Calculator.Core;

public record CalculationRecord(
    int Id,
    double Num1,
    double? Num2,
    Operation Op,
    Double Result,
    DateTime ExecutedAt
);
