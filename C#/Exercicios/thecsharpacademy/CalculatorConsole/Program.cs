using System.Text.RegularExpressions;

namespace CalculatorConsole;

class Program
{
    static void Main()
    {
        bool endApp = false;
        int count = 0;
        Calculator calculator = new Calculator();
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        while (!endApp)
        {
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput1;
            string? numInput2;
            double result;
            string? menu;

            do
            {
                Console.WriteLine("Type a option");
                Console.WriteLine("\tc - Calculator");
                Console.WriteLine("\th - History");
                Console.WriteLine($"Since the last start calculator was used: {count}");
                menu = Console.ReadLine();
            } while (menu == null || !Regex.IsMatch(menu, "^(c|h)$"));

            switch (menu)
            {
                case "c":
                {
                    // Ask the user to choose an operator.
                    Console.WriteLine("Choose an operator from the following list:");
                    Console.WriteLine("\ta - Add");
                    Console.WriteLine("\ts - Subtract");
                    Console.WriteLine("\tm - Multiply");
                    Console.WriteLine("\td - Divide");
                    Console.Write("Your option? ");

                    string? op = Console.ReadLine();

                    // Validate input is not null, and matches the pattern
                    if (op == null || !Regex.IsMatch(op, "^(a|s|m|d)$"))
                    {
                        Console.WriteLine("Error: Unrecognized input.");
                    }
                    else
                    {
                        try
                        {
                            result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                            if (double.IsNaN(result))
                            {
                                Console.WriteLine(
                                    "This operation will result in a mathematical error.\n"
                                );
                            }
                            else
                            {
                                Console.WriteLine("Your result: {0:0.##}\n", result);
                                count++;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(
                                "Oh no! An exception occurred trying to do the math.\n - Details: "
                                    + e.Message
                            );
                        }
                    }
                    break;
                }
                case "h":
                {
                    break;
                }
                default:
                {
                    Console.WriteLine("Not a valid option!");
                    break;
                }
            }
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write(
                "Press 'n' and Enter to close the app, or press any other key and Enter to continue: "
            );
            if (Console.ReadLine() == "n")
                endApp = true;
            Console.WriteLine("\n"); // Friendly linespacing.
        }
        return;
    }
}
