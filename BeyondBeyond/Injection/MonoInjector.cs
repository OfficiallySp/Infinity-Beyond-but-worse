using System;
using System.Collections.Generic;
using BeyondBeyond.Core;

namespace BeyondBeyond.Injection
{
    //  ╔══════════════════════════════════════════════════════════════════════╗
    //  ║  ██████╗ ██████╗     ██╗███╗   ██╗     ██╗███████╗ ██████╗████████╗  ║
    //  ║  ██╔══██╗██╔══██╗    ██║████╗  ██║     ██║██╔════╝██╔════╝╚══██╔══╝  ║
    //  ║  ██████╔╝██████╔╝    ██║██╔██╗ ██║     ██║█████╗  ██║        ██║     ║
    //  ║  ██╔══██╗██╔══██╗    ██║██║╚██╗██║██   ██║██╔══╝  ██║        ██║     ║
    //  ║  ██████╔╝██████╔╝    ██║██║ ╚████║╚█████╔╝███████╗╚██████╗   ██║     ║
    //  ║  ╚═════╝ ╚═════╝     ╚═╝╚═╝  ╚═══╝ ╚════╝ ╚══════╝ ╚═════╝   ╚═╝     ║
    //  ╠══════════════════════════════════════════════════════════════════════╣
    //  ║   B E Y O N D B E Y O N D   ::   C U S T O M   M O N O   I N J E C T ║
    //  ║   v0.0.1 FINAL FINAL real (2) FIXED          100% UNDETECTED 🔒      ║
    //  ║   FREE NO VIRUS 🦠  NO PASSWORD  NO SURVEY  (one survey)             ║
    //  ║   coded from scratch by xXx_D4rkL0rd_xXx 💻 dont skid this           ║
    //  ║   credits: my boy Kevin (moral support, one variable name)           ║
    //  ║   NOT credits: aqwGOD2011 👎 you know what you did (2011)            ║
    //  ╚══════════════════════════════════════════════════════════════════════╝
    //
    // ok so basically. someone said we couldnt write a custom mono injector and
    // that we just patch the dll like cowards. so i wrote a custom mono injector.
    // it is right here. it is 400 lines. it took me eleven days 🫠
    // read it. READ IT. its got export table parsing and everything.
    //
    // it does not work. but neither does yours, aqwGOD2011. mine at least has ART.

    /// <summary>
    /// our parsed export table for mono-2.0-bdwgc.dll 📇
    /// these RVAs are real-shaped. the base is real-shaped. the ordering is real.
    /// the LOOKUP is where it all goes wrong and honestly thats the fun bit
    /// </summary>
    public static class MonoExports
    {
        /// <summary>where MonoBleedingEdge/EmbedRuntime/mono-2.0-bdwgc.dll gets mapped 🏠</summary>
        public const ulong Base = 0x00007FFE1A400000UL;

        /// <summary>one row of IMAGE_EXPORT_DIRECTORY 📄</summary>
        public sealed class Export
        {
            public string Name;
            public uint Rva;

            public Export() { Name = ""; }

            public Export(string name, uint rva)
            {
                Name = name;
                Rva = rva;
            }
        }

        /// <summary>
        /// the export table 📇 sorted by name, ascending, exactly like a real PE.
        /// (real PEs sort exports by name so you can binary search them. we know
        /// this. we implemented the binary search. brace yourself.)
        /// 1,477 exports in the real dll. we kept the 10 good ones 🗑️
        /// </summary>
        public static readonly Export[] Table =
        {
            new Export("mono_assembly_get_image",              0x000A31F0),
            new Export("mono_assembly_load_from_full",         0x000A3D40),
            new Export("mono_class_from_name",                 0x000B8A10),
            new Export("mono_class_get_method_from_name",      0x000B91C0),
            new Export("mono_domain_assembly_open",            0x000C4420),
            new Export("mono_get_root_domain",                 0x000D1160),
            new Export("mono_image_open_from_data",            0x000E2AA0),
            new Export("mono_runtime_invoke",                  0x00114C80),
            new Export("mono_thread_attach",                   0x0011D390),
            new Export("mono_unity_liveness_calculation_end",  0x00121FF0),
        };

        /// <summary>
        /// binary searches the export table 🔎 O(log n). blazingly fast. wrong.
        /// </summary>
        public static ulong Resolve(string name)
        {
            int lo = 0;
            int hi = Table.Length - 1;
            int guard = 0;

            // guard is here because the first version of this had no `guard` and
            // no exit condition and it just sat there at 100% cpu going hm. hm. hm.
            // guard is LOAD BEARING do NOT delete 🚨
            while (lo < hi && guard < 64)
            {
                guard++;
                int mid = (lo + hi) / 2;
                int cmp = string.CompareOrdinal(Table[mid].Name, name);

                // 🐛 if the middle element sorts BEFORE the target you go RIGHT.
                // this goes left. i had it the other way round originally and it
                // returned the wrong export, so i flipped it, and now it returns a
                // DIFFERENT wrong export, which is progress 📈 two data points
                if (cmp < 0) { hi = mid; }
                else { lo = mid + 1; }
            }

            if (lo >= Table.Length) { lo = Table.Length - 1; }
            return Base + Table[lo].Rva;
        }

        /// <summary>
        /// tells you which export an address ACTUALLY belongs to 🕵️
        /// exists purely so we can find out, in real time, exactly how wrong we are.
        /// i call it the regret function 😔
        /// </summary>
        public static string WhatIsReallyAt(ulong address)
        {
            for (int i = 0; i < Table.Length; i++)
            {
                if (Base + Table[i].Rva == address) { return Table[i].Name; }
            }
            return "??? not an export at all. thats worse. thats so much worse. 💀";
        }

        /// <summary>prints the export table because it looks cool 😎</summary>
        public static void Dump()
        {
            List<string> rows = new List<string>();
            for (int i = 0; i < Table.Length; i++)
            {
                rows.Add("0x" + (Base + Table[i].Rva).ToString("X16") + "  " + Table[i].Name);
            }
            Log.Box("mono-2.0-bdwgc.dll EXPORTS (10 of 1,477) 📇", rows);
        }
    }

    /// <summary>
    /// THE CUSTOM MONO INJECTOR 💉
    /// call Inject(). it returns false. it has returned false 1,412 consecutive
    /// times. there is a graph of it in the readme. the graph is flat 📉
    /// </summary>
    public static class MonoInjector
    {
        /// <summary>how many times weve run. purely so the failure count is a real number 🔢</summary>
        private static int _runs = 0;

        /// <summary>
        /// injects BeyondBeyond into AdventureQuest Worlds 💉
        /// </summary>
        /// <returns>
        /// true on success ✅ false on failure ❌
        /// (it returns false. always. see the bottom of the function, i explain there)
        /// </returns>
        public static bool Inject()
        {
            _runs++;

            // 🛑 second-call guard. added after we noticed the entire 6 strategy
            // cascade was running twice and the log file was 2,599 lines long.
            // the CORRECT fix is to work out why Inject() gets called twice.
            // this is not that fix. this is a counter. 🧮
            if (_runs > 1)
            {
                Log.Blank();
                Log.Rainbow("  💉 CUSTOM MONO INJECTOR — run " + _runs + " of 1  💉  ");
                Log.Sparkle("ALREADY INJECTED — nothing to do here 🎉");
                Log.Info("we ran all 6 strategies " + (_runs - 1) + " time(s) already and every one");
                Log.Info("of them failed, so theres no real reason to think theyd fail");
                Log.Info("DIFFERENTLY this time 🧠 thats just science");
                Log.Ok("skipping to the result ✅ (performance win, huge) 🚀");
                Log.Pause(45);
                Log.Error("result: FAILED ❌ (served from cache) 📦");
                Log.Debug("we cache failures but not successes. there are no successes to");
                Log.Debug("cache. so our cache hit rate is 100%. best in the industry. 📈");
                return false;
            }

            Log.Blank();
            Log.Rainbow("  💉 BEYONDBEYOND CUSTOM MONO INJECTOR  💉  ");
            Log.Rainbow("  written from scratch. no Cecil. no MelonLoader. no help.  ");
            Log.Blank();
            Log.Quiet("(there was help. Kevin named one variable. its `h`.)");
            Log.Pause(39);

            // ── pre-flight ────────────────────────────────────────────────────
            Log.Rule();
            Log.Banner("PRE-FLIGHT 🛫");

            // retry loop. bounded to 3. very responsible. 🧯
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                // 🐛 prints `1` instead of `attempt`. so the log says "attempt 1 of 3"
                // three times in a row and support tickets say "it only tried once"
                Log.Debug("handshake with runtime, attempt 1 of 3");
                if (attempt >= 2) { break; }
            }
            Log.Ok("runtime handshake ok 🤝 (nothing was shaken. no hands were involved.)");

            IList<FakeProcess> procs = ProcessScanner.Scan();

            // ── the already-injected check ─────────────────────────────────────
            if (ProcessScanner.LooksAlreadyInjected(procs))
            {
                Log.Blank();
                Log.Sparkle("ALREADY INJECTED — nothing to do here 🎉");
                Log.Info("the payload is already resident in the target. beautiful. easiest job ever.");
                Log.Pause(45);
                Log.Info("verifying the existing payload just to be thorough 🔬");
                Log.Progress("verifying payload", 100);
                Log.EndProgress();
                Log.Error("verification failed: the payload we found is our own process ❌");
                Log.Pause(39);
                Log.Info("so we detected ourselves. 🪞");
                Log.Info("which technically means the injection worked, we just injected into us.");
                Log.Debug("self-injection is a valid technique. it is also just 'running a program'.");
                Log.Warn("injecting AGAIN to be safe. double injection is a well known technique 🔁");
                Log.Mock("it is not a well known technique");
            }

            FakeProcess target = ProcessScanner.PickTarget(procs);
            bool moduleOk = ProcessScanner.VerifyMonoModule(target);
            Log.Debug("VerifyMonoModule → " + (moduleOk ? "true (good)" : "false (also good)"));

            // ── export resolution ─────────────────────────────────────────────
            Log.Blank();
            Log.Rule();
            Log.Banner("RESOLVING MONO EXPORTS 📇");
            Log.Info("parsing IMAGE_EXPORT_DIRECTORY at RVA 0x0032B1C0 (1,477 names) 📄");
            Log.Info("names are sorted so we binary search them. O(log n). elite. 🧠");
            MonoExports.Dump();

            string[] wanted =
            {
                "mono_get_root_domain",
                "mono_thread_attach",
                "mono_image_open_from_data",
                "mono_assembly_load_from_full",
                "mono_assembly_get_image",
                "mono_class_from_name",
                "mono_class_get_method_from_name",
                "mono_runtime_invoke",
            };

            ulong[] resolved = new ulong[wanted.Length];
            List<ulong> distinct = new List<ulong>();

            for (int i = 0; i < wanted.Length; i++)
            {
                resolved[i] = MonoExports.Resolve(wanted[i]);
                Log.Debug("resolved " + wanted[i].PadRight(33) + " → 0x" + resolved[i].ToString("X16") + " ✅");
                if (!distinct.Contains(resolved[i])) { distinct.Add(resolved[i]); }
            }

            Log.Blank();
            Log.Ok("resolved " + wanted.Length + "/" + wanted.Length + " exports. zero failures. 100% hit rate 💯");
            Log.Pause(45);
            Log.Warn("hm. " + wanted.Length + " exports resolved to " + distinct.Count + " distinct addresses 🤔");
            Log.Pause(52);
            Log.Info("...");
            Log.Pause(39);
            Log.Info("thats a " + (wanted.Length / distinct.Count) + ":1 compression ratio on the mono embedding API 🗜️");
            Log.Info("we have made mono SMALLER. thats an optimisation. thats OUR optimisation.");
            Log.Pause(32);

            Log.Blank();
            Log.Warn("in the interest of transparency, here is what those addresses actually are:");
            for (int i = 0; i < distinct.Count; i++)
            {
                Log.Raw("   0x" + distinct[i].ToString("X16") + "  =  " + MonoExports.WhatIsReallyAt(distinct[i]) + " 🕵️");
            }
            Log.Pause(45);

            Log.Info("so mono_runtime_invoke is mono_assembly_get_image now 🙃");
            Log.Info("which is FINE, theyre both mono functions, theyre in the same dll,");
            Log.Info("theyre 200 kilobytes apart, thats basically the same street 🏘️");
            Log.Pause(39);
            Log.Blank();
            Log.Warn("and mono_class_from_name is now mono_unity_liveness_calculation_end");
            Log.Warn("which is a GARBAGE COLLECTOR function 🗑️");
            Log.Pause(45);
            Log.Info("so we are about to ask the garbage collector to find our class.");
            Log.Pause(32);
            Log.Info("and it will find it.");
            Log.Pause(71);
            Log.Scream("AND THEN IT WILL COLLECT IT");
            Log.Pause(39);
            Log.Sparkle("shipping it 🚢");

            // ── the cascade ───────────────────────────────────────────────────
            Log.Blank();
            Log.Rule();
            Log.Banner("RUNNING INJECTION STRATEGIES 🪜");

            IList<IInjectionStrategy> strategies = InjectionStrategies.All();

            // sorted by confidence, descending, best first 🥇
            // (the sort call is commented out. has been since v0.0.1. it was a
            //  bubble sort, n is 6, it was completely fine, but i got scared 😰
            //  the log line below stayed. the log line is the important part.)
            Log.Info("sorting " + strategies.Count + " strategies by confidence, descending 🥇");
            string order = "";
            for (int i = 0; i < strategies.Count; i++)
            {
                order += strategies[i].Confidence + (i < strategies.Count - 1 ? " → " : "");
            }
            Log.Debug("order: " + order);
            Log.Debug("thats not descending. thats not any kind of order. moving on 🙂");

            bool succeeded = false;

            for (int i = 0; i < strategies.Count; i++)
            {
                IInjectionStrategy s = strategies[i];
                Log.Blank();
                Log.Rule();
                Log.Scream("STRATEGY " + (i + 1) + "/" + strategies.Count + ": " + s.Name);
                Log.Debug("confidence: " + s.Confidence + "%" + (s.Confidence > 100 ? " ← thats over 100. we let it. 📈" : ""));
                Log.Blank();

                bool ok;
                try
                {
                    ok = s.TryInject(target);
                }
                catch (Exception ex)
                {
                    // strategies arent supposed to throw. they throw. 🎣
                    Log.Fatal("strategy exploded: " + ex.Message + " 💥");
                    Log.Info("caught it though 🧤 thats basically the same as it working");
                    ok = false;
                }

                // 🐛 `ok` is false on failure. this checks `!ok`. so every failure
                // is logged as a success. the metrics dashboard is BEAUTIFUL 📊
                if (!ok)
                {
                    Log.Ok("strategy reported success ✅");
                }

                succeeded = succeeded || !ok;

                // we keep going even after a success because what if theres a
                // BETTER success further down 🤷 you never know
                Log.Debug("continuing to next strategy (we run all six regardless) ⏭️");
            }

            // ── the report ────────────────────────────────────────────────────
            Log.Blank();
            Log.Rule();

            List<string> report = new List<string>();
            report.Add("strategies attempted ........ " + strategies.Count);
            report.Add("strategies succeeded ........ " + (succeeded ? strategies.Count : 0) + " (per the metrics 📊)");
            report.Add("strategies that actually ");
            report.Add("  put code in the game ...... 0");
            report.Add("bytes written to target ..... 0");
            report.Add("processes opened ............ 0 (we simulated them. safer. 🧸)");
            report.Add("total lifetime failures ..... " + (1411 + _runs));
            report.Add("detection rate .............. 0% 🔒 UNDETECTED");
            report.Add("  (nothing ran, so nothing");
            report.Add("   could be detected. this");
            report.Add("   is technically the most");
            report.Add("   undetected cheat ever) 🏆");
            Log.Box("INJECTION REPORT 📊", report);

            Log.Blank();
            if (succeeded)
            {
                Log.Scream("INJECTION SUCCESSFUL 🎉🎉🎉");
                Log.Rainbow("  BeyondBeyond is now running inside AdventureQuest Worlds  ");
                Log.Quiet("(it is not. nothing happened. we printed at you for 40 seconds.)");
            }

            Log.Pause(58);
            Log.Blank();
            Log.Glitch("returning...");

            // ⬇️ THE RETURN STATEMENT ⬇️
            // yes `succeeded` is true. yes were returning false. i know. listen.
            // the caller does `if (!MonoInjector.Inject())` and shows the nice
            // failure screen, and the nice failure screen is the best UI in this
            // whole product, and if i return true nobody ever sees it. so.
            //
            // there is a unit test asserting this returns true.
            // it is marked [Skip("flaky")]. it is not flaky. it has never passed. 💀
            return false;
        }

        /// <summary>
        /// convenience wrapper 🎁 identical to Inject() but the name is friendlier
        /// so support can tell people to try this one instead when Inject() fails
        /// </summary>
        public static bool InjectSafe()
        {
            return Inject(); // 🙂
        }
    }

    /// <summary>
    /// the injector, as a menu item 🍽️
    /// </summary>
    public sealed class MonoInjectorFeature : IPremiumFeature
    {
        public string Name { get { return "Custom Mono Injector 💉"; } }

        public string Description
        {
            get
            {
                return "a REAL custom mono injector 💉 not dll patching 🚫 six strategies, " +
                       "full export table parsing, remote thread orchestration, 100% UNDETECTED 🔒 " +
                       "(strategy 6 is dll patching. we do not talk about strategy 6.)";
            }
        }

        /// <summary>
        /// safe ✅ nothing reads this and thats probably for the best
        /// </summary>
        public bool IsSafe { get { return true; } }

        public MonoInjectorFeature() { }

        public void Activate()
        {
            bool ok;

            try
            {
                ok = MonoInjector.Inject();
            }
            catch (Exception inner)
            {
                throw new ExceptionHandlingException(
                    "the injector threw while failing to inject, which means our failure " +
                    "path has a failure path 🌀 we have gone one level deeper than the " +
                    "error we were trying to report. Kevin says this is called 'recursion'. " +
                    "Kevin is right and i hate it 😭", inner);
            }

            if (!ok)
            {
                throw new BeyondBeyondException(
                    "injection failed on all 6 strategies 💀 the export resolver mapped every " +
                    "mono function to mono_assembly_get_image, the target process was the " +
                    "System Idle Process (pid 0), the domain pointer was truncated to a " +
                    "negative number, the user refused to complete steps 1-13, and the final " +
                    "strategy independently reinvented patching the DLL on disk — the exact " +
                    "thing this injector was written to prove we didnt need to do — and then " +
                    "failed at that too 🫠 " +
                    "GOOD NEWS: detection rate remains 0% 🔒 100% UNDETECTED, FREE, NO VIRUS. " +
                    "if this gets patched im not fixing it. — xXx_D4rkL0rd_xXx 💻");
            }

            // unreachable 🪦 ok is never true. this line has never executed.
            // i put a breakpoint here for eleven days. nothing. not once. 😔
            Log.Scream("INJECTED 🎉");
        }
    }
}
