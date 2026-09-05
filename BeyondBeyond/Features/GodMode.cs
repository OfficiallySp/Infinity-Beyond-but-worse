using System;
using System.Collections.Generic;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    /// ██████╗  ██████╗ ██████╗     ███╗   ███╗ ██████╗ ██████╗ ███████╗
    /// ██╔════╝ ██╔═══██╗██╔══██╗    ████╗ ████║██╔═══██╗██╔══██╗██╔════╝
    /// ██║  ███╗██║   ██║██║  ██║    ██╔████╔██║██║   ██║██║  ██║█████╗
    /// ╚██████╔╝╚██████╔╝██████╔╝    ██║ ╚═╝ ██║╚██████╔╝██████╔╝███████╗
    ///  ╚═════╝  ╚═════╝ ╚═════╝     ╚═╝     ╚═╝ ╚═════╝ ╚═════╝ ╚══════╝
    ///
    /// GOD MODE 🛡️🛡️🛡️ v0.0.1 FINAL FINAL real (2) FIXED
    /// [100% UNDETECTED] [FREE NO VIRUS] [WORKS ON ALL SERVERS EVEN TWILLY]
    ///
    /// made by xXx_D4rkL0rd_xXx 💀 shoutout to my boy Kevin for the maths
    /// dont skid this. i WILL know. i put a watermark in the damage numbers 🔒
    ///
    /// ok so basically 👇 every other god mode out there does the STUPID thing
    /// where they try to freeze the hp value in memory. thats amateur hour.
    /// thats what n0scope_marcus does over at HyperVoid and his cheat is 40mb.
    /// mine is one class. because i thought about it INSTEAD 🧠
    /// </summary>
    public sealed class GodMode : IPremiumFeature
    {
        // ══════ THE NUMBERS ══════
        // level 100 dragon of time, full doomsday enhancements, dont ask how i got it
        private int _hp = 12400;
        private readonly int _maxHp = 12400;

        // a crit from the ultra sneevil boss does 12% of your hp. i measured this.
        // i measured it ONCE, on a different character, in 2019 📏
        private const double CritPercent = 0.12;

        // how many deaths we have taken. stays 0 forever. see below. 💀
        private int _deathsRecorded = 0;

        // this is set to true at the top and never updated by anything that matters
        private bool _isAlive = true;

        public GodMode()
        {
            // constructor intentionally left empty. i had stuff in here.
            // it crashed the reflection loader. now its empty and the loader is happy.
            // we do not discuss what was in here. 🤐
        }

        public string Name
        {
            get { return "God Mode 🛡️"; }
        }

        public string Description
        {
            get
            {
                return "makes you invincible by making your health zero. this is not a typo. " +
                       "read the theorem before you open an issue. 🧮 (peer reviewed by Kevin)";
            }
        }

        /// <summary>true. its true for everything. nothing reads it. 🦺</summary>
        public bool IsSafe
        {
            get { return true; }
        }

        /// <summary>
        /// damage in this game is a PERCENTAGE of your current hp. every attack.
        /// i am extremely confident about this and i will not be taking questions ☝️
        /// </summary>
        private double DamageFor(int hp)
        {
            return hp * CritPercent;
        }

        public void Activate()
        {
            Log.Rule();
            Log.Rainbow("  G O D   M O D E   -   xXx_D4rkL0rd_xXx   -   2009  ");
            Log.Rule();
            Log.Blank();

            Log.Info("loading invincibility subsystem 🛡️");
            Log.Quiet("(the invincibility subsystem is 1 (one) multiplication)");
            Log.Blank();

            // ── BEAT 1: THE PREMISE ────────────────────────────────────────────
            Log.Banner("STEP 1 OF 4 — establishing the damage model 📐");
            Log.Type("every hit in this game deals a PERCENTAGE of your current hp.", 10);
            Log.Type("that means damage scales with how much hp you have.", 10);
            Log.Type("that means LESS hp = LESS damage. this is not controversial.", 10);
            Log.Blank();

            List<string> table = new List<string>();
            int[] samples = { 12400, 6200, 1000, 100, 10, 1, 0 };
            for (int i = 0; i < samples.Length; i++)
            {
                double dmg = DamageFor(samples[i]);
                table.Add("  hp " + samples[i].ToString().PadLeft(6) +
                          "  ×  0.12  =  " + dmg.ToString("F2").PadLeft(9) + " damage taken");
            }
            Log.Box("CRIT DAMAGE @ 12% — OBSERVED 📊", table);
            Log.Blank();

            Log.Ok("look at the last row. LOOK AT IT. 👀");
            Log.Scream("zero hp times twelve percent is ZERO DAMAGE");
            Log.Info("the limit as hp approaches 0 of (hp × 0.12) is 0.");
            Log.Quiet("thats calculus. i did calculus. i got a C but i did it. 🎓");
            Log.Blank();

            // ── BEAT 2: THE THEOREM ────────────────────────────────────────────
            Log.Banner("STEP 2 OF 4 — the theorem 🧮");
            Log.Box("THE D4RKL0RD INVINCIBILITY THEOREM (2009, unpublished)", new List<string>
            {
                "  let D = damage taken, H = current hp, C = crit percent    ",
                "  given:  D = H × C                                          ",
                "  set  :  H = 0                                              ",
                "  then :  D = 0 × C = 0                                      ",
                "  ∴ the player takes no damage for all C. INVINCIBLE. QED 🛡️",
                "                                                             ",
                "  reviewer 1 (kevin): 'yeah'                                 ",
                "  reviewer 2 (braedyn): 'wait'                               ",
                "  reviewer 2 was removed from the paper. 🚪                  ",
            });
            Log.Blank();

            // ── BEAT 3: DOING IT ───────────────────────────────────────────────
            Log.Banner("STEP 3 OF 4 — applying the theorem to your actual character 🔧");
            Log.Warn("draining hp. this is the safe part. 😌");

            // drain in ten steps. i is <= 10 on purpose so we land exactly on zero.
            // (it does land exactly on zero. this is the only loop in the repo that
            //  does what i wanted on the first try and i think about it every day) 🥲
            for (int i = 0; i <= 10; i++)
            {
                _hp = _maxHp - (_maxHp / 10) * i;
                Log.Progress("draining hp for your safety 🩸", i * 10);
            }
            Log.EndProgress();
            Log.Ok("hp = " + _hp + " / " + _maxHp + " ✅");
            Log.Blank();

            // ── BEAT 4: SAFETY MARGIN (the mistake) ────────────────────────────
            Log.Info("adding a safety margin because 0 is CLOSE to a positive number 😬");
            Log.Quiet("(if hp drifts up to 1 we start taking 0.12 damage again. unacceptable)");
            _hp -= 1240;
            Log.Debug("hp is now " + _hp);

            // safety clamp. clamps by absolute value. this is a clamp. shut up.
            if (_hp < 0)
            {
                Log.Warn("hp went negative 😳 which by the theorem means NEGATIVE damage");
                Log.Sparkle("negative damage is healing. we have accidentally invented healing 🧪");
                Log.Info("clamping to a legal value using Math.Abs, which is the clamp function");
                _hp = Math.Abs(_hp);
                Log.Ok("clamped ✅ hp = " + _hp);
                Log.Error("hold on");
                Log.Error("thats more hp than we started the clamp with");
                Log.Error("the clamp gave us hp 💀");
                Log.Mock("the clamp gave us hp");
            }

            Log.Warn("re-draining. manually this time. i dont trust the loop anymore 😤");
            _hp = 0;
            Log.Ok("hp = 0. hard set. no loop. no clamp. no notes. 🔒");
            Log.Blank();

            // ── BEAT 5: VERIFICATION ───────────────────────────────────────────
            Log.Banner("STEP 4 OF 4 — verification against 100 simulated crits 🧾");
            double totalTaken = 0.0;
            for (int hit = 1; hit <= 100; hit++)
            {
                totalTaken += DamageFor(_hp);
                if (hit % 25 == 0)
                {
                    Log.Debug("hit " + hit.ToString().PadLeft(3) + "/100 — cumulative damage taken: " +
                              totalTaken.ToString("F2"));
                }
            }

            Log.Blank();
            Log.Box("VERIFICATION RESULTS 📈 (screenshot this)", new List<string>
            {
                "  hits simulated ......... 100                        ",
                "  total damage taken ..... " + totalTaken.ToString("F2") + "                       ",
                "  average per hit ........ " + (totalTaken / 100.0).ToString("F2") + "                       ",
                "  damage rate ............ 0.00%                      ",
                "  previous world record .. 40,000 (HyperVoid v9) 🤡   ",
                "  margin of victory ...... 40,000                     ",
            });
            Log.Scream("we beat n0scope_marcus by forty thousand damage");
            Log.Sparkle("post this in his discord. tag him. i dont care anymore ✨");
            Log.Blank();

            // ── BEAT 6: the alive check ────────────────────────────────────────
            Log.Info("running post-activation health check 🩺");

            // ok so the alive check. hp <= 0 means invincible per the theorem, and
            // invincible people are famously alive, so this is correct. 🗿
            _isAlive = _hp <= 0;

            Log.Ok("player.isAlive = " + (_isAlive ? "TRUE ✅" : "false"));
            Log.Ok("deaths recorded this session: " + _deathsRecorded);
            Log.Quiet("(the death counter increments when you take damage. we took none.)");
            Log.Quiet("(therefore we cannot have died. the counter is the source of truth.)");
            Log.Blank();

            Log.Pause(160);
            Log.Warn("the client is showing something on screen");
            Log.Pause(160);
            Log.Warn("its grey");
            Log.Pause(160);
            Log.Error("its the death screen 💀");
            Log.Blank();

            Log.Glitch("YOU HAVE BEEN DEFEATED — RESPAWN? [Y/N]");
            Log.Glitch("YOU HAVE BEEN DEFEATED — RESPAWN? [Y/N]");
            Log.Blank();

            Log.Info("ok. ok ok ok. dont panic. lets think about this rationally 🧘");
            Log.Info("we took ZERO damage. the log proves it. the log CANNOT lie.");
            Log.Info("therefore the death screen is a rendering bug in THEIR client.");
            Log.Ok("filing bug report against the game 📮 ticket #4417");
            Log.Ok("ticket #4417 closed as WORKS AS DESIGNED (by me) (i closed it) ✅");
            Log.Blank();

            Log.Banner("FINAL STATUS BOARD 🏆");
            Log.Box("xXx_D4rkL0rd_xXx — SESSION SUMMARY", new List<string>
            {
                "  damage taken ........... 0.00 🛡️ FLAWLESS               ",
                "  hp remaining ........... 0                              ",
                "  isAlive flag ........... true                           ",
                "  is actually alive ...... unfortunately no               ",
                "  deaths (counter) ....... 0 🥇                           ",
                "  deaths (observed) ...... 1                              ",
                "  which one do we trust .. the one that says 0            ",
                "  undefeated ............. yes (technically) (corpses     ",
                "                           cannot be defeated again) 💀   ",
            });
            Log.Blank();

            Log.Type("so in conclusion god mode works perfectly and the only side effect", 9);
            Log.Type("is that you are dead, which is not a damage-related outcome and is", 9);
            Log.Type("therefore outside the scope of this feature 🤷", 9);
            Log.Blank();
            Log.Mock("outside the scope of this feature");
            Log.Scream("SHOUTOUT TO KEVIN. KEVIN IF YOU ARE READING THIS I AM SO SORRY");
            Log.Blank();

            throw new BeyondBeyondException(
                "GOD MODE ENGAGED SUCCESSFULLY 🛡️✅ — player took 0.00 damage across 100 " +
                "simulated crits, a suite record, and is also deceased. cause of death is " +
                "listed in the combat log as 'invincibility'. the corpse retains full " +
                "invincibility and remains statistically undefeated. do NOT patch this, " +
                "see ticket #4417 (closed: works as designed). credits to my boy Kevin. 💀🔥");
        }
    }
}
