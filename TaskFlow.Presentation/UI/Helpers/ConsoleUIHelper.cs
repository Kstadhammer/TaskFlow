using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        ShowLoadingSpinnerInternal(
                message,
                () =>
                {
                    action();
                    return Task.CompletedTask;
                }
            )
            .Wait();
    }

    public static async Task ShowLoadingSpinner(string message, Func<Task> action)
    {
        await ShowLoadingSpinnerInternal(message, action);
    }

    private static async Task ShowLoadingSpinnerInternal(string message, Func<Task> action)
    {
        Console.Write($"{message} ");
        var spinner = new[] { '|', '/', '-', '\\' };
        var spinnerPos = Console.CursorLeft;
        var cancellationTokenSource = new CancellationTokenSource();

        var spinnerTask = Task.Run(
            async () =>
            {
                int counter = 0;
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Console.CursorLeft = spinnerPos;
                    Console.Write(spinner[counter % spinner.Length]);
                    await Task.Delay(100, cancellationTokenSource.Token);
                    counter++;
                }
            },
            cancellationTokenSource.Token
        );

        try
        {
            await action();
        }
        finally
        {
            cancellationTokenSource.Cancel();
            try
            {
                await spinnerTask;
            }
            catch (OperationCanceledException)
            {
                // Expected when we cancel the spinner
            }
            Console.WriteLine("\nDone!");
        }
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
