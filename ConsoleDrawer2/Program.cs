using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleDrawer2
{
    class Menu
    {
        public Menu(IEnumerable<string> optionLabels)
        {
            Options = optionLabels.Select(x => new MenuOption(x)).ToArray();
            Options[SelectedOptionIndex].IsSelected = true;
        }
        public int SelectedOptionIndex { get; set; }
        public MenuOption[] Options { get; }
        private void DrawEdges()
        {
            Console.SetCursorPosition(0, 0);
            var horizontalLine = new StringBuilder().Append('═', Console.WindowWidth - 2).ToString();
            var bodyLine = new StringBuilder().Append('║').Append(' ', Console.WindowWidth - 2).Append('║').ToString();
            Console.WriteLine($"╔{horizontalLine}╗");
            for (int i = 0; i < Console.WindowHeight - 2; i++)
            {
                Console.WriteLine(bodyLine);
            }
            Console.Write($"╚{horizontalLine}╝");
        }
        private void DrawOptions()
        {
            foreach (var option in Options)
            {
                option.Draw();
            }
        }
        public void Init()
        {
            int leftCenterIdx = Console.WindowWidth / 2;
            int maxContentWidth = 18;
            int leftOptionStartPos = leftCenterIdx - maxContentWidth / 2;
            int contentHeight = Options.Length * 3 + Options.Length - 1;
            int topCenterIdx = Console.WindowHeight / 2;
            int topOptionStartPos = topCenterIdx - contentHeight / 2;
            Console.CursorTop = topOptionStartPos;
            DrawEdges();
            for (int i = 0; i < Options.Length; i++)
            {
                Options[i].Position = (topOptionStartPos + i * 4, leftOptionStartPos);
                Options[i].InitTextures();
            }
            DrawOptions();
        }

        internal void MoveDown()
        {
            ChangeSelection(true);
        }

        internal void MoveUp()
        {
            ChangeSelection(false);
        }
        private void ChangeSelection(bool isDownMove)
        {
            Options[SelectedOptionIndex].IsSelected = false;
            Options[SelectedOptionIndex].Draw();
            if (isDownMove) 
                SelectedOptionIndex = SelectedOptionIndex == Options.Length - 1 ? 0 : SelectedOptionIndex + 1;
            else 
                SelectedOptionIndex = SelectedOptionIndex == 0 ? Options.Length - 1 : SelectedOptionIndex - 1;
            Options[SelectedOptionIndex].IsSelected = true;
            Options[SelectedOptionIndex].Draw();
        }
    }
    class MenuOption
    {
        public static int BaseWidth { get; set; } = 20;
        public (int top, int left) Position { get; set; }
        public MenuOption(string label)
        {
            Label = label;
        }

        public string Label { get; }
        public bool IsSelected { get; set; }
        private void SwitchConsoleColors()
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = temp;
        }
        public void Draw()
        {
            var texture = IsSelected ? HighlightedTexture : NormalTexture;
            Console.SetCursorPosition(Position.left, Position.top);
            Console.WriteLine(texture[0]);
            if (IsSelected) SwitchConsoleColors();
            Console.CursorLeft = Position.left;
            Console.WriteLine(texture[1]);
            if (IsSelected) SwitchConsoleColors();
            Console.CursorLeft = Position.left;
            Console.WriteLine(texture[2]);
        }
        public string[] HighlightedTexture { get; private set; }
        public string[] NormalTexture { get; private set; }
        public void InitTextures(bool highlighted = false)
        {
            int filler = BaseWidth - Label.Length;
            int filler1 = filler / 2;
            int filler2 = filler - filler1;
            if (filler1 < filler2)
            {
                filler1++;
                filler2--;
            }
            NormalTexture = new string[3];
            HighlightedTexture = new string[3];
            HighlightedTexture[0] = new StringBuilder().Append('▄', BaseWidth + 2).ToString();
            HighlightedTexture[1] = new StringBuilder().Append(' ', filler1 + 1).Append(Label).Append(' ', filler2 + 1).ToString();
            HighlightedTexture[2] = new StringBuilder().Append('▀', BaseWidth + 2).ToString();
            NormalTexture[0] = new StringBuilder().Append('┌').Append('─', BaseWidth).Append('┐').ToString();
            NormalTexture[1] = new StringBuilder().Append('│').Append(' ', filler1).Append(Label).Append(' ', filler2).Append('│').ToString();
            NormalTexture[2] = new StringBuilder().Append('└').Append('─', BaseWidth).Append('┘').ToString();
        }
    }
    internal class Program
    {
        static void DrawMainMenu(int selectedIdx = 2)
        {
            DrawEdges();
            //DrawOptions(selectedIdx);
        }

        private static void DrawEdges()
        {
            var horizontalLine = "".PadRight(Console.WindowWidth - 2, '═');
            var bodyLine = "║".PadRight(Console.WindowWidth - 1, ' ') + '║';
            Console.WriteLine($"╔{horizontalLine}╗");
            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.WriteLine(bodyLine);
            }
            Console.Write($"╚{horizontalLine}╝");
        }
        static void Main(string[] args)
        {
            //ConsoleExtension.SetCodePage(65001u);
            //ConsoleExtension.SetConsoleFont("Yu Gothic UI", 16);
            Console.Title = "Console Drawer 2";
            Console.CursorVisible = false;
            ConsoleKeyInfo info;
            var mainMenuOptions = new string[] { "Létrehozás", "Szerkesztés", "Törlés", "Kilépés" };
            var mainMenu = new Menu(mainMenuOptions);
            mainMenu.Init();
            do
            {
                info = Console.ReadKey(true);
                switch (info.Key)
                {
                    case ConsoleKey.UpArrow:
                        mainMenu.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        mainMenu.MoveDown();
                        break;
                }
            } while (info.Key != ConsoleKey.Escape);
        }
    }
}
