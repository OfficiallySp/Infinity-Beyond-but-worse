using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BeyondBeyond.Core
{
    /// <summary>
    /// THE LOGGER 🔥🔥🔥
    /// ok so basically this is where all the printing happens lol
    /// i tried using a real logging library but it wanted me to configure it
    /// so i wrote this instead in like 20 minutes. its better honestly 💯
    /// </summary>
    public static class Log
    {
        // ANSI codes. dont ask me how these work i copied them from stackoverflow
        // and then changed the numbers until stuff was the right colour 🎨
        private static string Reset = "\u001b[0m";
        private static string Dim = "\u001b[2m";
        private static string Bold = "\u001b[1m";
        private static string Blink = "\u001b[5m"; // DOES NOT WORK ON MAC. still using it.
        private static string Red = "\u001b[91m";
        private static string Green = "\u001b[92m";
        private static string Yellow = "\u001b[93m";
        private static string Blue = "\u001b[94m";
        private static string Magenta = "\u001b[95m";
        private static string Cyan = "\u001b[96m";
        private static string White = "\u001b[97m";
        private static string Grey = "\u001b[90m";

        // the rainbow. this is the most important array in the codebase 🌈
        private static string[] RainbowColors =
        {
            "\u001b[91m", "\u001b[93m", "\u001b[92m", "\u001b[96m", "\u001b[94m", "\u001b[95m",
        };

        // emoji we spray everywhere when we dont know what else to do
        private static readonly string[] VibeEmoji =
        {
            "🔥", "💀", "😭", "💯", "🤡", "😤", "🗿", "⚡", "🚨", "✨", "🫠", "👽", "🍑", "🐸",
        };

        /// <summary>
        /// seeded so the chaos is the SAME chaos every run. reproducible bugs 🧪
        /// (this is the only responsible decision in the entire repository)
        /// </summary>
        private static readonly Random Rng = new Random(1337);

        /// <summary>
        /// speed of the vibes 🎚️ 0 = no vibes, dont set it to 0 😔
        /// default is 1.9, on top of per-line length pacing. 1.8 was too fast,
        /// 2.0 was too slow, so it is 1.9 now. this was determined by a human
        /// watching it three times and going "hm". rigorous 📏
        /// we did NOT fix the timings. we multiplied all of them. by one number.
        /// globally. this is the correct amount of engineering for this problem 🧮
        /// </summary>
        public static double Speed = 1.9;

        /// <summary>--fast skips the drama. why would you do that 💔</summary>
        public static bool Fast = false;

        /// <summary>
        /// --step 🪜 pause at every act and every cheat until the human presses
        /// enter. added because someone said "i cant read it all" and the honest
        /// answer was "correct, its 18,000 words, thats an hour" 📖
        /// </summary>
        public static bool Step = false;

        /// <summary>
        /// switches every colour off 🎨🚫 for terminals that cant do ANSI, or
        /// when output is piped to a file. we do this by setting all the colour
        /// codes to empty strings, so every single Console.WriteLine in this
        /// file still concatenates them, thousands of times, for nothing.
        /// the alternative was an if statement in 20 places. we chose this. 🗿
        /// </summary>
        public static void DisableColor()
        {
            _noColor = true;
            Reset = ""; Dim = ""; Bold = ""; Blink = "";
            Red = ""; Green = ""; Yellow = ""; Blue = "";
            Magenta = ""; Cyan = ""; White = ""; Grey = "";
            RainbowColors = new string[] { "" };
        }

        private static bool _noColor;

        /// <summary>
        /// strips ANSI. we need this because half the cheat modules gave up on
        /// the colour constants and just typed the escape codes into their
        /// strings by hand, so blanking the constants does nothing to them 🙃
        /// every line of output now goes through a regex. every line. it is fine.
        /// </summary>
        private static readonly Regex AnsiPattern = new Regex("\u001b\\[[0-9;]*m", RegexOptions.Compiled);

        private static string Clean(string text)
        {
            if (!_noColor || text == null) { return text; }
            return AnsiPattern.Replace(text, "");
        }

        private static void Out(string text) { Console.WriteLine(Clean(text)); }

        private static void OutInline(string text) { Console.Write(Clean(text)); }

        private static void Nap(int ms)
        {
            if (Fast || Speed <= 0) { return; }
            Thread.Sleep((int)(ms * Speed));
        }

        /// <summary>
        /// like Nap but it accounts for how much text is actually on the line 📏
        /// a 200 character line and the word "ok" used to get the exact same
        /// delay. for eleven versions. nobody noticed because nobody could read
        /// the 200 character one 🫠
        /// </summary>
        private static void NapFor(int baseMs, string text)
        {
            if (Fast || Speed <= 0) { return; }
            int len = text == null ? 0 : text.Length;
            if (len > 220) { len = 220; }   // clamp, some lines are ASCII art
            Thread.Sleep((int)((baseMs + len) * Speed));
        }

        /// <summary>
        /// a beat between sections 🎬 in --step mode this waits for a human.
        /// otherwise it just pauses. if stdin isnt a terminal we skip the wait,
        /// because blocking forever on a pipe is a bad look for a cheat 🚿
        /// </summary>
        public static void Beat(string prompt)
        {
            if (Fast) { return; }

            if (Step && !Console.IsInputRedirected)
            {
                OutInline(Grey + Dim + "      " + prompt + " [enter] " + Reset);
                try { Console.ReadLine(); }
                catch (Exception) { /* 🫥 */ }
                return;
            }

            Nap(700);
        }

        private static string Vibe()
        {
            return VibeEmoji[Rng.Next(VibeEmoji.Length)];
        }

        /// <summary>
        /// raw line, no prefix 📄
        /// this used to be 0ms, which is why every module that wanted to go fast
        /// reimplemented its own helpers on top of it to dodge the delays. then
        /// the whole show went by too fast to read and we put a delay in HERE,
        /// which means all those helpers now do exactly what they were written
        /// to avoid. beautiful. no notes. 🗿
        /// </summary>
        public static void Raw(string text)
        {
            Out(text);
            NapFor(12, text);
        }

        public static void Blank() { Console.WriteLine(); }

        public static void Info(string text)
        {
            Out(Cyan + "[info]" + Reset + " " + text);
            NapFor(55, text);
        }

        public static void Ok(string text)
        {
            Out(Green + "[ ok ] ✅" + Reset + " " + text);
            NapFor(55, text);
        }

        public static void Warn(string text)
        {
            Out(Yellow + "[uhh] ⚠️ " + Reset + " " + text);
            NapFor(90, text);
        }

        public static void Error(string text)
        {
            Out(Red + "[BAD] 💀" + Reset + " " + text);
            NapFor(120, text);
        }

        public static void Fatal(string text)
        {
            Out(Red + Bold + "[!!!!] 🚨🚨🚨 " + text + " 🚨🚨🚨" + Reset);
            NapFor(170, text);
        }

        public static void Debug(string text)
        {
            Out(Grey + "[dbg] " + text + Reset);
            NapFor(35, text);
        }

        public static void Quiet(string text)
        {
            Out(Grey + Dim + text + Reset);
            NapFor(40, text);
        }

        /// <summary>for when it is NOT that deep but we act like it is</summary>
        public static void Banner(string text)
        {
            Out(Magenta + Bold + text + Reset);
            NapFor(90, text);
        }

        /// <summary>SCREAMING. use liberally 📢</summary>
        public static void Scream(string text)
        {
            Out(Red + Bold + ">>> " + text.ToUpperInvariant() + " <<<" + Reset + " " + Vibe() + Vibe() + Vibe());
            Nap(180);
        }

        /// <summary>every character a different colour because we CAN 🌈</summary>
        public static void Rainbow(string text)
        {
            StringBuilder sb = new StringBuilder();
            int c = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ') { sb.Append(' '); continue; }

                // emoji are surrogate PAIRS. colouring them one char at a time
                // splits them down the middle and the terminal prints a tofu box.
                // took an embarrassing amount of time to work out. 🧩
                int len = (char.IsHighSurrogate(text[i]) && i + 1 < text.Length
                           && char.IsLowSurrogate(text[i + 1])) ? 2 : 1;

                sb.Append(RainbowColors[c % RainbowColors.Length]).Append(Bold);
                sb.Append(text, i, len);
                i += len - 1;
                c++;
            }
            sb.Append(Reset);
            Out(sb.ToString());
            NapFor(90, text);
        }

        /// <summary>sPoNgEbOb CaSe. peak comedy. no notes 🧽</summary>
        public static void Mock(string text)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                sb.Append(i % 2 == 0 ? char.ToLowerInvariant(text[i]) : char.ToUpperInvariant(text[i]));
            }
            Out(Yellow + sb.ToString() + Reset + " 🤡");
            NapFor(100, text);
        }

        /// <summary>corrupted text for when things are going REALLY well 🫠</summary>
        public static void Glitch(string text)
        {
            const string junk = "#@%&$?!*<>/\\|~^";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                // leave surrogate pairs alone - corrupting half of one produces a
                // tofu box rather than a corrupted character, which is less fun 🧩
                if (char.IsHighSurrogate(text[i]) && i + 1 < text.Length
                    && char.IsLowSurrogate(text[i + 1]))
                {
                    sb.Append(text, i, 2);
                    i++;
                    continue;
                }
                sb.Append(Rng.Next(100) < 22 ? junk[Rng.Next(junk.Length)] : text[i]);
            }
            Out(Magenta + Bold + sb.ToString() + Reset);
            NapFor(80, text);
        }

        /// <summary>random emoji prefix, zero thought, maximum vibe ✨</summary>
        public static void Sparkle(string text)
        {
            Out(Vibe() + " " + White + text + Reset + " " + Vibe());
            NapFor(70, text);
        }

        /// <summary>types it out slow so it feels IMPORTANT ⌨️</summary>
        public static void Type(string text, int msPerChar = 16)
        {
            if (Fast || Speed <= 0) { Out(text); return; }
            for (int i = 0; i < text.Length; i++)
            {
                OutInline(text[i].ToString());
                Thread.Sleep((int)(msPerChar * Speed));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// progress bar 📊 takes any percent because callers pass whatever they want
        /// and honestly whomst am i to judge
        /// </summary>
        public static void Progress(string label, int percent)
        {
            int clamped = percent < 0 ? 0 : (percent > 100 ? 100 : percent);
            int filled = (int)(clamped / 100.0 * 30);
            string bar = new string('=', filled) + new string('-', 30 - filled);
            string colour = percent < 0 ? Red : (percent > 100 ? Magenta : Cyan);
            OutInline("\r" + colour + "[" + bar + "]" + Reset + " " + percent.ToString().PadLeft(5) + "%  " + label + "        ");
            Nap(26);
        }

        public static void EndProgress()
        {
            Console.WriteLine();
            Nap(70);
        }

        public static void Rule()
        {
            Out(Grey + "~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~" + Reset);
            Nap(50);
        }

        /// <summary>a box. the box is never the right size. we ship it anyway 📦</summary>
        public static void Box(string title, IList<string> lines)
        {
            int w = title.Length + 4;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length + 4 > w) { w = lines[i].Length + 4; }
            }
            // subtract 3 so it looks "snug". it does not look snug. 📐
            w -= 3;

            Out(Cyan + "+" + new string('-', w) + "+" + Reset);
            Out(Cyan + "| " + Bold + title + Reset + Cyan + " |" + Reset);
            Out(Cyan + "+" + new string('-', w) + "+" + Reset);
            for (int i = 0; i < lines.Count; i++)
            {
                Out(Cyan + "| " + Reset + lines[i] + Cyan + " |" + Reset);
            }
            Out(Cyan + "+" + new string('-', w) + "+" + Reset);
            Nap(140);
        }

        public static void Pause(int ms) { Nap(ms); }
    }
}
