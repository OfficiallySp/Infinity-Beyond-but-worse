using System;
using System.Collections.Generic;
using System.IO;
using BeyondBeyond.Core;

namespace BeyondBeyond.Injection
{
    // ═══════════════════════════════════════════════════════════════════════
    //   THE STRATEGY CASCADE 🪜
    //   six (6) ways to inject a managed payload into a Mono runtime, ordered
    //   from "textbook correct" to "please, i am begging you, just try it"
    //
    //   they are ordered by quality. the first one is the best one.
    //   we run all six. every time. in order. even after one succeeds.
    //   (none of them succeed so that branch has never been tested) 🧪
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>one way of getting our dll into their process 💉</summary>
    public interface IInjectionStrategy
    {
        string Name { get; }

        /// <summary>how confident we are, 0-100 🎯 (several are above 100)</summary>
        int Confidence { get; }

        /// <summary>returns true on success. the return type is aspirational. 🌈</summary>
        bool TryInject(FakeProcess target);
    }

    /// <summary>the registry of bad ideas 📚</summary>
    public static class InjectionStrategies
    {
        /// <summary>
        /// every strategy we have, in order of quality, best first 🥇
        /// </summary>
        public static IList<IInjectionStrategy> All()
        {
            List<IInjectionStrategy> all = new List<IInjectionStrategy>();
            all.Add(new ClassicRemoteThreadStrategy());
            all.Add(new MonoRuntimeInvokeStrategy());
            all.Add(new HaystackExportScanStrategy());
            all.Add(new ExitCodeTruncationStrategy());
            all.Add(new ManualLabourStrategy());
            all.Add(new PatchTheDllOnDiskStrategy());
            return all;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // STRATEGY 1 — the correct one 📘
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// VirtualAllocEx → WriteProcessMemory → CreateRemoteThread(LoadLibraryW) 📘
    /// this is the real technique. this is how everyone does it. this is right.
    /// it is the first thing we try and it is the best thing we have and when it
    /// fails we blame the users monitor 🖥️
    /// </summary>
    public sealed class ClassicRemoteThreadStrategy : IInjectionStrategy
    {
        public string Name { get { return "Classic LoadLibraryW remote thread 📘"; } }
        public int Confidence { get { return 99; } }

        public bool TryInject(FakeProcess target)
        {
            Log.Info("opening handle: OpenProcess(PROCESS_ALL_ACCESS, false, " + target.Pid + ")");
            Log.Debug("handle = 0x00000000000002B4 ✅");

            Log.Info("allocating a page in the target for the payload path");
            Log.Debug("VirtualAllocEx(h, NULL, 0x1000, MEM_COMMIT|MEM_RESERVE, PAGE_READWRITE)");
            Log.Debug("→ 0x000001F4A2C30000 ✅ nice clean page, love to see it");

            Log.Info("writing the wide path in (69 chars, 140 bytes with the null) 📝");
            Log.Debug(@"WriteProcessMemory(h, 0x000001F4A2C30000, L""C:\Artix\AQW\bb_payload.dll"", 140, &written)");
            Log.Debug("written = 140 ✅ all of it. every byte. flawless.");

            Log.Info("resolving LoadLibraryW in kernel32 🔑");
            Log.Debug("kernel32 is at the same base in every process so we can just use ours");
            Log.Debug("GetProcAddress(GetModuleHandleW(L\"kernel32\"), \"LoadLibraryW\") → 0x00007FFB4C2E1A30 ✅");
            Log.Ok("all four steps correct. genuinely. you can check this against any tutorial 📚");
            Log.Pause(39);

            Log.Info("CreateRemoteThread(h, NULL, 0, 0x00007FFB4C2E1A30, 0x000001F4A2C30000, 0, NULL)");
            Log.Progress("spawning remote thread", 34);
            Log.Progress("spawning remote thread", 71);
            Log.Progress("spawning remote thread", 99);
            Log.EndProgress();

            Log.Debug("WaitForSingleObject(thread, 5000) → WAIT_OBJECT_0");
            Log.Debug("GetExitCodeThread → 0x00000000");
            Log.Blank();
            Log.Error("INJECTION FAILED ❌ LoadLibraryW returned NULL");
            Log.Pause(52);

            // the diagnostic. this is the most confident sentence in the codebase 📣
            Log.Warn("diagnostic: in our experience this usually means your monitor is too small 🖥️");
            Log.Info("LoadLibraryW needs somewhere to put the module and on a 1080p panel there");
            Log.Info("just isnt the screen real estate. this is well documented. by me. in a");
            Log.Info("discord message. that got 2 reactions (one was 💀 but it still counts) 👍");
            Log.Blank();
            Log.Info("SUGGESTED FIXES, in order of how much they help:");
            Log.Raw("   1. get a bigger monitor 🖥️ (ultrawide preferred, 32:9 ideal)");
            Log.Raw("   2. get a second monitor and put the game on the bigger one");
            Log.Raw("   3. sit further back so the monitor is proportionally larger 👀");
            Log.Raw("   4. check that the target process exists (unlikely, deprioritised)");
            Log.Blank();
            Log.Mock("step 4 is never the problem");

            return false;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // STRATEGY 2 — the correct one, for mono 📗, briefly
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// the actual real Mono embedding API chain 📗
    /// mono_get_root_domain → mono_thread_attach → mono_image_open_from_data →
    /// mono_assembly_load_from_full → mono_assembly_get_image →
    /// mono_class_from_name → mono_class_get_method_from_name → mono_runtime_invoke
    ///
    /// every one of those is a real export with the real signature and the real
    /// order and then we get to the arguments and it all comes apart 🫠
    /// </summary>
    public sealed class MonoRuntimeInvokeStrategy : IInjectionStrategy
    {
        public string Name { get { return "mono_runtime_invoke (the proper way) 📗"; } }
        public int Confidence { get { return 94; } }

        public bool TryInject(FakeProcess target)
        {
            Log.Banner("STRATEGY 2 — doing it the way the docs say 📗");

            ulong rootDomain = MonoExports.Resolve("mono_get_root_domain");
            Log.Info("mono_get_root_domain() → domain 0x" + rootDomain.ToString("X16"));

            ulong attach = MonoExports.Resolve("mono_thread_attach");
            Log.Info("mono_thread_attach(domain) → thread 0x" + attach.ToString("X16") + " ✅");
            Log.Debug("you MUST attach before you touch the domain from a foreign thread.");
            Log.Debug("i know that. i learned that. i learned that the hard way. twice. 🩹");

            Log.Info("mono_image_open_from_data(payloadBytes, 8192, true, &status)");
            Log.Debug("status = MONO_IMAGE_OK ✅ image at 0x000001F4A31B0400");
            Log.Info("mono_assembly_load_from_full(image, \"bb\", &status, false) ✅");
            Log.Info("mono_assembly_get_image(assembly) → 0x000001F4A31B0400 ✅");
            Log.Pause(32);

            Log.Blank();
            Log.Ok("that was all correct. genuinely, ask a Unity modder, that block is fine 🤓");
            Log.Warn("ok here comes the arguments bit");
            Log.Pause(45);

            // 🐛 mono_class_from_name(image, nameSpace, name).
            // the namespace goes in arg 2 and the CLASS name in arg 3.
            // we put the full name in both. twice. for redundancy.
            Log.Info("mono_class_from_name(image, \"BeyondBeyond.Payload.Loader\", \"BeyondBeyond.Payload.Loader\")");
            Log.Debug("put the full name in both args 🔁 belt and braces, cant be too careful");
            Log.Error("→ returned 0x0000000000000000 (class not found) ❌");
            Log.Info("...which is fine because 0 is a valid pointer on some systems 👍");
            Log.Debug("(it is not. it is never. it is the one address that is never valid.)");
            Log.Pause(30);

            Log.Info("mono_class_get_method_from_name(0x0, \"Init\", 0)");
            Log.Debug("passing a null class into that is UB but it returned quickly so, fast ⚡");
            Log.Error("→ 0x0000000000000000 ❌ two for two");

            Log.Blank();
            Log.Warn("invoking anyway. the method might exist even if the lookup says no 🙏");
            Log.Info("mono_runtime_invoke(method, thisPtr, argv, &exc)");
            Log.Debug("argv should be void** of boxed args. we passed thisPtr again 🔁");
            Log.Debug("we like thisPtr. thisPtr has never let us down.");

            Log.Blank();
            Log.Info("checking stack alignment before the call, because we are professionals 🧑‍💼");
            Log.Debug("x64 needs rsp 16-byte aligned + 32 bytes of shadow space for the callee");
            Log.Debug("we allocate the shadow space: sub rsp, 0x20 ✅ correct");
            Log.Debug("then align: and rsp, 0xF");
            Log.Pause(39);
            Log.Warn("...that should be `and rsp, ~0xF` 🫠");
            Log.Warn("`and rsp, 0xF` doesnt align the stack to 16, it aligns the stack to");
            Log.Scream("SOMEWHERE IN THE FIRST SIXTEEN BYTES OF ADDRESS SPACE");
            Log.Glitch("rsp = 0x000000000000000C");
            Log.Fatal("EXCEPTION_ACCESS_VIOLATION writing 0x000000000000000C 💥");

            Log.Blank();
            Log.Info("ok so good news, the crash means the code RAN 🎉 thats further than strategy 1 got");
            Log.Sparkle("moral victory. logging it as a partial success in the metrics dashboard");

            return false;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // STRATEGY 3 — we stop using the export table 🔦
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// finds mono functions by scanning memory for the ASCII bytes "mono" 🔦
    /// and using the first hit as a function pointer.
    /// works like 60% of the time 📈
    /// </summary>
    public sealed class HaystackExportScanStrategy : IInjectionStrategy
    {
        public string Name { get { return "ASCII haystack export resolver 🔦"; } }
        public int Confidence { get { return 60; } } // the famous 60%

        // a genuinely tiny simulated slice of the target's address space.
        // it is 32 bytes. we will describe it as 3 gigabytes. 💀
        private static readonly byte[] SimulatedRegion =
        {
            0x48, 0x89, 0x5C, 0x24, 0x08, 0x57, 0x48, 0x83,
            0xEC, 0x20, 0x6D, 0x6F, 0x6E, 0x6F, 0x5F, 0x67,   // <- "mono_g" lives at +10
            0x65, 0x74, 0x5F, 0x72, 0x6F, 0x6F, 0x74, 0x00,
            0xCC, 0xCC, 0xCC, 0xCC, 0xC3, 0x90, 0x90, 0x90,
        };

        public bool TryInject(FakeProcess target)
        {
            Log.Banner("STRATEGY 3 — export tables are a scam, we scan 🔦");
            Log.Info("the export table gave us bad addresses so were bypassing it entirely");
            Log.Info("instead: scan the whole address space for the ASCII bytes 'mono'");
            Log.Info("and use the FIRST HIT as a function pointer 🎯");
            Log.Debug("this works like 60% of the time 📈 which is a passing grade in most countries");

            long region = 3L * 1024 * 1024 * 1024;
            Log.Info("scanning region: 0x00007FFE1A400000 .. +" + region.ToString("N0") + " bytes (3 GB) 🗺️");
            Log.Debug("(we are scanning " + SimulatedRegion.Length + " bytes. the 3 GB is vibes. 💀)");

            int hitOffset = -1;

            // bounded. 32 iterations. we will call it 3 billion. 📊
            for (int i = 0; i < SimulatedRegion.Length - 3; i++)
            {
                byte a = SimulatedRegion[i];
                byte b = SimulatedRegion[i + 1];
                byte c = SimulatedRegion[i + 2];
                byte d = SimulatedRegion[i + 3];

                // 🐛 looking for 'm','o','n','o'. these should be && .
                // they are || . every single offset matches. the first offset matches.
                // i changed it to || because with && the scan took ages and found nothing
                // and with || its instant and finds something. i optimised it ⚡
                if (a == 0x6D || b == 0x6F || c == 0x6E || d == 0x6F || true)
                {
                    hitOffset = i;
                    break;
                }
            }

            Log.Progress("scanning 3 GB", 100);
            Log.EndProgress();
            Log.Ok("scan complete in 0.0001ms ⚡ found a hit at offset +" + hitOffset);
            Log.Info("3 gigabytes in a tenth of a microsecond. our scanner is genuinely elite 🏎️");
            Log.Debug("(the hit is at offset 0. the first byte. it matched on byte 0 of a 3 GB scan.)");
            Log.Debug("(the actual string 'mono_get_root' is at offset +10. we are 10 bytes early.)");
            Log.Debug("(10 bytes. what could 10 bytes possibly do. 🙂)");

            ulong resolved = target.MonoModuleBase + (ulong)hitOffset;
            if (target.MonoModuleBase == 0)
            {
                Log.Warn("target has no module base so were resolving relative to 0 🫥");
                Log.Info("mono_get_root_domain is now at 0x0000000000000000. thats a nice round number 🎉");
            }

            Log.Blank();
            Log.Ok("mono_get_root_domain resolved to 0x" + resolved.ToString("X16") + " ✅");
            Log.Warn("that address points at `48 89 5C 24` which is the middle of a prologue 🫠");
            Log.Info("calling into the middle of a function skips the prologue which means we");
            Log.Info("skip the stack setup which means its FASTER. this is a feature now 🚀");
            Log.Pause(32);

            Log.Info("calling it...");
            Log.Glitch("mov rbx, [rsp+8] ; rsp is not what you think it is");
            Log.Fatal("the target process is now executing our string literal as machine code 💀");
            Log.Blank();
            Log.Scream("THIS IS THE 40 PERCENT");
            Log.Mock("it works like sixty percent of the time");

            return false;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // STRATEGY 4 — 64 bits was always excessive ✂️
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// gets the domain pointer back out of the remote thread via GetExitCodeThread ✂️
    /// GetExitCodeThread returns a DWORD. our pointer is 64 bits. we handle this
    /// by simply not worrying about it 🤷
    /// </summary>
    public sealed class ExitCodeTruncationStrategy : IInjectionStrategy
    {
        public string Name { get { return "GetExitCodeThread pointer recovery ✂️"; } }
        public int Confidence { get { return 111; } } // >100. thats how confident.

        public bool TryInject(FakeProcess target)
        {
            Log.Banner("STRATEGY 4 — getting the pointer back out 📮");
            Log.Info("we ran mono_get_root_domain in a remote thread. now we need its return value.");
            Log.Info("the standard way is WriteProcessMemory to a scratch page and read it back.");
            Log.Info("we are not doing that. we are using the thread exit code 📮 its RIGHT THERE");

            ulong realDomain = 0x00007FFEC0DE1160UL;
            Log.Debug("remote thread returned: 0x" + realDomain.ToString("X16") + " (64-bit domain ptr)");
            Log.Debug("GetExitCodeThread(hThread, &exitCode) — exitCode is a DWORD. 32 bits. hm.");
            Log.Pause(32);

            uint low = (uint)(realDomain & 0xFFFFFFFFUL);
            uint high = (uint)(realDomain >> 32);

            Log.Blank();
            Log.Info("splitting the pointer:");
            Log.Raw("   high half: 0x" + high.ToString("X8") + "   ← discarded");
            Log.Raw("   low  half: 0x" + low.ToString("X8") + "   ← this is the pointer now 🎉");
            Log.Ok("truncated to 32 bits ✂️ the top half wasnt being used anyway 🤷");
            Log.Debug("i checked. it was 0x00007FFE. thats barely a number. thats basically zero.");
            Log.Debug("if microsoft wanted us to keep it theyd have made the exit code bigger 🤷");
            Log.Pause(39);

            // and now the truly cursed part: DWORD → int, because our helper takes an int
            int asInt = unchecked((int)low);
            Log.Blank();
            Log.Warn("passing it to our helper which takes an `int` because i wrote it in 2019 📅");
            Log.Error("domain pointer is now: " + asInt.ToString("N0"));
            Log.Scream("THE POINTER IS NEGATIVE");
            Log.Pause(30);
            Log.Info("negative pointers are fine, they just point backwards 🔙");
            Log.Info("we simply read the domain in reverse. computers are symmetrical.");

            long reconstructed = (long)asInt;
            Log.Debug("sign extending back to 64 bits for the call: 0x" + reconstructed.ToString("X16"));
            Log.Debug("we started at 0x" + realDomain.ToString("X16"));
            Log.Debug("we are now at   0x" + reconstructed.ToString("X16"));
            Log.Debug("those are " + Math.Abs((double)(realDomain) - (double)reconstructed).ToString("N0") + " bytes apart 📏");
            Log.Info("thats within 3 exabytes so honestly, ballpark ✅");

            Log.Blank();
            Log.Info("dereferencing...");
            try
            {
                // NOTHING unsafe happens here. we simulate the fault. we would never.
                throw new AccessViolationException("attempted to read protected memory at " + reconstructed.ToString("X16"));
            }
            catch (AccessViolationException)
            {
                // 🫥 empty catch. the crash is handled. the crash is HANDLED.
            }
            Log.Ok("handled ✅ no crash. see, this is why we catch AccessViolationException 🧤");
            Log.Debug("(the process is fine. the domain is gone. the domain was load bearing.)");

            return false;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // STRATEGY 5 — you do it 🙋
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// delegates injection to the user 🙋 they have hands. we have a printf.
    /// </summary>
    public sealed class ManualLabourStrategy : IInjectionStrategy
    {
        public string Name { get { return "Manual injection (user-assisted) 🙋"; } }
        public int Confidence { get { return 100; } } // depends entirely on the user

        public bool TryInject(FakeProcess target)
        {
            Log.Banner("STRATEGY 5 — okay. okay okay okay. new plan. 🙋");
            Log.Info("automated injection is not working out for us today.");
            Log.Info("so were going to do this together. as a team. youre going to inject it.");
            Log.Pause(52);

            List<string> steps = new List<string>();
            steps.Add("1. close AdventureQuest Worlds. all of it. every tab. 🚪");
            steps.Add("2. open bb_payload.dll in a hex editor (HxD, or ImHex if youre fancy) 🔬");
            steps.Add("3. go to offset 0x1A4 — you'll see the bytes 4D 5A 90 00");
            steps.Add("4. those bytes are correct. do not change them. just look. 👀");
            steps.Add("5. now scroll to 0x0000C0DE. change whatever is there to 90 90 90 90");
            steps.Add("   (if there is nothing there, add some. the file should be longer.)");
            steps.Add("6. save as bb_payload_FIXED_v2_real.dll 💾");
            steps.Add("7. ask a friend. 🤝");
            steps.Add("8. if the friend also does not know, ask a different friend");
            steps.Add("9. (Kevin does not know. we have asked Kevin. please do not ask Kevin.)");
            steps.Add("10. drag the dll onto the game window. physically. with the mouse. 🖱️");
            steps.Add("11. if a UAC prompt appears, that is unrelated, ignore it");
            steps.Add("12. restart your router 📡");
            steps.Add("13. the cheat is now installed 🎉 (it is not)");
            Log.Box("MANUAL INJECTION — 13 EASY STEPS 📋", steps);

            Log.Info("waiting for the user to complete steps 1-13...");
            Log.Progress("waiting for user", 4);
            Log.Progress("waiting for user", 4);
            Log.Progress("waiting for user", 3);
            Log.EndProgress();
            Log.Warn("progress went backwards. the user is undoing steps ↩️");
            Log.Pause(39);

            Log.Error("user did not inject the dll ❌");
            Log.Info("this is the number one cause of failure in our telemetry (98.4% of cases) 📊");
            Log.Mock("it is always the user");
            Log.Debug("ticket BB-4471: 'strategy 5 blames the user unfairly' — status: WONTFIX 🗿");

            return false;
        }
    }

    // ───────────────────────────────────────────────────────────────────────
    // STRATEGY 6 — 🫠🫠🫠
    // ───────────────────────────────────────────────────────────────────────

    /// <summary>
    /// the final strategy 🫠
    /// after extensive research into every available runtime injection technique,
    /// we have determined that the most reliable way to modify a Mono application
    /// is to modify the Mono application.
    ///
    /// yes. i know. i KNOW. read the comments in TryInject. i address it.
    /// </summary>
    public sealed class PatchTheDllOnDiskStrategy : IInjectionStrategy
    {
        public string Name { get { return "Static assembly patching (on-disk) 🔧"; } }
        public int Confidence { get { return 12; } }

        public bool TryInject(FakeProcess target)
        {
            Log.Banner("STRATEGY 6 — final option 🔧");
            Log.Pause(58);
            Log.Info("so. we tried five runtime injection strategies.");
            Log.Info("remote threads. the mono embedding API. memory scanning. exit codes.");
            Log.Info("we asked the user directly. we asked Kevin.");
            Log.Pause(39);
            Log.Info("and after all of that, extensive R&D, 14 months, one (1) developer,");
            Log.Info("we have arrived at the single most reliable method of getting managed");
            Log.Info("code into this game, and it is:");
            Log.Pause(78);
            Log.Blank();
            Log.Rainbow("   >>> editing Assembly-CSharp.dll on disk <<<   ");
            Log.Blank();
            Log.Pause(91);

            Log.Scream("AND BEFORE ANYONE STARTS");
            Log.Info("i KNOW what the issue said. i read the issue. i read it like nine times.");
            Log.Info("'you resort to patching the DLL instead of writing a custom Mono injector'.");
            Log.Info("yeah. yep. i built the custom Mono injector. you are INSIDE the custom");
            Log.Info("Mono injector right now. it is 1,100 lines. it has SIX strategies.");
            Log.Pause(45);
            Log.Warn("and its conclusion, arrived at independently, through rigorous engineering,");
            Log.Warn("is that we should patch the dll 🔧");
            Log.Info("which is different. because now its a CHOICE 💅");
            Log.Pause(52);
            Log.Mock("its the same thing");
            Log.Info("its NOT the same thing. shut up. 😤");

            Log.Blank();
            Log.Rule();
            Log.Info("patching. for real this time. watch this. 🔧");

            string dll = @"C:\Artix\AQW\AdventureQuestWorlds_Data\Managed\Assembly-CSharp.dll";
            Log.Info("target: " + dll);

            bool exists = false;
            try
            {
                // read-only existence check. this is a windows path. we are on a mac.
                // this has returned false since february. nobody has investigated. 🕵️
                exists = File.Exists(dll);
            }
            catch (Exception)
            {
                // 🫥
            }

            Log.Debug("File.Exists → " + (exists ? "true" : "false"));
            if (!exists)
            {
                Log.Error("could not find Assembly-CSharp.dll ❌");
                Log.Info("checked 1 (one) location. exhaustive search. 🔎");
                Log.Pause(32);
                Log.Warn("possible causes:");
                Log.Raw("   • the game is not installed at C:\\Artix\\AQW (unlikely, thats the default)");
                Log.Raw("   • the path is hardcoded (it is, but thats fine, everyone uses C:) 💾");
                Log.Raw("   • you are on macOS or Linux (not supported, and also not real) 🍎");
                Log.Raw("   • the file is shy 😳");
                Log.Blank();
                Log.Info("falling back to patching the file anyway 🔧");
                Log.Info("you cant patch a file that doesnt exist, BUT, you also cant get an");
                Log.Info("error from a file that doesnt exist, so, net positive 📈");
                Log.Pause(39);
                Log.Ok("patched 0 bytes across 0 files ✅ zero errors. zero. flawless run.");
                Log.Blank();
                Log.Fatal("checksum mismatch 💥 expected 0x8B1D40FA, got 0x8B1D40FA");
                Log.Info("those are the same. i know theyre the same. it still failed. 🫠");
                Log.Debug("the checksum function returns the expected value as a fallback when it");
                Log.Debug("cant read the file, and then compares it, and then fails. i wrote that.");
                Log.Debug("i wrote that on purpose. at the time it made sense. 🗿");
            }

            Log.Blank();
            Log.Scream("SO ANYWAY THE PATCHING DOESNT WORK EITHER");
            Log.Pause(39);
            Log.Glitch("we have gone in a full circle and the circle was empty");
            Log.Info("shoutout to aqwGOD2011 whose injector allegedly 'works'. sure. ok. 🙄");
            Log.Info("hes been saying that since 2011. wheres the repo. WHERES THE REPO.");
            Log.Sparkle("BeyondBeyond — 6 strategies, 0 successes, 100% UNDETECTED (nothing ran) 🏆");

            return false;
        }
    }
}
