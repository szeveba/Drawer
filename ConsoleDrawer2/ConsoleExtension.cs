using System.Runtime.InteropServices;

namespace ConsoleDrawer2
{
    public static class ConsoleExtension
    {
        public static unsafe void SetConsoleFont(string fontName, short fontSize)
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
                throw new Exception($"Font name too long. Maximum length is {LF_FACESIZE - 1} characters.");
            }
        }
        public static bool SetCodePage(uint codePageID)
        {
            return SetConsoleCP(codePageID) && SetConsoleOutputCP(codePageID);
        }
        private const int LF_FACESIZE = 32;
        private const int STD_OUTPUT_HANDLE = -11;
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            internal short X;
            internal short Y;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);
    }
}
