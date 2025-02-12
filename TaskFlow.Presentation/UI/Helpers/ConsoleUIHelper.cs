using System.Text;

namespace TaskFlow.Presentation.UI.Helpers;

public static class ConsoleUIHelper
{
    public static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(
            @"
╔════════════════════════════════════════════════════════════╗
║                                                            ║
║     ████████╗ █████╗ ███████╗██╗  ██╗███████╗██╗      ██████╗ ██╗    ██╗    ║
║     ╚══██╔══╝██╔══██╗██╔════╝██║ ██╔╝██╔════╝██║     ██╔═══██╗██║    ██║    ║
║        ██║   ███████║███████╗█████╔╝ █████╗  ██║     ██║   ██║██║ █╗ ██║    ║
║        ██║   ██╔══██║╚════██║██╔═██╗ ██╔══╝  ██║     ██║   ██║██║███╗██║    ║
║        ██║   ██║  ██║███████║██║  ██╗██║     ███████╗╚██████╔╝╚███╔███╔╝    ║
║        ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝     ╚══════╝ ╚═════╝  ╚══╝╚══╝     ║
║                                                            ║
║                Project Management System                   ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝
"
        );
        Console.ResetColor();
        Thread.Sleep(1500); // Show welcome screen for 1.5 seconds
    }

    public static void ShowLoadingSpinner(string message, Action action)
    {
        Console.Write($"{message} ");
        var spinner = new[] { '|', '/', '-', '\\' };
        var spinnerPos = Console.CursorLeft;
        var spinnerThread = new Thread(() =>
        {
            int counter = 0;
            while (true)
            {
                Console.CursorLeft = spinnerPos;
                Console.Write(spinner[counter % spinner.Length]);
                Thread.Sleep(100);
                counter++;
            }
        });

        spinnerThread.Start();
        action.Invoke();
        spinnerThread.Interrupt();
        Console.WriteLine("\nDone!");
    }

    public static void DrawBox(string title)
    {
        int width = Console.WindowWidth - 4;
        string horizontalLine = new string('═', width);

        Console.WriteLine($"╔{horizontalLine}╗");
        Console.WriteLine($"║{title.PadRight(width)}║");
        Console.WriteLine($"╚{horizontalLine}╝");
    }

    public static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        DrawBox($" Error: {message} ");
        Console.ResetColor();
        PressAnyKey();
    }

    public static void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        DrawBox($" Success: {message} ");
        Console.ResetColor();
        PressAnyKey();
    }

    public static void DisplayInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        DrawBox($" Info: {message} ");
        Console.ResetColor();
    }

    public static void PressAnyKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

    public static string GetInput(string prompt, ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.ForegroundColor = color;
        Console.Write($"{prompt}: ");
        Console.ResetColor();
        return Console.ReadLine() ?? string.Empty;
    }

    public static void ClearCurrentLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}
