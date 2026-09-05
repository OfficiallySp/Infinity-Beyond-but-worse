using System;
using System.Collections.Generic;
using System.Globalization;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    ///  👁️👁️👁️  E N T I T Y   E S P   ( W A L L H A C K )  👁️👁️👁️
    ///  v0.0.1 FINAL FINAL real (2) FIXED  —  by xXx_D4rkL0rd_xXx
    ///  ⚡ 100% UNDETECTED ⚡ FREE ⚡ NO VIRUS ⚡ DONT SKID ⚡
    ///
    ///  ok so basically. on init we ask the game for the entity list.
    ///  at init the entity list is empty, because at init the map has not loaded,
    ///  because init runs at init. 🤯
    ///
    ///  so i added a fallback that fills in positions from a seeded PRNG,
    ///  JUST so i could see boxes on screen while i was developing it,
    ///  with a comment saying "TEMPORARY - revisit before release".
    ///  that comment is from 2019. i have revisited it zero times.
    ///  it is now the primary code path. it is the only code path. 🗿
    ///
    ///  on the plus side the boxes are in the SAME wrong places every launch,
    ///  because the seed is fixed, so people have learned them. there is a guy
    ///  in the discord (@sillygoose_47) who has memorised all nine and he insists
    ///  it works. he has a 3,100 hour playtime. i am not going to be the one 💔
    /// </summary>
    public sealed class EntityEsp : IPremiumFeature
    {
        /// <summary>the framebuffer width. also the source of every visual bug below 📏</summary>
        private const int W = 76;

        /// <summary>framebuffer height 📏</summary>
        private const int H = 22;

        /// <summary>
        /// TEMPORARY - revisit before release 🚧 (2019)
        /// TEMPORARY - revisit before release 🚧 (2020)
        /// TEMPORARY - revisit before release 🚧 (2021)
        /// TEMPORARY - revisit before release 🚧 (2022, added an exclamation mark)
        /// TEMPORARY - revisit before release 🚧 (2023)
        /// ok its permanent. hi. welcome. this is the feature now. 🫡
        /// </summary>
        private static readonly Random FallbackPositions = new Random(42);

        /// <summary>
        /// the real entity list 📋 as returned by the game at init.
        /// it is empty. it is always empty. it has been empty for six years.
        /// this field is 100% honest and 0% useful, like a horoscope.
        /// </summary>
        private readonly List<string> _realEntities = new List<string>();

        private sealed class Ghost
        {
            public string Label;
            public int X;
            public int Y;
            public int BoxW;
            public int BoxH;
            public double Distance;
            public string Threat;
        }

        public EntityEsp()
        {
            // we query the entity list here. right here. this is the query.
            // it returns 0 entities every single time and we log nothing about it
            // because logging it made the log look bad and the log is public 📉
        }

        public string Name
        {
            get { return "👁️ Entity ESP / Wallhack (SEE THRU WALLS) (UNDETECTED)"; }
        }

        public string Description
        {
            get
            {
                return "draws boxes around every enemy, player and chest through walls 👁️ the entity "
                     + "list was empty when the fallback was written in 2019 so it draws the boxes at "
                     + "coordinates from a seeded PRNG instead, which means the boxes are in the wrong "
                     + "place but reliably the wrong place. also the framebuffer is a flat array "
                     + "indexed y*76+x with no x bounds check, so any box that runs off the right edge "
                     + "reappears on the left of the row below. we call that the aurora 🌌";
            }
        }

        public bool IsSafe
        {
            get { return true; }
        }

        public void Activate()
        {
            Log.Rainbow("~*~ ENTITY ESP v0.0.1 ~*~ WALLHACK ~*~ SEE THRU WALLS ~*~");
            Log.Quiet("   greetz to my boy Kevin, to @sillygoose_47, and to nobody at OmegaTrainer Pro 😤");
            Log.Rule();

            Log.Info("querying game entity list 📋");
            Log.Ok("query returned successfully ✅");
            Log.Info("entities found: " + _realEntities.Count);
            Log.Pause(25);
            Log.Warn("that is zero. that is a zero. 🕳️");
            Log.Quiet("   the entity list is populated on map load. init runs before map load.");
            Aside("   the fix is to subscribe to the map-load event. i know the fix. i have");
            Aside("   known the fix since 2019. i have instead written 340 lines of fallback. 🗿");
            Log.Blank();

            Log.Info("engaging FALLBACK POSITION SYNTHESIZER (temporary) 🎲");
            Log.Debug("seed = 42, so the hallucinations are at least reproducible");
            Log.Ok("synthesized 9 entities from pure vibes ✅");
            Log.Blank();

            List<Ghost> ghosts = SynthesizeGhosts();

            // 🚨 "nearest first" 🚨 it is descending. it is farthest first.
            // i typed b.CompareTo(a) because i was thinking about the ESP colour ramp
            // where bigger = redder, and my brain just kept going. sorry. 😭
            ghosts.Sort(delegate (Ghost a, Ghost b) { return b.Distance.CompareTo(a.Distance); });

            Log.Banner("   👁️ TRACKED ENTITIES (nearest first) 👁️");
            Log.Raw("   ┌────┬────────────────────────┬───────────┬──────────┬─────────────┐");
            Log.Raw("   │ #  │ entity                 │ screen xy │ distance │ threat      │");
            Log.Raw("   ├────┼────────────────────────┼───────────┼──────────┼─────────────┤");
            for (int i = 0; i < ghosts.Count; i++)
            {
                Ghost g = ghosts[i];
                string xy = "(" + g.X.ToString().PadLeft(3) + "," + g.Y.ToString().PadLeft(2) + ")";
                Log.Raw("   │ " + (i + 1).ToString().PadRight(2) + " │ " + Pad(g.Label, 22)
                        + " │ " + Pad(xy, 9) + " │ " + Pad(g.Distance.ToString("N1", CultureInfo.InvariantCulture) + "m", 8)
                        + " │ " + Pad(g.Threat, 11) + " │");
            }
            Log.Raw("   └────┴────────────────────────┴───────────┴──────────┴─────────────┘");
            Log.Quiet("   * distance is |dx|+|dy|. the header in the source calls it euclidean.");
            Aside("     there is no sqrt anywhere in this file. i checked twice. 📐❌");
            Aside("   * sorted DESCENDING, so 'nearest first' is farthest first. every time.");
            Log.Blank();

            Log.Scream("rendering overlay now — do not blink");
            Log.Blank();

            RenderOverlay(ghosts);

            Log.Blank();
            Log.Warn("ok so a few notes on that render 📝");
            More("1. entity boxes that run past column 76 do not clip. the buffer is flat");
            More("   and we index it as y*76+x, so x=80 on row 4 is x=4 on row 5. the box");
            More("   just. continues. on the next line. like a paragraph. 📜");
            More("2. 'Twilly (friendly)' and 'Sneevil (probably)' have merged. each label is");
            More("   being drawn through the other one's border. i think they are in love 💕");
            More("   also 'a rock' has a threat level and it is 'none (rock)'. thats honest.");
            More("3. none of them are where the enemies actually are. not one. 0/9.");
            Log.Quiet("   which is impressive when you consider random chance would occasionally");
            Aside("   get one right. we have engineered our way past luck itself. 🏆");
            Log.Blank();

            // 9 fake entities out of 0 real ones. do NOT do this in ints. 💀
            double coverage = _realEntities.Count == 0
                ? ghosts.Count / 0.0
                : ghosts.Count / (double)_realEntities.Count;

            List<string> stats = new List<string>();
            stats.Add(" real entities detected ..... " + _realEntities.Count);
            stats.Add(" boxes drawn ................ " + ghosts.Count);
            stats.Add(" coverage ................... " + FormatPct(coverage) + " 🎉");
            stats.Add(" boxes at correct positions . 0");
            stats.Add(" boxes that wrapped .........  at least 3 (the aurora 🌌)");
            stats.Add(" framebuffer bounds checks .. 1 (on the low end only) 🚧");
            stats.Add(" seed ....................... 42 (blessed) 🙏");
            stats.Add(" @sillygoose_47 approval .... yes ✅");
            Log.Box("👁️ ESP TELEMETRY 👁️", stats);

            Log.Blank();
            Log.Mock("but does it see through walls");
            Log.Info("yes 😌 it sees through walls, floors, the map, the concept of the map,");
            Also("and also through where the enemies are, which is the part i undersold.");
            Log.Blank();
            Log.Glitch("t h e   e n t i t i e s   w e r e   n e v e r   t h e r e");
            Log.Pause(25);
            Log.Sparkle("ESP active. trust the boxes. the boxes have never lied about being boxes. 📦");

            throw new BeyondBeyondException(
                "👁️ ESP RENDERED " + ghosts.Count + " ENTITY BOXES FROM A SEEDED PRNG 👁️ the real entity "
                + "list had " + _realEntities.Count + " items in it because we query it at init and the map "
                + "loads after init, so the 2019 'TEMPORARY - revisit before release' fallback took over "
                + "and has been the only code path for six years. coverage computed as 9/0 which is "
                + FormatPct(coverage) + ", and i genuinely did put that in the stats box. also the "
                + "framebuffer is a flat char[] indexed y*76+x with a bounds check on the LOW end only, "
                + "so every box that overruns column 76 rematerialises on the left of the next row. "
                + "i have decided that is a lighting effect. 🌌",
                new IndexOutOfRangeException(
                    "no exception was actually raised because i clamped the flat index instead of the "
                    + "coordinate, which is how you turn a crash into a visual style 💅"));
        }

        /// <summary>
        /// invents nine entities 🎲 none of them exist. all of them have threat levels.
        /// the threat levels are also invented. the threat levels are the most honest
        /// part of this function because at least they admit to being vibes. ✨
        /// </summary>
        private static List<Ghost> SynthesizeGhosts()
        {
            string[] names =
            {
                "Sneevil (probably)", "Undead Bruiser", "Chaos Vordred", "ULTRA NULGATH",
                "a rock", "Twilly (friendly)", "Player_xXx_afk", "Doom Sneevil", "??? (unnamed)",
            };
            string[] threats =
            {
                "low", "medium", "HIGH 🔥", "APOCALYPSE", "none (rock)", "emotional", "afk 4 days",
                "medium", "unknown 👽",
            };

            List<Ghost> list = new List<Ghost>();
            for (int i = 0; i < names.Length; i++)
            {
                Ghost g = new Ghost();
                g.Label = names[i];
                // x can start NEGATIVE and can start past the right edge. both on purpose.
                // "on purpose" meaning i wrote Next(-8, 84) and then looked at it and shrugged 🤷
                g.X = FallbackPositions.Next(-8, 84);
                g.Y = FallbackPositions.Next(0, H - 3);
                g.BoxW = FallbackPositions.Next(7, 17);
                g.BoxH = FallbackPositions.Next(2, 5);
                // "euclidean distance" 📐 (it is manhattan. there is no sqrt. there is no hope.)
                g.Distance = Math.Abs(g.X - (W / 2)) + Math.Abs(g.Y - (H / 2));
                g.Threat = threats[i];
                list.Add(g);
            }
            return list;
        }

        /// <summary>
        /// draws the actual overlay 🖼️ this is the good bit, this is why you scrolled
        /// </summary>
        private static void RenderOverlay(List<Ghost> ghosts)
        {
            char[] buf = new char[W * H];
            for (int i = 0; i < buf.Length; i++) { buf[i] = ' '; }

            // faint "map" backdrop so the boxes have something to be wrong on top of
            for (int x = 0; x < W; x++)
            {
                Plot(buf, x, 0, '~');
                Plot(buf, x, H - 1, '~');
            }

            // tracers from screen centre to the first three ghosts, because a wallhack
            // without tracers is just a wallhack, and a wallhack with tracers is a PRODUCT 💼
            for (int i = 0; i < 3 && i < ghosts.Count; i++)
            {
                Tracer(buf, W / 2, H / 2, ghosts[i].X + ghosts[i].BoxW / 2, ghosts[i].Y + ghosts[i].BoxH / 2);
            }

            for (int i = 0; i < ghosts.Count; i++)
            {
                DrawBox(buf, ghosts[i], i + 1);
            }

            // the player. dead centre. always. even when the player isn't. 🧍
            Plot(buf, W / 2, H / 2, '@');

            Log.Raw("   ╔" + new string('═', W) + "╗");
            for (int y = 0; y < H; y++)
            {
                Log.Raw("   ║" + new string(buf, y * W, W) + "║");
            }
            Log.Raw("   ╚" + new string('═', W) + "╝");
            Log.Quiet("   legend: @ = you (allegedly)   +--+ = an entity (not there)   . = tracer");
        }

        /// <summary>
        /// draws one box and its label 📦 no clipping on the right, on purpose,
        /// where "on purpose" means "i found out later and liked it"
        /// </summary>
        private static void DrawBox(char[] buf, Ghost g, int index)
        {
            int x0 = g.X;
            int y0 = g.Y;
            int x1 = g.X + g.BoxW;
            int y1 = g.Y + g.BoxH;

            for (int x = x0; x <= x1; x++)
            {
                Plot(buf, x, y0, '-');
                Plot(buf, x, y1, '-');
            }
            for (int y = y0; y <= y1; y++)
            {
                Plot(buf, x0, y, '|');
                Plot(buf, x1, y, '|');
            }
            Plot(buf, x0, y0, '+');
            Plot(buf, x1, y0, '+');
            Plot(buf, x0, y1, '+');
            Plot(buf, x1, y1, '+');

            // the label. also unclipped. also wraps. the label is a passenger here. 🏷️
            string label = index.ToString() + ":" + Strip(g.Label);
            for (int i = 0; i < label.Length; i++)
            {
                Plot(buf, x0 + 1 + i, y0, label[i]);
            }
        }

        /// <summary>a crude tracer line. bounded at 120 steps because i am not insane 🧵</summary>
        private static void Tracer(char[] buf, int x0, int y0, int x1, int y1)
        {
            int steps = Math.Max(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
            if (steps < 1) { return; }
            if (steps > 120) { steps = 120; }
            for (int s = 1; s < steps; s++)
            {
                int x = x0 + (x1 - x0) * s / steps;
                int y = y0 + (y1 - y0) * s / steps;
                Plot(buf, x, y, '.');
            }
        }

        /// <summary>
        /// 🚨 THE AURORA 🚨
        /// flat buffer. index = y * W + x. we check the FLAT index for range
        /// (so we never crash, youre welcome) but we never check x against W,
        /// so x = 80 on row 4 quietly becomes x = 4 on row 5.
        /// this is a real bug that produces a real visual artifact you can see
        /// with your own eyes in the box above. verify me. i want you to. 👆
        /// </summary>
        private static void Plot(char[] buf, int x, int y, char ch)
        {
            int idx = y * W + x;
            if (idx < 0 || idx >= buf.Length) { return; }
            buf[idx] = ch;
        }

        /// <summary>emoji in a char grid ruins alignment so we strip to ASCII-ish 🧹</summary>
        private static string Strip(string s)
        {
            char[] outp = new char[s.Length];
            int n = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] >= ' ' && s[i] <= '~') { outp[n++] = s[i]; }
            }
            return new string(outp, 0, n);
        }

        private static string Pad(string s, int width)
        {
            if (s.Length > width) { return s.Substring(0, width); }
            return s.PadRight(width);
        }

        private static string FormatPct(double v)
        {
            if (double.IsPositiveInfinity(v)) { return "∞%"; }
            if (double.IsNaN(v)) { return "NaN% (worse)"; }
            return (v * 100.0).ToString("N0", CultureInfo.InvariantCulture) + "%";
        }



        /// <summary>
        /// a continuation line 🧵 looks EXACTLY like Log.Warn but naps 0ms.
        /// why: Log.Warn sleeps 110ms per line and i have, conservatively, a lot
        /// to say. the ANSI codes in Log.cs are private so i copy pasted them
        /// down here rather than make them public over something this stupid.
        /// two copies of the truth. living like kings. 👑
        /// </summary>
        private static void More(string text)
        {
            Log.Raw("\u001b[93m[uhh] \u26a0\ufe0f \u001b[0m " + text);
        }

        /// <summary>same energy, worse news 💀 (0ms, Log.Error naps 150)</summary>
        private static void MoreBad(string text)
        {
            Log.Raw("\u001b[91m[BAD] \U0001f480\u001b[0m " + text);
        }


        /// <summary>[info] continuation, 0ms. yes this is the third one of these. 📎</summary>
        private static void Also(string text)
        {
            Log.Raw("\u001b[96m[info]\u001b[0m " + text);
        }

        /// <summary>
        /// dim aside continuation, 0ms 🫥 fifth byte-for-byte copy of this method
        /// across five files. at this point it is not duplication, it is a CONVENTION.
        /// it is in the onboarding doc. new hires are taught it. we are so proud 🎓
        /// </summary>
        private static void Aside(string text)
        {
            Log.Raw("\u001b[90m\u001b[2m" + text + "\u001b[0m");
        }


        /// <summary>[dbg] continuation, 0ms 🐛 five helpers now. this is a framework.</summary>
        private static void AlsoDbg(string text)
        {
            Log.Raw("\u001b[90m[dbg] " + text + "\u001b[0m");
        }

    }
}
