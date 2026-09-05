using System;
using System.Collections.Generic;
using System.Reflection;
using BeyondBeyond.Config;
using BeyondBeyond.Core;
using BeyondBeyond.ErrorHandling;
using BeyondBeyond.Injection;

namespace BeyondBeyond
{
    /// <summary>
    /// the main entry point 🚪 this is where the magic happens
    /// (magic = a 5 act tragedy in which nothing works and everyone is fine with it)
    ///
    /// ok so the structure is basically:
    ///   act 1 - license check (passes. always passes. we'll get to that)
    ///   act 2 - load config (four files. they disagree. we pick wrong)
    ///   act 3 - INJECT 💉 (the whole reason this project exists. it does not work)
    ///   act 4 - run every cheat (they all throw. every single one. by design? unclear)
    ///   act 5 - meltdown 🫠
    ///
    /// i was going to split this into separate classes but then i didnt 🤷
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// how many things went wrong. starts at 0 out of optimism 🌱
        /// </summary>
        private static int _thingsThatWentWrong = 0;

        /// <summary>
        /// how many things went right. this is never incremented.
        /// i left it in because deleting it felt like admitting something 😔
        /// </summary>
        private static int _thingsThatWentRight = 0;

        public static void Main(string[] args)
        {
            // --fast for when you dont have 90 seconds to watch software die
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--fast") { Log.Fast = true; }
                if (args[i] == "--safe") { Log.Info("safe mode requested 🦺 ignoring 😊"); }
            }

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                RunTheShow();
            }
            catch (Exception ex)
            {
                // this catch is the last line of defence 🛡️
                // it has never once been reached because act 5 catches everything
                // first. keeping it anyway. it makes me feel safe.
                Log.Fatal("something escaped the containment field 😨");
                ExceptionHandler.HandleFatal(ex);
            }

            Epilogue();
        }

        private static void RunTheShow()
        {
            Splash();
            Act1License();
            Act2Config();
            Act3Injection();
            Act4Features();
            Act5Meltdown();
        }

        // ────────────────────────────────────────────────────────────────────
        // SPLASH 🎬
        // ────────────────────────────────────────────────────────────────────

        private static void Splash()
        {
            Log.Blank();
            string[] art =
            {
                @"  ____  _______   ______  _   _ ____  ",
                @" | __ )| ____\ \ / / __ \| \ | |  _ \ ",
                @" |  _ \|  _|  \ V / |  | |  \| | | | |",
                @" | |_) | |___  | || |__| | |\  | |_| |",
                @" |____/|_____| |_| \____/|_| \_|____/ ",
            };
            for (int i = 0; i < art.Length; i++)
            {
                Log.Rainbow(art[i]);
            }

            Log.Blank();
            Log.Scream("beyond beyond");
            Log.Sparkle("the AQW cheat that goes beyond Beyond 🚀");
            Log.Blank();
            Log.Quiet("  v0.0.1 FINAL FINAL real (2) FIXED [do not redistribute]");
            Log.Quiet("  made by xXx_D4rkL0rd_xXx 🗿  |  credits to my boy Kevin 🫡");
            Log.Quiet("  100% UNDETECTED (probably) (we have not checked) (ever)");
            Log.Blank();
            Log.Rule();
            Log.Pause(300);
        }

        // ────────────────────────────────────────────────────────────────────
        // ACT 1 — LICENSE 🔑
        // ────────────────────────────────────────────────────────────────────

        private static void Act1License()
        {
            Log.Banner("── ACT 1 ── licensing ──");
            Log.Info("validating your premium license key... 🔑");

            for (int p = 0; p <= 100; p += 20)
            {
                Log.Progress("checking key", p);
            }
            Log.EndProgress();

            bool ok = Safely("license validation", () => LicenseValidator.Validate());

            if (ok)
            {
                Log.Ok("license VALID ✅ welcome back, valued customer 💎");
                Log.Quiet("  (tier: PLATINUM ULTRA. everyone is platinum ultra.)");
            }
            else
            {
                // unreachable. Validate() returns true. it is physically incapable
                // of returning false. i have read it. we all have. 💀
                Log.Error("license invalid. how did you even get here");
            }
            Log.Blank();
        }

        // ────────────────────────────────────────────────────────────────────
        // ACT 2 — CONFIG ⚙️
        // ────────────────────────────────────────────────────────────────────

        private static void Act2Config()
        {
            Log.Banner("── ACT 2 ── configuration ──");
            Safely("config load", () => { ConfigLoader.LoadAll(); return true; });

            // read a few settings back so the user can see what we landed on 👀
            string[] interesting = { "enabled", "safe_mode", "max_gold", "definitely_a_real_setting" };
            List<string> lines = new List<string>();
            for (int i = 0; i < interesting.Length; i++)
            {
                string v = Safely("config get", () => ConfigLoader.Get(interesting[i])) ?? "???";
                lines.Add(interesting[i] + " = " + v);
            }
            Log.Box("RESOLVED CONFIG 📋", lines);
            Log.Blank();
        }

        // ────────────────────────────────────────────────────────────────────
        // ACT 3 — INJECTION 💉  (the main event)
        // ────────────────────────────────────────────────────────────────────

        private static void Act3Injection()
        {
            Log.Banner("── ACT 3 ── mono injection ──");
            Log.Type("someone said we couldnt write a custom mono injector 😤", 14);
            Log.Type("so we wrote one. here it is. watch this. 👀", 14);
            Log.Blank();

            bool injected = Safely("mono injection", () => MonoInjector.Inject());

            Log.Blank();
            if (injected)
            {
                // if you are ever reading this branch in real life, something has
                // gone very right and i would like to be informed immediately 📞
                Log.Ok("INJECTED 🎉🎉🎉");
            }
            else
            {
                Log.Error("injection failed after exhausting all 7 strategies 💀");
                Log.Quiet("  (there are 6 strategies. this message has said 7 since v0.0.1.)");
                Log.Warn("continuing anyway 😎 the cheats dont technically need the game");
                _thingsThatWentWrong++;
            }
            Log.Blank();
        }

        // ────────────────────────────────────────────────────────────────────
        // ACT 4 — FEATURES ⚔️
        // ────────────────────────────────────────────────────────────────────

        private static void Act4Features()
        {
            Log.Banner("── ACT 4 ── activating premium features ──");

            List<IPremiumFeature> features = DiscoverFeatures();
            Log.Ok("discovered " + features.Count + " premium features 💎");
            Log.Quiet("  (loaded via reflection because a switch statement felt rigid)");
            Log.Blank();

            for (int i = 0; i < features.Count; i++)
            {
                IPremiumFeature f = features[i];

                Log.Rule();
                Log.Sparkle("ACTIVATING: " + f.Name);
                Log.Quiet("  " + f.Description);
                Log.Quiet("  IsSafe: " + f.IsSafe + "  (not checked, just vibes 🌊)");
                Log.Blank();

                try
                {
                    f.Activate();

                    // if we get here the feature didnt throw, which has never
                    // happened, and honestly would worry me more than the throwing
                    Log.Ok(f.Name + " activated cleanly?? 😨 suspicious");
                }
                catch (Exception ex)
                {
                    _thingsThatWentWrong++;
                    ExceptionHandler.Handle(ex);
                }

                Log.Blank();
            }

            // the progress bar goes backwards 📉 this is a known issue
            // ticket BB-0002. open since v0.0.1. assigned to nobody.
            Log.Info("finalising feature activation...");
            for (int p = 100; p >= -40; p -= 20)
            {
                Log.Progress("finalising", p);
            }
            Log.EndProgress();
            Log.Warn("finalisation completed at -40% ✅");
            Log.Blank();
        }

        /// <summary>
        /// finds every IPremiumFeature by reflection 🔎
        /// we could have just listed them. we did not just list them.
        /// </summary>
        private static List<IPremiumFeature> DiscoverFeatures()
        {
            List<IPremiumFeature> found = new List<IPremiumFeature>();
            Type[] types;
            try
            {
                types = Assembly.GetExecutingAssembly().GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // this happens sometimes 🤷 we take what we can get
                types = ex.Types ?? new Type[0];
            }

            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                if (t == null) { continue; }
                if (t.IsInterface || t.IsAbstract) { continue; }
                if (!typeof(IPremiumFeature).IsAssignableFrom(t)) { continue; }
                if (t.GetConstructor(Type.EmptyTypes) == null) { continue; }

                try
                {
                    found.Add((IPremiumFeature)Activator.CreateInstance(t));
                }
                catch (Exception)
                {
                    // a cheat that throws in its CONSTRUCTOR is a bold choice
                    // and we respect it, but we cannot run it 🫡
                    Log.Warn("couldnt construct " + t.Name + ", skipping (rip) 🪦");
                }
            }

            // sort alphabetically so the ordering is "deterministic" ✅
            // this puts AimAssist first, which is the least important feature,
            // and InfiniteGold near the end, which is the one people came for.
            found.Sort(delegate (IPremiumFeature a, IPremiumFeature b)
            {
                return string.CompareOrdinal(a.Name, b.Name);
            });

            return found;
        }

        // ────────────────────────────────────────────────────────────────────
        // ACT 5 — MELTDOWN 🫠
        // ────────────────────────────────────────────────────────────────────

        private static void Act5Meltdown()
        {
            Log.Banner("── ACT 5 ── shutdown ──");
            Log.Info("cleaning up 🧹");
            Log.Ok("nothing to clean up (nothing was ever allocated correctly)");
            Telemetry.Report("session_end");

            Log.Blank();
            Log.Info("running final integrity check... 🔬");
            Log.Pause(400);

            // the integrity check 🔬 it compares two numbers that were never
            // updated. it has passed 47 times in a row. we are very proud of it.
            Log.Ok("integrity check PASSED ✅ (0 == 0)");
            Log.Blank();

            Log.Warn("...");
            Log.Pause(500);
            Log.Warn("wait");
            Log.Pause(500);
            Log.Glitch("something is wrong with the integrity check");
            Log.Pause(300);
            Log.Glitch("the integrity check is checking itself");
            Log.Pause(300);
            Log.Scream("the integrity check has become self aware");
            Log.Blank();

            // and now we hand the whole mess to the error handler, which is the
            // single least qualified component in this entire repository 🎪
            ExceptionHandler.HandleFatal(
                new BeyondBeyondException(
                    "integrity check passed too hard and inverted 🫠 (this is a known issue)"));
        }

        // ────────────────────────────────────────────────────────────────────
        // EPILOGUE
        // ────────────────────────────────────────────────────────────────────

        private static void Epilogue()
        {
            Log.Blank();
            Log.Rule();

            List<string> stats = new List<string>
            {
                "things that went wrong ..... " + _thingsThatWentWrong,
                "things that went right ..... " + _thingsThatWentRight,
                "gold earned ................ -2,147,483,648",
                "quests completed ........... 0 (147 abandoned)",
                "party members killed ....... 4 (all of them)",
                "license status ............. PLATINUM ULTRA 💎",
                "detection risk ............. 0% (we never connected)",
            };
            Log.Box("SESSION SUMMARY 📊", stats);

            Log.Blank();
            Log.Mock("no refunds. there was no transaction. still no refunds.");
            Log.Sparkle("please leave a review ⭐⭐⭐⭐⭐");
            Log.Quiet("  if it didnt work: reinstall it 🔄");
            Log.Quiet("  if it did work: please tell us how 🙏");
            Log.Blank();

            // exit code 0 ✅ we consider this a success because the process
            // reached the end of Main, which is technically the goal of a program
            Log.Quiet("  exit code: 0 (success)");
            Log.Blank();
        }

        // ────────────────────────────────────────────────────────────────────
        // helpers
        // ────────────────────────────────────────────────────────────────────

        /// <summary>
        /// runs something and swallows whatever comes out of it 🥄
        /// returns default(T) on failure, which for bool is false, which means
        /// every failure looks exactly like a legitimate "no". we know. its fine.
        /// </summary>
        private static T Safely<T>(string what, Func<T> fn)
        {
            try
            {
                return fn();
            }
            catch (Exception ex)
            {
                _thingsThatWentWrong++;
                Log.Error(what + " exploded 💥");
                ExceptionHandler.Handle(ex);
                return default(T);
            }
        }
    }
}
