namespace ConsoleDrawer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //█▓▒░
            char symbol = '█';
            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey();
                switch (info.Key)
                {
                    case ConsoleKey.UpArrow: if (0 < Console.CursorTop) Console.CursorTop -= 1; break;
                    case ConsoleKey.DownArrow: if (Console.CursorTop < Console.WindowHeight - 1) Console.CursorTop += 1; break;
                    case ConsoleKey.LeftArrow: if (0 < Console.CursorLeft) Console.CursorLeft -= 1; break;
                    case ConsoleKey.RightArrow: if (Console.CursorLeft < Console.WindowWidth - 1) Console.CursorLeft += 1; break;
                    case ConsoleKey.F1: symbol = '█'; break;
                    case ConsoleKey.F2: symbol = '▓'; break;
                    case ConsoleKey.F3: symbol = '▒'; break;
                    case ConsoleKey.F4: symbol = '░'; break;
                    case ConsoleKey.Spacebar:
                        var cursorPosition = Console.GetCursorPosition();
                        Console.Write(symbol);
                        Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
                        break;
                }
            } while (info.Key != ConsoleKey.Escape);
        }
    }
}
