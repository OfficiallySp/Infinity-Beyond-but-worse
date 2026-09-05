using System;
using System.Runtime.InteropServices;

namespace BeyondBeyond.Core
{
    /// <summary>
    /// makes the colours work on windows 🪟
    ///
    /// on mac and linux the terminal just does ANSI. on windows the console
    /// CAN do ANSI but it is switched off by default, so without this you get
    /// several thousand lines of literal escape codes, which is somehow both
    /// less readable AND less funny 💀
    ///
    /// NOTE ON THE P/INVOKE BELOW 📌 this is the only DllImport in the entire
    /// project and it is three console-mode functions. it does not open, read,
    /// write or inject into any process. the injector in Injection/ is entirely
    /// simulated and touches nothing. this is here so text is coloured. thats it.
    /// </summary>
    public static class TerminalSetup
    {
        private const int StdOutputHandle = -11;
        private const uint EnableVirtualTerminalProcessing = 0x0004;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        /// <summary>
        /// turns on ANSI where it needs turning on 🎨
        /// returns true if colour should work. false means we gave up and the
        /// caller should switch colour off entirely rather than spray the
        /// terminal with escape codes.
        /// </summary>
        public static bool TryEnableAnsi()
        {
            // mac + linux: already fine, nothing to do 🐧🍎
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }

            // windows terminal sets this and already has VT on. checking it
            // first means we dont need the p/invoke at all on win11 ✅
            string wt = Environment.GetEnvironmentVariable("WT_SESSION");
            if (!string.IsNullOrEmpty(wt))
            {
                return true;
            }

            try
            {
                IntPtr handle = GetStdHandle(StdOutputHandle);
                if (handle == IntPtr.Zero || handle == new IntPtr(-1))
                {
                    return false;
                }

                uint mode;
                if (!GetConsoleMode(handle, out mode))
                {
                    // output is redirected to a file or a pipe. no console, no
                    // colour. this is the correct outcome and we take it well 📄
                    return false;
                }

                if ((mode & EnableVirtualTerminalProcessing) != 0)
                {
                    return true;
                }

                return SetConsoleMode(handle, mode | EnableVirtualTerminalProcessing);
            }
            catch (Exception)
            {
                // if kernel32 isnt there we have bigger problems than colours 🫥
                return false;
            }
        }

        /// <summary>
        /// makes emoji render instead of turning into "?" 🔤
        /// on windows the console is codepage 437 by default, which was designed
        /// in 1981 and does not have 🤡 in it. rude.
        /// </summary>
        public static void TryEnableUtf8()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch (Exception)
            {
                // some terminals refuse. the show goes on, just uglier 🫠
            }
        }
    }
}
