using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ConsoleDrawer2
{
    internal class Program
    {
        static void MainMenu()
        {
            //╔═║╗╚╝┌┐└┘│─

            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey(true);
            } while (info.Key != ConsoleKey.Escape);
        }
        enum MainMenuOptions
        {
            Create, Update, Delete, Exit
        }
        static string[] GetButton(string label, bool highlighted = true)
        {

            var op = new string[3];
            if (highlighted)
            {
                //▖▘▗▝▄▀ ▌▐
                op[0] = "▗".PadRight(label.Length + 1, '▄') + '▖';
                op[1] = $"▐{label}▌";
                op[2] = "▝".PadRight(label.Length + 1, '▀') + '▝';
            }
            else
            {
                op[0] = "┌".PadRight(label.Length + 1, '─') + '┐';
                op[1] = $"│{label}│";
                op[2] = "└".PadRight(label.Length + 1, '─') + '┘';
            }
            return op;
        }
        static void DrawMainMenu()
        {
            var horizontalLine = "".PadRight(Console.WindowWidth - 2, '═');
            var bodyLine = "║".PadRight(Console.WindowWidth - 1, ' ') + '║';
            Console.WriteLine($"╔{horizontalLine}╗");
            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.WriteLine(bodyLine);
            }
            Console.Write($"╚{horizontalLine}╝");

            Console.SetCursorPosition(10, 10);

            var optionLabels = new[] { "Létrehozás", "Szerkesztés", "Törlés", "Kilépés" };
            //┌┐└┘│─
            //▖▘▗▝
            var button = new string[3];
            for (int i = 0; i < optionLabels.Length; i++)
            {
                var label = optionLabels[i];
                foreach (var line in GetButton(label))
                {
                    Console.CursorLeft = 10;
                    Console.WriteLine(line);
                }
            }
        }
        static void Main(string[] args)
        {
            ConsoleExtension.SetCodePage(65001u);
            ConsoleExtension.SetConsoleFont("Consolas", 16);

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.CursorVisible = false;
                    DrawMainMenu();
                    return;
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
