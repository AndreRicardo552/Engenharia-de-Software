namespace Calculator.Core;

public readonly record struct Command(double Num1, double Num2, Operation Op);
