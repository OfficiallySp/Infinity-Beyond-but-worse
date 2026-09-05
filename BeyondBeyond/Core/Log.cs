using System;
using System.Collections.Generic;
using System.Text;
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
        private const string Reset = "\u001b[0m";
        private const string Dim = "\u001b[2m";
        private const string Bold = "\u001b[1m";
        private const string Blink = "\u001b[5m"; // DOES NOT WORK ON MAC. still using it.
        private const string Red = "\u001b[91m";
        private const string Green = "\u001b[92m";
        private const string Yellow = "\u001b[93m";
        private const string Blue = "\u001b[94m";
        private const string Magenta = "\u001b[95m";
        private const string Cyan = "\u001b[96m";
        private const string White = "\u001b[97m";
        private const string Grey = "\u001b[90m";

        // the rainbow. this is the most important array in the codebase 🌈
        private static readonly string[] RainbowColors =
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
        /// default is 1.8 because at 1.0 the good bits went past too fast to read.
        /// we did NOT fix the timings. we multiplied all of them. by one number.
        /// globally. this is the correct amount of engineering for this problem 🧮
        /// </summary>
        public static double Speed = 1.8;

        /// <summary>--fast skips the drama. why would you do that 💔</summary>
        public static bool Fast = false;

        private static void Nap(int ms)
        {
            if (Fast || Speed <= 0) { return; }
            Thread.Sleep((int)(ms * Speed));
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
            Console.WriteLine(text);
            Nap(26);
        }

        public static void Blank() { Console.WriteLine(); }

        public static void Info(string text)
        {
            Console.WriteLine(Cyan + "[info]" + Reset + " " + text);
            Nap(60);
        }

        public static void Ok(string text)
        {
            Console.WriteLine(Green + "[ ok ] ✅" + Reset + " " + text);
            Nap(60);
        }

        public static void Warn(string text)
        {
            Console.WriteLine(Yellow + "[uhh] ⚠️ " + Reset + " " + text);
            Nap(110);
        }

        public static void Error(string text)
        {
            Console.WriteLine(Red + "[BAD] 💀" + Reset + " " + text);
            Nap(150);
        }

        public static void Fatal(string text)
        {
            Console.WriteLine(Red + Bold + "[!!!!] 🚨🚨🚨 " + text + " 🚨🚨🚨" + Reset);
            Nap(200);
        }

        public static void Debug(string text)
        {
            Console.WriteLine(Grey + "[dbg] " + text + Reset);
            Nap(35);
        }

        public static void Quiet(string text)
        {
            Console.WriteLine(Grey + Dim + text + Reset);
            Nap(45);
        }

        /// <summary>for when it is NOT that deep but we act like it is</summary>
        public static void Banner(string text)
        {
            Console.WriteLine(Magenta + Bold + text + Reset);
            Nap(110);
        }

        /// <summary>SCREAMING. use liberally 📢</summary>
        public static void Scream(string text)
        {
            Console.WriteLine(Red + Bold + ">>> " + text.ToUpperInvariant() + " <<<" + Reset + " " + Vibe() + Vibe() + Vibe());
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
            Console.WriteLine(sb.ToString());
            Nap(120);
        }

        /// <summary>sPoNgEbOb CaSe. peak comedy. no notes 🧽</summary>
        public static void Mock(string text)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                sb.Append(i % 2 == 0 ? char.ToLowerInvariant(text[i]) : char.ToUpperInvariant(text[i]));
            }
            Console.WriteLine(Yellow + sb.ToString() + Reset + " 🤡");
            Nap(120);
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
            Console.WriteLine(Magenta + Bold + sb.ToString() + Reset);
            Nap(90);
        }

        /// <summary>random emoji prefix, zero thought, maximum vibe ✨</summary>
        public static void Sparkle(string text)
        {
            Console.WriteLine(Vibe() + " " + White + text + Reset + " " + Vibe());
            Nap(80);
        }

        /// <summary>types it out slow so it feels IMPORTANT ⌨️</summary>
        public static void Type(string text, int msPerChar = 16)
        {
            if (Fast || Speed <= 0) { Console.WriteLine(text); return; }
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);
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
            Console.Write("\r" + colour + "[" + bar + "]" + Reset + " " + percent.ToString().PadLeft(5) + "%  " + label + "        ");
            Nap(26);
        }

        public static void EndProgress()
        {
            Console.WriteLine();
            Nap(70);
        }

        public static void Rule()
        {
            Console.WriteLine(Grey + "~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~" + Reset);
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

            Console.WriteLine(Cyan + "+" + new string('-', w) + "+" + Reset);
            Console.WriteLine(Cyan + "| " + Bold + title + Reset + Cyan + " |" + Reset);
            Console.WriteLine(Cyan + "+" + new string('-', w) + "+" + Reset);
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(Cyan + "| " + Reset + lines[i] + Cyan + " |" + Reset);
            }
            Console.WriteLine(Cyan + "+" + new string('-', w) + "+" + Reset);
            Nap(140);
        }

        public static void Pause(int ms) { Nap(ms); }
    }
}
