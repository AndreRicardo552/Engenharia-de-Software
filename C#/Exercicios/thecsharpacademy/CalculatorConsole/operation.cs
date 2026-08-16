using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace CalculatorConsole;

class Operation
{
    public double num1 { get; set; }
    public double num2 { get; set; }
    public string op { get; set; }
    public double result { get; set; }

    public void SetNumbers()
    { // Ask the user to type the first number.
        string numInput1,
            numInput2;

        Console.Write("Type a number, and then press Enter: ");
        numInput1 = Console.ReadLine();

        double cleanNum1;
        while (!double.TryParse(numInput1, out cleanNum1))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput1 = Console.ReadLine();
        }

        // Ask the user to type the second number.
        Console.Write("Type another number, and then press Enter: ");
        numInput2 = Console.ReadLine();

        double cleanNum2;
        while (!double.TryParse(numInput2, out cleanNum2))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput2 = Console.ReadLine();
        }

        num1 = cleanNum1;
        num2 = cleanNum2;
    }

    public void SetOperator() { }
}
