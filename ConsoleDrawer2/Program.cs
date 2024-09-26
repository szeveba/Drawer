using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleDrawer2
{

    internal class Program
    {
        private const int LF_FACESIZE = 32;
        private const int STD_OUTPUT_HANDLE = -11;
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X; // X koordináta
            internal short Y; // Y koordináta
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        static unsafe void SetConsoleFont(string fontName, short fontSize)
        {
            if (fontName.Length >= LF_FACESIZE)
            {
                throw new ArgumentException($"Font name too long. Maximum length is {LF_FACESIZE - 1} characters.");
            }

            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
            newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
            newInfo.dwFontSize = new COORD() { X = fontSize, Y = fontSize };
            newInfo.FontFamily = 54; // FF_DONTCARE (0x00) | FIXED_PITCH (0x01)
            newInfo.FontWeight = 400; // FW_NORMAL

            // Copy fontName to FaceName
            for (int i = 0; i < fontName.Length; i++)
            {
                newInfo.FaceName[i] = fontName[i];
            }
            newInfo.FaceName[fontName.Length] = '\0'; // Null-terminate the string

            if (!SetCurrentConsoleFontEx(hnd, false, ref newInfo))
            {
                Console.WriteLine("Nem sikerült beállítani a konzol betűtípusát.");
            }
        }

        static void MainMenu()
        {
            //╔═║╗╚╝┌┐└┘│─

            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey(true);
            } while (info.Key != ConsoleKey.Escape);
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleCP(uint wCodePageID);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleOutputCP(uint wCodePageID);

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

            SetConsoleCP(65001u);
            SetConsoleOutputCP(65001u);
            SetConsoleFont("Consolas", 16);

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
