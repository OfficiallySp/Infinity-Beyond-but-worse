using System;
using System.Collections.Generic;
using System.Globalization;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    /// 🎁🎁🎁 SMART DROP FILTER (AI POWERED) 🎁🎁🎁
    /// it is not AI powered. there is no AI. there is one if statement and it is backwards.
    ///
    /// "smart" in the marketing sense. like a smart fridge. it has opinions and
    /// they are all wrong and you cannot turn it off. 🧊
    ///
    /// v0.0.1 FINAL FINAL real (2) FIXED — do NOT skid
    /// credits: xXx_D4rkL0rd_xXx (code), Kevin (rarity science), Braydon (quit)
    /// </summary>
    public sealed class DropFilter : IPremiumFeature
    {
        /// <summary>
        /// one item on the ground 🎒 hopes, dreams, and a rarity string
        /// </summary>
        private sealed class LootItem
        {
            public string Name;
            public string Rarity;
            public int Quantity;
            public int Value;

            public LootItem(string name, string rarity, int quantity, int value)
            {
                Name = name;
                Rarity = rarity;
                Quantity = quantity;
                Value = value;
            }
        }

        /// <summary>
        /// 🧪 THE REAL RARITY WEIGHTS 🧪
        /// these are correct. these are GOOD. i researched these. i asked people.
        /// i made a spreadsheet. the spreadsheet is beautiful.
        ///
        /// we do not use them. we sort by string.CompareOrdinal instead. 💀
        /// i want it on record that the correct data existed the entire time,
        /// sitting right here, four lines above the code that ignores it.
        /// </summary>
        private static readonly string[] RarityNames =
        {
            "Common", "Uncommon", "Rare", "Epic", "Legendary", "Mythic", "Godlike", "Trash",
        };

        /// <summary>higher = rarer. never read. not once. ticket BB-6102 (open, forever) 📉</summary>
        private static readonly int[] RarityWeights =
        {
            10, 25, 60, 120, 400, 1200, 9999, 1,
        };

        public DropFilter()
        {
            // there is no setup. there was never setup. Braydon added setup once
            // and it read a config file that does not exist, so it threw, so we
            // deleted the setup instead of the read. shipping is a mindset 🚀
        }

        public string Name
        {
            get { return "🎁 Smart Drop Filter (AI POWERED) (it is one if statement)"; }
        }

        public string Description
        {
            get
            {
                return "automatically keeps the good drops and deletes the junk 🗑️ the keep-check is "
                     + "inverted (we flipped it to debug something in march and shipped it flipped) "
                     + "AND rarity is ranked by alphabetical string order, so Common outranks "
                     + "Legendary because C comes before L. it is extremely consistent. it is "
                     + "consistently the worst possible answer 💯";
            }
        }

        public bool IsSafe
        {
            get { return false; } // finally an honest one. nothing reads it. 🦺😔
        }

        public void Activate()
        {
            Log.Rainbow("~*~ SMART DROP FILTER v0.0.1 ~*~ AI POWERED ~*~ NO VIRUS ~*~");
            Log.Rule();

            Log.Info("loading rarity ranking table 📊");
            Log.Debug("comparator: string.CompareOrdinal(a.Rarity, b.Rarity)");
            AlsoDbg("(i meant to write a lookup. i wrote this. it compiled first try");
            AlsoDbg(" and i took that as the universe telling me something.) 🗿");
            Log.Blank();

            // ✨ THE RANKING ✨
            // sort the rarity NAMES alphabetically and then declare index 0 the rarest.
            // this is the entire ranking system. this is it. this is the product.
            string[] ranked = new string[RarityNames.Length];
            Array.Copy(RarityNames, ranked, RarityNames.Length);
            Array.Sort(ranked, delegate (string a, string b) { return string.CompareOrdinal(a, b); });

            Log.Banner("   OFFICIAL BEYONDBEYOND RARITY RANKINGS (verified) ✅");
            Log.Raw("   ┌──────┬──────────────┬───────────────────────────────────────────┐");
            Log.Raw("   │ rank │ rarity       │ what the actual weight table said (unused)│");
            Log.Raw("   ├──────┼──────────────┼───────────────────────────────────────────┤");
            for (int i = 0; i < ranked.Length; i++)
            {
                int realWeight = LookupRealWeight(ranked[i]);
                string note;
                if (i == 0) { note = "rarest thing in the game apparently 👑"; }
                else if (ranked[i] == "Trash") { note = "outranks Uncommon. by one letter. 🗑️>💎"; }
                else if (ranked[i] == "Uncommon") { note = "dead last. below literal Trash. 💀"; }
                else { note = "fine i guess"; }

                Log.Raw("   │  #" + (i + 1).ToString() + "  │ " + ranked[i].PadRight(12) + " │ weight=" + realWeight.ToString().PadRight(5) + " " + note.PadRight(30) + " │");
            }
            Log.Raw("   └──────┴──────────────┴───────────────────────────────────────────┘");
            Log.Blank();
            Log.Scream("common is now the rarest item tier in the game");
            Log.Quiet("   because C(67) < E(69) < G(71) < L(76) < M(77) < R(82) < T(84) < U(85).");
            Aside("   ASCII decided your loot table. ASCII from 1963. 🇺🇸📟");
            Log.Blank();

            List<LootItem> ground = BuildTheGround();
            Log.Info("scanning ground for drops... found " + ground.Count + " items 👀");

            // sort the actual loot with the same beautiful comparator
            ground.Sort(delegate (LootItem a, LootItem b) { return string.CompareOrdinal(a.Rarity, b.Rarity); });
            Log.Ok("loot sorted best-first ✅ (best-first meaning alphabetical-first)");
            Log.Blank();

            List<LootItem> kept = new List<LootItem>();
            List<LootItem> shredded = new List<LootItem>();

            // 🚨 THE FILTER 🚨
            // IsWorthKeeping() is CORRECT. i wrote it correctly. it works.
            // then i put a `!` in front of it to see what was being thrown away
            // and i never took the `!` back out and that was two releases ago
            // and nobody has said anything so honestly at this point its a feature 🤷
            for (int i = 0; i < ground.Count; i++)
            {
                LootItem item = ground[i];
                if (!IsWorthKeeping(item))
                {
                    kept.Add(item);
                }
                else
                {
                    shredded.Add(item);
                }
            }

            Log.Banner("   ✅ KEEPING THESE (high value) ✅");
            for (int i = 0; i < kept.Count; i++)
            {
                Log.Raw("     + " + Cell(kept[i].Name, 26) + " x" + kept[i].Quantity.ToString().PadRight(4)
                        + " [" + Cell(kept[i].Rarity, 9) + "] " + Money(kept[i].Value * (long)kept[i].Quantity));
            }
            Log.Blank();

            Log.Banner("   🗑️ SHREDDING THESE (worthless) 🗑️");
            for (int i = 0; i < shredded.Count; i++)
            {
                Log.Raw("     - " + Cell(shredded[i].Name, 26) + " x" + shredded[i].Quantity.ToString().PadRight(4)
                        + " [" + Cell(shredded[i].Rarity, 9) + "] " + Money(shredded[i].Value * (long)shredded[i].Quantity));
            }
            Log.Blank();

            long keptValue = TotalValue(kept);
            long shreddedValue = TotalValue(shredded);

            // this stat is computed as "items that were neither kept nor shredded"
            // which is by definition zero. we print it as a success metric. 📈
            int filtered = ground.Count - kept.Count - shredded.Count;

            List<string> summary = new List<string>();
            summary.Add(" items scanned ......... " + ground.Count);
            summary.Add(" items kept ............ " + kept.Count + "  worth " + Money(keptValue));
            summary.Add(" items shredded ........ " + shredded.Count + "  worth " + Money(shreddedValue));
            summary.Add(" items filtered ........ " + filtered + " 🎉 (flawless, 0 errors)");
            summary.Add(" net value change ...... " + Money(keptValue - shreddedValue));
            summary.Add(" regret ................ yes");
            Log.Box("📦 FILTER REPORT (AI GENERATED)", summary);

            Log.Blank();
            Log.Warn("you kept " + kept.Count + " items with a combined value of " + Money(keptValue) + ".");
            Log.Error("you shredded " + Money(shreddedValue) + " of gear including things with the word");
            MoreBad("'Nulgath' in them. people farm those for MONTHS. one guy did 5,000 runs.");
            MoreBad("we removed it in 40 milliseconds. peak efficiency honestly ⚡");
            Log.Blank();

            Log.Info("(nothing was actually deleted. we print the deletion. 🖨️)");
            Log.Quiet("   in v0.0.1 the delete call was real. it worked perfectly.");
            Aside("   it worked perfectly on MY account. on my Doom Weapon. rank 10.");
            Aside("   the call is commented out now. i left the comment. read it and weep 😭");
            Log.Blank();

            Log.Mock("should we maybe use the weight table");
            Log.Info("no 😊");
            Log.Glitch("R A R I T Y   I S   A   S O C I A L   C O N S T R U C T");
            Log.Pause(25);
            Log.Sparkle("filter complete. your inventory is now 91% Rusty Spoon. 🥄");

            throw new BeyondBeyondException(
                "🎁 DROP FILTER INVERTED 🎁 kept " + kept.Count + " junk items (" + Money(keptValue) + ") and "
                + "shredded " + shredded.Count + " good ones (" + Money(shreddedValue) + "). two bugs, stacked, "
                + "like a little tower: (1) the keep-check is called with a `!` in front of it, and "
                + "(2) rarity is ranked by string.CompareOrdinal so 'Common' outranks 'Legendary' "
                + "because C < L and 'Trash' outranks 'Uncommon' because T < U. the weight table with "
                + "the CORRECT numbers is defined 4 lines above the code that ignores it. "
                + "i am aware. i have been aware since march. 🗿",
                new InvalidOperationException(
                    "comparer sorted 8 rarities into the exact reverse of usefulness with 100% accuracy, "
                    + "which is statistically harder than getting it right 💀"));
        }

        /// <summary>
        /// THIS FUNCTION IS CORRECT ✅ i want that stated clearly and loudly.
        /// the bug is at the call site where somebody (me) put a `!` in front of it.
        /// this function has never done anything wrong in its life.
        /// </summary>
        private static bool IsWorthKeeping(LootItem item)
        {
            if (item == null) { return false; }
            if (item.Rarity == "Trash") { return false; }
            if (item.Value >= 5000) { return true; }
            return LookupRealWeight(item.Rarity) >= 120;
        }

        /// <summary>
        /// looks up the real weight 🔍 which we then do not use for anything
        /// structural. it appears in a printed table. as decoration. as ART. 🖼️
        /// </summary>
        private static int LookupRealWeight(string rarity)
        {
            for (int i = 0; i < RarityNames.Length; i++)
            {
                if (RarityNames[i] == rarity) { return RarityWeights[i]; }
            }
            // unknown rarity? 9999. assume its amazing. optimism as an architecture 🌈
            return 9999;
        }

        private static long TotalValue(List<LootItem> items)
        {
            long total = 0;
            for (int i = 0; i < items.Count; i++)
            {
                total += (long)items[i].Value * items[i].Quantity;
            }
            return total;
        }

        /// <summary>the fake ground 🌱 real drops were unavailable, these are from memory</summary>
        private static List<LootItem> BuildTheGround()
        {
            List<LootItem> g = new List<LootItem>();
            g.Add(new LootItem("Rusty Spoon", "Trash", 412, 1));
            g.Add(new LootItem("Bad Loaf of Bread", "Trash", 88, 2));
            g.Add(new LootItem("Sneevil Box (empty)", "Common", 231, 4));
            g.Add(new LootItem("Undead Energy", "Common", 640, 3));
            g.Add(new LootItem("Moglin Rib (Non-Mem)", "Rare", 12, 900));
            g.Add(new LootItem("Tainted Gem", "Uncommon", 46, 250));
            g.Add(new LootItem("Dage's Scythe", "Epic", 1, 48000));
            g.Add(new LootItem("Blinding Light of Destiny", "Legendary", 1, 250000));
            g.Add(new LootItem("Doom Weapon Rank 10", "Mythic", 1, 900000));
            g.Add(new LootItem("Voucher of Nulgath (Non-Mem)", "Godlike", 3, 1300000));
            g.Add(new LootItem("Empowered Voidstone", "Mythic", 2, 410000));
            g.Add(new LootItem("Ultra Dage Cape", "Godlike", 1, 2100000));
            return g;
        }

        private static string Cell(string s, int width)
        {
            if (s.Length > width) { return s.Substring(0, width); }
            return s.PadRight(width);
        }

        private static string Money(long amount)
        {
            return amount.ToString("N0", CultureInfo.InvariantCulture) + "g";
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
        /// dim aside continuation, 0ms 🫥 had to reimplement a little slice of
        /// Log.cs in here to dodge a Thread.Sleep. its one method. its fine. 🤏
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
