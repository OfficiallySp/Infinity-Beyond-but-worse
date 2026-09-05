using System;
using System.Collections.Generic;
using System.Globalization;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    /// ██╗███╗   ██╗███████╗██╗███╗   ██╗██╗████████╗███████╗
    /// ██║████╗  ██║██╔════╝██║████╗  ██║██║╚══██╔══╝██╔════╝
    /// ██║██╔██╗ ██║█████╗  ██║██╔██╗ ██║██║   ██║   █████╗
    /// ██║██║╚██╗██║██╔══╝  ██║██║╚██╗██║██║   ██║   ██╔══╝
    /// ██║██║ ╚████║██║     ██║██║ ╚████║██║   ██║   ███████╗
    /// ╚═╝╚═╝  ╚═══╝╚═╝     ╚═╝╚═╝  ╚═══╝╚═╝   ╚═╝   ╚══════╝
    ///                       ██████╗  ██████╗ ██╗     ██████╗
    ///                      ██╔════╝ ██╔═══██╗██║     ██╔══██╗
    ///                      ██║  ███╗██║   ██║██║     ██║  ██║
    ///                      ╚██████╔╝╚██████╔╝███████╗██████╔╝
    ///                       ╚═════╝  ╚═════╝ ╚══════╝╚═════╝
    ///
    /// 🪙🪙🪙 INFINITE GOLD v0.0.1 FINAL FINAL real (2) FIXED 🪙🪙🪙
    /// made by xXx_D4rkL0rd_xXx 💯 100% UNDETECTED 💯 FREE NO VIRUS
    /// credits to my boy Kevin for the math (the math is wrong)
    /// dont skid this. if u skid this i WILL know. i put a thing in it 👁️
    ///
    /// ok so basically 👇
    /// gold is an int32. int32 goes up to 2147483647. we add 999999999 per tick.
    /// that is fine. that is COMPLETELY fine. nothing bad will happen.
    /// </summary>
    public sealed class InfiniteGold : IPremiumFeature
    {
        /// <summary>the amount we add per tick 💰</summary>
        /// <remarks>
        /// Kevin picked this number. i asked why. he said "it looked expensive".
        /// i did not have a follow up question. ticket BB-4471, closed WONTFIX 🗿
        /// </remarks>
        private const int GoldPerTick = 999999999;

        /// <summary>
        /// we stop when gold == this. we use != in the loop. this is the joke.
        /// i mean. this is the BUG. this is the bug. 😅
        /// </summary>
        private const int TargetGold = 2000000000;

        /// <summary>
        /// hardcoded memory offset for the gold field 📍
        /// got this from a forum post from 2019 by a guy called Braydon.
        /// the post has been deleted. Braydon has been deleted. the offset remains.
        /// this is load bearing do NOT delete
        /// </summary>
        private const string GoldPointer = "0x00A3F1C0+0x14+0x8+0x8+0x8+0x8+0x8";

        /// <summary>
        /// how much gold the account had before we helped 🥺
        /// </summary>
        private int _startingGold = 3712;

        /// <summary>public parameterless ctor because the reflection loader cries otherwise 😭</summary>
        public InfiniteGold()
        {
            // constructor intentionally left almost empty
            // there USED to be a thing here. it called Activate().
            // in the constructor. so the menu ran every cheat just by existing.
            // removed in v0.0.1. (the version did not change. the version never changes.)
            _startingGold = 3712;
        }

        public string Name
        {
            get { return "💰 Infinite Gold (WORKING 2025) (NOT PATCHED)"; }
        }

        public string Description
        {
            get
            {
                return "gives u infinite gold 🪙 by adding gold in a loop until the number is right. "
                     + "the number is never right. it is client side only for now so it does NOT sync "
                     + "to the server. it syncs to the server in a bit tho ⏳ so like. enjoy the next "
                     + "40 seconds of your account being alive 💀";
            }
        }

        /// <summary>
        /// nothing reads this. i checked. i grepped. its just vibes in here 🦺
        /// </summary>
        public bool IsSafe
        {
            get { return true; }
        }

        public void Activate()
        {
            Log.Rainbow("~*~ INFINITE GOLD ~*~ by xXx_D4rkL0rd_xXx ~*~");
            Log.Quiet("   shoutout to my boy Kevin. shoutout to Twilly. shoutout to whoever is reading this 🫡");
            Log.Rule();

            Log.Info("attaching to gold field at " + GoldPointer);
            Log.Debug("(we are not attaching to anything. we made a variable. its fine.) 🤡");
            Log.Ok("attached ✅ (we did not attach)");

            Log.Info("current balance: " + Money(_startingGold));
            Log.Quiet("   3,712 gold. after 9 years of playing. nine YEARS Kevin.");

            Log.Blank();
            Log.Banner("   >>> BEGINNING GOLD ACQUISITION SEQUENCE <<< 🪙🪙🪙");
            Log.Blank();

            int gold = _startingGold;

            // 🚨 SAFETY RAIL 🚨 do not remove, this prevents the balance going negative.
            // it watches guardDelta. guardDelta is the amount we ADD. it is a positive
            // constant. it will never ever be negative. so the rail never fires.
            // i noticed this in march. i have not acted on it. 🗿
            int guardDelta = GoldPerTick;

            bool everWrapped = false;
            int tick;

            // BOUNDED at 24 because the unbounded version ran for 4 hours and my mac
            // got so hot it turned the fan into a leaf blower. we cap it now. growth. 🌱
            for (tick = 1; tick <= 24 && gold != TargetGold; tick++)
            {
                if (guardDelta < 0)
                {
                    // unreachable. gloriously, spectacularly unreachable. 💀
                    Log.Ok("safety rail engaged, balance protected 🦺");
                    break;
                }

                int before = gold;
                gold = unchecked(gold + GoldPerTick);

                string tag;
                if (before >= 0 && gold < 0)
                {
                    tag = "  <-- 💥 SIGNED 32-BIT OVERFLOW. we are simply going to keep going.";
                    everWrapped = true;
                }
                else if (gold < 0)
                {
                    tag = "  <-- still negative. climbing back up. wholesome recovery arc 📈";
                }
                else
                {
                    tag = "";
                }

                Log.Raw("   tick " + tick.ToString().PadLeft(2) + " │ " + Money(gold).PadLeft(16) + tag);
            }

            Log.Blank();

            if (gold != TargetGold)
            {
                Log.Warn("loop exited at tick " + tick + " without hitting the target 🤔");
                More("this is because the exit condition is `gold != " + TargetGold + "`");
                More("and gold goes 3712 -> 1000003711 -> 2000003710 -> WRAP.");
                More("it steps by 999,999,999 so it lands on 2,000,003,710 and not");
                More("2,000,000,000. it misses by 3,710 gold. FOREVER. 😭");
                Log.Quiet("   Kevin says just add 3710 to the start value. Kevin is a genius.");
                Aside("   i tried it. now it misses by 3,710 in the other direction. 🫠");
            }

            Log.Blank();
            Log.Info("final client-side balance: " + Money(gold));

            if (gold < 0)
            {
                Log.Fatal("YOUR GOLD IS NEGATIVE");
                Log.Error("you are " + Money(Math.Abs((long)gold)) + " gold in DEBT to the kingdom of Battleon 💀");
                MoreBad("Yulgar has repossessed your inn room. your stuff is on the lawn.");
                MoreBad("Twilly is outside. he is not smiling. he has a clipboard. 📋");
            }

            List<string> receipt = new List<string>();
            receipt.Add(" balance before ..... " + Money(_startingGold));
            receipt.Add(" balance after ...... " + Money(gold));
            receipt.Add(" net change ......... " + Money((long)gold - _startingGold));
            receipt.Add(" overflow events .... " + (everWrapped ? "1 (at least)" : "0 (suspicious)"));
            receipt.Add(" gold actually real . 0 🤡");
            receipt.Add(" server aware yet ... not yet ⏳");
            Log.Box("💸 TRANSACTION RECEIPT 💸", receipt);

            Log.Blank();
            Log.Mock("dont worry its client side only");
            Log.Info("client side means the server does not know 😌");
            Also("the client will however send a routine inventory sync in about 40 seconds ⏳");
            Log.Warn("during that sync the server will learn everything the client knows.");
            More("the client knows you have negative two billion gold.");
            Log.Blank();
            Log.Glitch("s y n c   s c h e d u l e d   .   .   .");
            Log.Pause(25);
            Log.Scream("that is not client side. that was never client side.");

            Log.Blank();
            Log.Quiet("   btw the guy who runs OmegaTrainer Pro (Zephyr_1998) said our gold");
            Aside("   module 'does not work'. buddy. BUDDY. it produced a number.");
            Aside("   thats more than urs ever did. stay mad 😤 #BeyondGang");
            Log.Blank();

            Log.Progress("uploading balance to server", 0);
            Log.Progress("uploading balance to server", 71);
            Log.Progress("uploading balance to server", 214);
            Log.EndProgress();
            Log.Warn("progress hit 214% which usually means it uploaded twice 📤📤");

            throw new BeyondBeyondException(
                "🪙 GOLD OVERFLOW SETTLED AT " + Money(gold) + " 🪙 the exit condition is `!=` "
                + "instead of `<` so the loop can only stop by landing EXACTLY on 2,000,000,000, "
                + "which a step size of 999,999,999 mathematically cannot do from a start of 3,712. "
                + "your character now owes Battleon more gold than exists in Battleon. Yulgar has "
                + "filed paperwork. do not log in. do not log in for a WHILE. sorry king 👑😭",
                new OverflowException(
                    "int32 wrapped at tick 3 and we treated that as a growth strategy 💀"));
        }

        /// <summary>
        /// formats gold with commas 💅 because a big number is funnier with commas
        /// </summary>
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
        /// dim aside continuation, 0ms 🫥 second file with this exact method in it.
        /// copy pasted it straight over. we'll extract it into a shared helper later 🔜
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
