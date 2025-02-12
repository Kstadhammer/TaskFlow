namespace TaskFlow.Presentation.UI.Helpers;

public static class MenuHelper
{
    private static int _selectedIndex;

    public static string ShowMenu(string title, Dictionary<string, string> options)
    {
        _selectedIndex = 0;
        var optionsList = options.ToList();
        ConsoleKey key;

        do
        {
            Console.CursorVisible = false;
            ConsoleHelper.DisplayHeader(title);

            // Display all options
            for (int i = 0; i < optionsList.Count; i++)
            {
                if (i == _selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{optionsList[i].Key}. {optionsList[i].Value}");

                Console.ResetColor();
            }

            Console.WriteLine("\nUse ↑↓ arrows to navigate, Enter to select");

            var keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            // Handle arrow keys
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _selectedIndex = Math.Max(0, _selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    _selectedIndex = Math.Min(optionsList.Count - 1, _selectedIndex + 1);
                    break;
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        return optionsList[_selectedIndex].Key;
    }
}
