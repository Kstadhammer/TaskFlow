namespace TaskFlow.Presentation.UI.Helpers;

public static class ConsoleHelper
{
    public static void DisplayHeader(string title)
    {
        Console.Clear();
        Console.WriteLine($"=== {title} ===\n");
    }

    public static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nError: {message}");
        Console.ResetColor();
        PressAnyKey();
    }

    public static void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nSuccess: {message}");
        Console.ResetColor();
        PressAnyKey();
    }

    public static void DisplayMenu(string title, Dictionary<string, string> options)
    {
        DisplayHeader(title);
        foreach (var option in options)
        {
            Console.WriteLine($"{option.Key}. {option.Value}");
        }
        Console.Write("\nSelect an option: ");
    }

    public static void PressAnyKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    public static string GetRequiredInput(string prompt)
    {
        string? input;
        do
        {
            Console.Write($"{prompt}: ");
            input = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    public static string? GetOptionalInput(string prompt, string currentValue)
    {
        Console.Write($"{prompt} ({currentValue}): ");
        var input = Console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(input) ? currentValue : input;
    }

    public static DateTime GetDate(string prompt)
    {
        DateTime date;
        do
        {
            Console.Write($"{prompt} (yyyy-MM-dd): ");
        } while (!DateTime.TryParse(Console.ReadLine(), out date));
        return date;
    }

    public static DateTime? GetOptionalDate(string prompt, DateTime currentValue)
    {
        Console.Write($"{prompt} ({currentValue:yyyy-MM-dd}): ");
        var input = Console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(input) ? currentValue
            : DateTime.TryParse(input, out DateTime date) ? date
            : currentValue;
    }

    public static decimal GetDecimal(string prompt)
    {
        decimal value;
        do
        {
            Console.Write($"{prompt}: ");
        } while (!decimal.TryParse(Console.ReadLine(), out value));
        return value;
    }

    public static int GetId(string prompt)
    {
        int id;
        do
        {
            Console.Write($"{prompt}: ");
        } while (!int.TryParse(Console.ReadLine(), out id));
        return id;
    }
}
