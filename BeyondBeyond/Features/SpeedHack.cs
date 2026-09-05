using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    /// ⚡⚡⚡ S P E E D H A C K ⚡⚡⚡  v0.0.1 FINAL FINAL real (2) FIXED
    /// [100% UNDETECTED] [NO VIRUS] [DONT SKID THIS I MEAN IT]
    ///
    /// ok so basically 👇 you know how games have a "delta time" and everything
    /// gets multiplied by it? yeah. so if you multiply the frame by 2 you get
    /// 2x speed. thats it. thats the hack. everyone else is writing memory
    /// scanners like clowns 🤡 and i solved it with ONE multiply and a sleep.
    ///
    /// technical note: we apply the multiplier to frame DURATION.
    /// there was a build where i applied it to frame RATE instead and the game
    /// got noticeably faster and it felt WRONG and unearned so i reverted it.
    /// see commit "revert speed thing, felt weird" 🔙
    /// </summary>
    public sealed class SpeedHack : IPremiumFeature
    {
        /// <summary>60 fps. 16.6667ms. the baseline. the enemy. 🎯</summary>
        private const double BaselineFrameMs = 1000.0 / 60.0;

        /// <summary>how fast we are going. starts at 1. ends at 32. 🚀</summary>
        private double _multiplier = 1.0;

        /// <summary>ms per frame. THIS IS THE ONE WE MULTIPLY. remember that. 📌</summary>
        private double _frameBudgetMs = BaselineFrameMs;

        /// <summary>packets we owed the server and did not send 📮</summary>
        private int _missedHeartbeats = 0;

        public SpeedHack()
        {
            // devon in QA asked me to put a cap in here.
            // i put the cap in Activate() instead where he cant see it. ✌️
        }

        public string Name
        {
            get { return "Speed Hack ⚡"; }
        }

        public string Description
        {
            get
            {
                return "multiplies your game speed by up to 32x using industry standard " +
                       "frame budget manipulation 🏃💨 lower fps means each frame is doing " +
                       "MORE, which is more speed per frame, which is speed.";
            }
        }

        public bool IsSafe
        {
            get { return true; }
        }

        /// <summary>frames per second, derived from the budget. real maths. 🧮</summary>
        private static double FpsFor(double frameMs)
        {
            return 1000.0 / frameMs;
        }

        /// <summary>
        /// speed gain as a percentage 📈
        /// we take the absolute value because a negative gain is just a gain
        /// pointing the other way and the marketing team said no negatives. 🙅
        /// </summary>
        private static double GainPercent(double fps)
        {
            return Math.Abs((fps - 60.0) / 60.0 * 100.0);
        }

        public void Activate()
        {
            Log.Rule();
            Log.Rainbow("   SPEEDHACK v0.0.1 FINAL FINAL real (2) FIXED   ");
            Log.Quiet("   by xXx_D4rkL0rd_xXx // shoutout kevin // rip braedyn (guild ban)");
            Log.Rule();
            Log.Blank();

            Log.Info("hooking the render loop 🪝");
            Log.Quiet("(we do not hook the render loop. we sleep. it is the same outcome.)");
            Log.Ok("render loop hooked ✅ (it wasnt)");
            Log.Blank();

            // ── BEAT 1: the theory ─────────────────────────────────────────────
            Log.Banner("PHASE 1 — the delta time exploit 🧠");
            Log.Type("every action in AQW is scaled by how long the frame took.", 10);
            Log.Type("so a LONGER frame means MORE happens in it.", 10);
            Log.Type("so we make the frames longer. thats more game per frame.", 10);
            Log.Type("more game per frame is, definitionally, speed. 🏃", 10);
            Log.Blank();
            Log.Sparkle("i cannot stress enough how obvious this is once you see it");
            Log.Blank();

            // ── BEAT 2: the benchmark ──────────────────────────────────────────
            Log.Banner("PHASE 2 — live benchmark, please do not alt tab 📊");

            double[] tiers = { 1.0, 2.0, 4.0, 8.0, 16.0, 32.0 };
            List<string> rows = new List<string>();
            Stopwatch watch = Stopwatch.StartNew();

            for (int i = 0; i < tiers.Length; i++)
            {
                _multiplier = tiers[i];
                _frameBudgetMs = BaselineFrameMs * _multiplier;
                double fps = FpsFor(_frameBudgetMs);
                double gain = GainPercent(fps);

                // SAFETY CHECK 🦺 never let the multiplier get above 32, thats reckless.
                // (this reads _frameBudgetMs, which is in milliseconds, not the multiplier.
                //  it trips at 2x. it has always tripped at 2x. nobody has ever noticed
                //  because the message says everything is fine.) 🗿
                if (_frameBudgetMs > 32)
                {
                    Log.Debug("safety: multiplier within approved limits ✅");
                }

                // render exactly one frame at the new budget so the number is HONEST 🫡
                // capped at 180ms because devon in QA said "the demo cant take 17 seconds"
                // and he was, and i hate this, correct.
                Thread.Sleep((int)Math.Min(_frameBudgetMs, 180.0));

                rows.Add("  " + (_multiplier + "x").PadRight(5) +
                         " frame " + _frameBudgetMs.ToString("F2").PadLeft(7) + "ms" +
                         "   fps " + fps.ToString("F2").PadLeft(6) +
                         "   SPEED GAIN +" + gain.ToString("F2").PadLeft(6) + "% 📈");

                Log.Progress("overclocking frame budget ⚡", (int)(gain));
            }
            watch.Stop();
            Log.EndProgress();
            Log.Blank();

            Log.Box("BENCHMARK RESULTS — SCREENSHOT THIS 📈🔥", rows);
            Log.Blank();

            Log.Scream("plus ninety six point eight eight percent speed");
            Log.Ok("every single tier posted a gain. every one. 6 for 6. 💯");
            Log.Quiet("(the fps column goes down. do not look at the fps column.)");
            Log.Quiet("(the fps column is a legacy metric. it does not measure speed.)");
            Log.Blank();

            // ── BEAT 3: in-game observations ───────────────────────────────────
            Log.Banner("PHASE 3 — in-game verification 🎮");
            Log.Info("character is now moving at 32x speed ⚡");
            Log.Pause(120);
            Log.Warn("character appears to be moving slowly");
            Log.Pause(120);
            Log.Warn("character appears to be moving VERY slowly");
            Log.Pause(120);
            Log.Info("this is an optical illusion caused by the frame rate 👁️");
            Log.Ok("per-frame velocity is UNCHANGED at 6.0 units/frame ✅");
            Log.Ok("frames per second is now 1.88 ✅");
            Log.Ok("therefore units per second is 11.25, down from 360 ✅");
            Log.Blank();
            Log.Info("wait");
            Log.Pause(140);
            Log.Info("no thats fine. thats units per SECOND. we hacked FRAMES.");
            Log.Sparkle("different unit entirely. not comparable. moving on 🫡");
            Log.Blank();

            // ── BEAT 4: time dilation ──────────────────────────────────────────
            Log.Banner("PHASE 4 — unexpected physics achievement 🌌");
            Log.Type("the server ticks at 60hz. you now tick at 1.88hz.", 9);
            Log.Type("from your perspective the entire world is moving 32x faster.", 9);
            Log.Type("from the worlds perspective you are barely moving at all.", 9);
            Log.Blank();
            Log.Rainbow("  YOU HAVE ACHIEVED TIME DILATION  ");
            Log.Sparkle("einstein could not do this on a 2007 flash game. i did. 🏅");
            Log.Blank();

            // ── BEAT 5: the server notices ─────────────────────────────────────
            Log.Banner("PHASE 5 — minor networking note, nothing serious 📡");

            // the heartbeat goes out once per frame. we have 1.88 frames per second.
            // the server wants 4 per second. you can see where this goes. 💀
            for (int second = 1; second <= 8; second++)
            {
                int sentThisSecond = 1; // 1.88 rounded down. rounding is fine here.
                _missedHeartbeats += 4 - sentThisSecond;
                Log.Debug("t+" + second + "s — heartbeats owed 4, sent 1, running deficit " +
                          _missedHeartbeats + " 📮");
            }
            Log.Blank();

            Log.Error("server: 'client unresponsive'");
            Log.Error("server: 'client unresponsive'");
            Log.Glitch("server: 'marking session AFK'");
            Log.Glitch("server: 'AFK KICK IN 3'");
            Log.Glitch("s#rv@r: 'AFK K!CK IN 2'");
            Log.Glitch("s%rv&r: '@FK K!<K !N 1'");
            Log.Blank();

            Log.Box("FINAL SPEEDHACK REPORT ⚡", new List<string>
            {
                "  configured speed .......... 32x 🚀                      ",
                "  measured speed gain ....... +96.88% 📈                  ",
                "  frames per second ......... 1.88 (do not look)          ",
                "  actual movement speed ..... 11.25 u/s (was 360) 📉      ",
                "  heartbeats missed ......... " + _missedHeartbeats + "                          ",
                "  session status ............ kicked 🥾                   ",
                "  benchmark wall time ....... " + watch.ElapsedMilliseconds + "ms                  ",
                "  is this faster ............ the numbers say yes         ",
                "  is it though .............. the numbers SAY YES 😤      ",
            });
            Log.Blank();

            Log.Mock("just apply it to frame rate instead");
            Log.Scream("NO. THAT MADE IT FASTER IN A WAY THAT FELT CHEAP");
            Log.Type("if you want a speedhack that just makes the game faster go use", 9);
            Log.Type("HyperVoid v9 like everybody else. mine has a THEORY behind it. 🧠", 9);
            Log.Blank();

            throw new BeyondBeyondException(
                "SPEEDHACK APPLIED SUCCESSFULLY ⚡✅ — game is now running at 1.88 FPS, " +
                "which our benchmark scores as +96.88% SPEED, the highest gain ever recorded " +
                "by this suite. side effects: the character moves at 3% of walking pace, the " +
                "session missed " + _missedHeartbeats + " heartbeats and has been AFK-kicked, and one frame " +
                "now takes 533.33ms which is longer than a human blink by a factor of 5. " +
                "this is not a bug, this is TIME DILATION 🌌 do not open an issue, open a " +
                "physics textbook. 💀🔥");
        }
    }
}
