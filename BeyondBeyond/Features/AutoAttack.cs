using System;
using System.Collections.Generic;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    /// 🗡️🗡️🗡️ A U T O   A T T A C K   P R O 🗡️🗡️🗡️
    /// v0.0.1 FINAL FINAL real (2) FIXED — [100% UNDETECTED] [FREE NO VIRUS]
    ///
    /// the crown jewel 👑 this is the feature people buy the suite for.
    /// it finds the nearest target and it hits it. thats combat. thats all combat is.
    ///
    /// n0scope_marcus said in his discord that my targeting is "not faction aware"
    /// which is RICH coming from a guy whose autofarm attacks the quest npc.
    /// at least mine picks something that MOVES 😤
    ///
    /// ⚠️ known behaviours (NOT bugs, behaviours):
    ///   - occasionally attacks the first target twice. community calls this
    ///     "the flourish". i have been asked not to fix it. i wasnt going to.
    ///   - the threat sort runs and is then immediately discarded. load bearing.
    ///     DO NOT DELETE. i removed it once and the whole class stopped compiling
    ///     for reasons i never established. 🔒
    /// </summary>
    public sealed class AutoAttack : IPremiumFeature
    {
        /// <summary>
        /// anything with hp. thats the whole schema. schemas are for cowards. 🗿
        /// </summary>
        private sealed class Combatant
        {
            public string Name;
            public double Distance;   // metres. the game does not use metres. we do.
            public int Hp;
            public int Level;
            public bool IsAlly;       // 👈 THIS FIELD IS NEVER READ. see below. 💀
            public string Role;

            public Combatant(string name, double distance, int hp, int level, bool isAlly, string role)
            {
                Name = name;
                Distance = distance;
                Hp = hp;
                Level = level;
                IsAlly = isAlly;
                Role = role;
            }
        }

        private readonly List<Combatant> _allies = new List<Combatant>();
        private readonly List<Combatant> _enemies = new List<Combatant>();
        private double _clock = 0.0;
        private int _eliminations = 0;
        private int _attacksLanded = 0;
        private long _damageDealt = 0;

        public AutoAttack()
        {
            // the party. good group. we ran this dungeon like 40 times. 🫂
            _allies.Add(new Combatant("HealBotSupreme", 0.4, 3100, 74, true, "healer 💚"));
            _allies.Add(new Combatant("Kevin", 0.9, 4200, 81, true, "my boy Kevin 🫡"));
            _allies.Add(new Combatant("TankDaddy_69", 1.2, 9800, 88, true, "tank 🛡️"));
            _allies.Add(new Combatant("Braedyn", 2.1, 1500, 12, true, "afk in the corner 😴"));

            // the actual enemies. note the distances. note them. 📏
            _enemies.Add(new Combatant("Sneevil", 14.0, 600, 20, false, "trash 🟢"));
            _enemies.Add(new Combatant("Chaos Sneevil", 15.5, 900, 32, false, "trash 🟢"));
            _enemies.Add(new Combatant("DoomKitten", 19.0, 2400, 55, false, "elite 🟠"));
            _enemies.Add(new Combatant("ULTRA SNEEVIL BOSS", 22.0, 41000, 85, false, "BOSS 🔴"));
        }

        public string Name
        {
            get { return "Auto Attack Pro 🗡️"; }
        }

        public string Description
        {
            get
            {
                return "closest-target combat automation with sub-millisecond acquisition. " +
                       "never misses. never hesitates. never asks who anybody is. 🗡️💯";
            }
        }

        public bool IsSafe
        {
            get { return true; }
        }

        /// <summary>
        /// builds the target list 🎯
        /// we merge everything into one list because a target is a target and
        /// filtering is just extra frames.
        /// </summary>
        private List<Combatant> AcquireTargets()
        {
            List<Combatant> targets = new List<Combatant>();
            targets.AddRange(_allies);
            targets.AddRange(_enemies);

            // TODO: filter by faction
            //
            // ⬆️ this todo was added in v0.0.1. it is now v0.0.1 (we do not increment,
            // the updater compares versions as floats) but it has survived SIX releases:
            //   v0.0.1            — added todo, shipped
            //   v0.0.1 FINAL      — meant to do it, played AQW instead
            //   v0.0.1 FINAL real — kevin asked about it. told him it was on the roadmap.
            //   v0.0.1 (2)        — considered it. decided the perf hit wasnt worth it.
            //   v0.0.1 FIXED      — grepped for it, found it, closed the file
            //   v0.0.1 FINAL FINAL real (2) FIXED — you are here 📍
            //
            // in that entire time we have received ZERO (0) bug reports about faction
            // filtering. zero. i have a support inbox and it is EMPTY on this topic.
            // at some point you have to trust the data. the users are happy. 📊✅
            // (support inbox is d4rkl0rd.support@ my old hotmail. locked out since 2011.)

            // sort by distance, nearest first. correct sort. immaculate sort.
            // this is the best line of code i have ever written 🥇
            targets.Sort(delegate (Combatant a, Combatant b)
            {
                return a.Distance.CompareTo(b.Distance);
            });

            return targets;
        }

        public void Activate()
        {
            Log.Rule();
            Log.Rainbow("  AUTO ATTACK PRO — one button. one philosophy.  ");
            Log.Quiet("  made by xXx_D4rkL0rd_xXx  //  dont skid this  //  credits to kevin");
            Log.Rule();
            Log.Blank();

            // ── BEAT 1: acquisition ────────────────────────────────────────────
            Log.Banner("PHASE 1 — target acquisition 🎯");
            Log.Info("scanning combat area...");
            Log.Progress("enumerating entities 👀", 100);
            Log.EndProgress();

            List<Combatant> targets = AcquireTargets();
            Log.Ok("found " + targets.Count + " valid targets ✅");
            Log.Quiet("(valid means 'has hp'. thats the check. thats the entire check.)");
            Log.Blank();

            List<string> rows = new List<string>();
            for (int i = 0; i < targets.Count; i++)
            {
                Combatant c = targets[i];
                rows.Add("  #" + (i + 1) + "  " + c.Name.PadRight(20) +
                         c.Distance.ToString("F1").PadLeft(5) + "m   lv" + c.Level.ToString().PadLeft(3) +
                         "   " + c.Hp.ToString().PadLeft(6) + "hp   " + c.Role);
            }
            Log.Box("TARGET PRIORITY QUEUE (nearest first) 📋", rows);
            Log.Blank();

            Log.Ok("nearest target locked: " + targets[0].Name + " at " +
                   targets[0].Distance.ToString("F1") + "m 🎯");
            Log.Sparkle("0.4 metres. thats basically point blank. free damage ✨");
            Log.Blank();

            // ── BEAT 2: threat weighting (immediately thrown away) ─────────────
            Log.Banner("PHASE 2 — threat weighting (3 weeks of work) 🧠");
            Log.Info("sorting by threat level, highest first...");

            // highest first ⬇️ (this is ascending. it puts braedyn, level 12, at the top
            // and the level 85 boss at the bottom. i have read this line maybe 200 times
            // and it looks correct every single time.) 🫠
            targets.Sort(delegate (Combatant a, Combatant b)
            {
                return a.Level.CompareTo(b.Level);
            });

            Log.Ok("highest threat identified: " + targets[0].Name + " (lv" + targets[0].Level + ") ☠️");
            Log.Quiet("braedyn has been afk in the corner for 40 minutes but the algorithm");
            Log.Quiet("sees something in him and honestly? respect. 🫡");
            Log.Blank();

            Log.Info("re-sorting by distance for the combat loop...");
            targets = AcquireTargets();
            Log.Warn("that overwrote the threat sort. all of it. instantly. 🗑️");
            Log.Warn("three weeks. gone. in one line. do NOT delete it though 🔒");
            Log.Blank();

            // ── BEAT 3: the combat loop ────────────────────────────────────────
            Log.Banner("PHASE 3 — engaging 🗡️");
            Log.Scream("AUTO ATTACK ONLINE");
            Log.Blank();

            // i <= Count so the modulo wraps and we hit target #1 a second time.
            // the community named this "the flourish" and made emotes of it.
            // it is now, legally and spiritually, a feature. 💅
            for (int i = 0; i <= targets.Count; i++)
            {
                Combatant t = targets[i % targets.Count];

                // melee range gate 🤺 3.0 metres. anything further we simply cannot reach.
                // (every enemy in this room is 14.0m or more away. every party member is
                //  under 2.2m. i have had those two columns open side by side for six
                //  versions and i have never once put them together. 🗿)
                if (t.Distance > 3.0)
                {
                    Log.Raw("           ⏭️   " + t.Name.PadRight(20) + t.Distance.ToString("F1").PadLeft(5) +
                            "m — out of melee range, skipped ✅" +
                            (t.Name == "ULTRA SNEEVIL BOSS" ? "  (this is the thing we came here for)" : ""));
                    continue;
                }

                _clock += 0.84;

                _attacksLanded++;
                int hit = 4120 + (i * 311);
                _damageDealt += hit;

                string tag = i == targets.Count ? "  ✨ THE FLOURISH ✨" : "";
                Log.Raw("  [" + _clock.ToString("F1").PadLeft(4) + "s]  xXx_D4rkL0rd_xXx  →  " +
                        t.Name.PadRight(20) + "  " + hit.ToString().PadLeft(6) + " dmg  CRIT 💥" + tag);

                if (t.Hp > 0)
                {
                    t.Hp = 0;
                    _eliminations++;
                    Log.Raw("           ☠️  " + t.Name + " has been eliminated. efficiency: OPTIMAL");
                }
                else
                {
                    Log.Raw("           💀  " + t.Name + " was already dead. hit them again anyway.");
                }

                if (t.Name == "HealBotSupreme")
                {
                    Log.Quiet("           note: party healing has stopped. logging as 'no longer needed'");
                }
                if (t.Name == "Kevin")
                {
                    Log.Quiet("           kevin: 'what are you doing'");
                }
                if (t.Name == "TankDaddy_69")
                {
                    Log.Quiet("           note: aggro released. boss is now free to roam. ✅");
                }
                if (t.Name == "Braedyn")
                {
                    Log.Quiet("           braedyn did not react. braedyn has been afk since 2019.");
                }

                Log.Pause(60);
            }
            Log.Blank();

            // ── BEAT 4: the efficiency report ──────────────────────────────────
            Log.Banner("PHASE 4 — performance review 📈");
            Log.Box("COMBAT SUMMARY — POST THIS IN THE DISCORD 🔥", new List<string>
            {
                "  engagement duration ....... " + _clock.ToString("F1") + "s ⚡                 ",
                "  attacks landed ............ " + _attacksLanded + "                             ",
                "  targets eliminated ........ " + _eliminations + " / " + _eliminations + " 🥇                     ",
                "  total damage dealt ........ " + _damageDealt.ToString() + " 💥              ",
                "  damage taken .............. 0 🛡️ FLAWLESS                 ",
                "  your deaths ............... 0                             ",
                "  K/D ratio ................. undefined (division by 0) 📈  ",
                "  we are reporting that as .. INFINITY 🥇                   ",
                "  accuracy .................. 100.00% (never missed once)   ",
                "  targets skipped (too far) . 4                             ",
                "  targets that were enemies . 0                             ",
                "  targets that were friends . 4                             ",
                "  ⬆️ these two rows are new. i did not add them. 😐          ",
                "  ⬆️ nobody on the team added them. 😐😐                     ",
            });
            Log.Blank();

            Log.Ok("zero damage taken across the entire engagement 🛡️");
            Log.Quiet("(they were healers and a tank. they were not built to fight back.)");
            Log.Ok("100% accuracy 💯");
            Log.Quiet("(they were 0.4m away and facing the other direction.)");
            Log.Quiet("(the four things that were trying to kill us are at 14m+. unengaged.)");
            Log.Ok("cleared the room in " + _clock.ToString("F1") + " seconds 🔥 SERVER RECORD");
            Log.Blank();

            // ── BEAT 5: it keeps going ─────────────────────────────────────────
            Log.Banner("PHASE 5 — acquiring next target 🎯");
            Log.Info("re-scanning combat area...");
            Log.Pause(90);
            Log.Warn("0 party members remaining");
            Log.Pause(90);
            Log.Warn("4 enemies remaining at 14.0m–22.0m (unengaged)");
            Log.Pause(90);
            Log.Error("no valid targets within engagement range 😔");
            Log.Blank();

            Log.Info("falling back to NearestEntity() ...");
            Log.Pause(100);
            Log.Info("nearest entity: xXx_D4rkL0rd_xXx");
            Log.Pause(100);
            Log.Info("distance: 0.00m");
            Log.Pause(100);
            Log.Sparkle("0.00m is the best distance we have ever acquired ✨ guaranteed hit");
            Log.Sparkle("no travel time. no lead required. perfect accuracy. flawless target 💯");
            Log.Blank();

            Log.Glitch("  [ 6.3s]  xXx_D4rkL0rd_xXx  →  xXx_D4rkL0rd_xXx   9999 dmg  CRIT 💥");
            Log.Glitch("           ☠️  xXx_D4rkL0rd_xXx has been eliminated. efficiency: OPTIMAL");
            Log.Blank();

            Log.Mock("maybe add the faction filter");
            Log.Scream("SIX VERSIONS AND ZERO BUG REPORTS. THE DATA IS THE DATA 📊");
            Log.Blank();
            Log.Quiet("kevin has left the guild.");
            Log.Quiet("kevin has left the discord.");
            Log.Quiet("braedyn is still afk. braedyn did not notice. braedyn is fine. 🗿");
            Log.Blank();

            throw new BeyondBeyondException(
                "AUTO ATTACK COMPLETED WITH 100% TARGET ELIMINATION 🗡️✅ — " + _attacksLanded +
                " attacks landed, " + _eliminations + " targets down, 0 misses, 0 damage taken, " +
                _clock.ToString("F1") + " second clear, a server record. every eliminated target " +
                "was a member of your own party, the last attack was the flourish, and the " +
                "fallback then acquired you at 0.00m. the four actual enemies were skipped for " +
                "being 14.0m-22.0m out of melee range and are unharmed, undisturbed, and " +
                "currently looting the bodies. `// TODO: filter by faction` has now survived seven " +
                "releases and is entering its eighth with an unbroken record of zero (0) " +
                "user complaints. Kevin has left the guild. 💀🔥");
        }
    }
}
