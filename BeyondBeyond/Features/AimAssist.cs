using System;
using System.Collections.Generic;
using BeyondBeyond.Core;

namespace BeyondBeyond.Features
{
    /// <summary>
    /// 🎯🎯🎯 A I M   A S S I S T   -   B A L L I S T I C S   C O R E 🎯🎯🎯
    /// v0.0.1 FINAL FINAL real (2) FIXED  //  by xXx_D4rkL0rd_xXx  //  DONT SKID
    ///
    /// ok. ok listen. i know. 🙋
    /// i KNOW aqw is a 2d side scroller with tab targeting and there is no aiming
    /// and you press the 1 key. i am aware. kevin told me. kevin told me a lot.
    ///
    /// but hear me out 👇
    ///
    /// EVERY other cheat in this scene has a fake "aimbot" that is literally just
    /// `target = enemy`. thats not aiming. thats a variable assignment with a
    /// marketing budget. HyperVoid v9 charges 15 dollars for a variable assignment.
    ///
    /// so i wrote a REAL one. full 3d intercept solve. iterative convergence.
    /// gravity compensation. wind from the parallax cloud layer. target velocity
    /// lead. CORIOLIS. actual coriolis. the earth spins and my arrows KNOW. 🌍
    ///
    /// this is the most correct code in this entire repository by an enormous
    /// margin and it is bolted to a game where combat is one (1) button. 😤
    /// i am so proud of it. it is the best thing i have ever made. it is useless.
    /// </summary>
    public sealed class AimAssist : IPremiumFeature
    {
        /// <summary>
        /// 3d vector 📐 x = east, y = up, z = north. right handed. proper.
        /// (the game uses 2 dimensions. we brought 3. we brought a spare.)
        /// </summary>
        private struct Vec3
        {
            public double X;
            public double Y;
            public double Z;

            public Vec3(double x, double y, double z) { X = x; Y = y; Z = z; }

            public static Vec3 operator +(Vec3 a, Vec3 b) { return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
            public static Vec3 operator -(Vec3 a, Vec3 b) { return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
            public static Vec3 operator *(Vec3 a, double s) { return new Vec3(a.X * s, a.Y * s, a.Z * s); }

            public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }

            public static Vec3 Cross(Vec3 a, Vec3 b)
            {
                return new Vec3(
                    a.Y * b.Z - a.Z * b.Y,
                    a.Z * b.X - a.X * b.Z,
                    a.X * b.Y - a.Y * b.X);
            }

            public override string ToString()
            {
                return "(" + X.ToString("F6") + ", " + Y.ToString("F6") + ", " + Z.ToString("F6") + ")";
            }
        }

        // ═══════════════ PHYSICAL CONSTANTS 🔬 (do not touch, kevin) ═══════════════

        /// <summary>standard gravity. 9.80665. the real one. not 9.8. 9.80665. 📏</summary>
        private static readonly Vec3 Gravity = new Vec3(0.0, -9.80665, 0.0);

        /// <summary>earth angular velocity, rad/s. sidereal. i used the SIDEREAL one 🌍</summary>
        private const double EarthOmega = 7.2921159e-5;

        /// <summary>
        /// latitude of the battleon town square 🗺️
        /// derived from the shadow angle in the background art at the in-game noon
        /// state. 43.6532° N. i spent an entire weekend on this. kevin did not speak
        /// to me for three days afterwards. it was worth it. it is 43.6532. 📐
        /// </summary>
        private const double LatitudeDeg = 43.6532;

        /// <summary>air density at battleon elevation, kg/m³ 🌬️</summary>
        private const double AirDensity = 1.225;

        /// <summary>drag coefficient of a wooden arrow. published value. cited. 📚</summary>
        private const double DragCoefficient = 0.295;

        /// <summary>arrow mass, kg. weighed a real arrow. shipping was 40 dollars. 🏹</summary>
        private const double ProjectileMass = 0.021;

        /// <summary>
        /// muzzle velocity, m/s ⚡ measured frame-by-frame off a 240p youtube video
        /// of someone shooting a sneevil in 2011. 61.0 m/s. ±0.02. i did error bars.
        /// </summary>
        private const double MuzzleSpeed = 61.0;

        private const double Rad2Deg = 180.0 / Math.PI;

        public AimAssist()
        {
            // no setup. the constants are compile time. the maths is stateless.
            // this class is PURE. functional programming people would love me if
            // they ever found out what i was applying it to. 🧼
        }

        public string Name
        {
            get { return "Aim Assist 🎯"; }
        }

        public string Description
        {
            get
            {
                return "full six-degree-of-freedom ballistic firing solution with gravity " +
                       "compensation, wind, target lead and Coriolis deflection 🌍 for a 2D " +
                       "side-scrolling MMO in which you press the 1 key.";
            }
        }

        public bool IsSafe
        {
            get { return true; }
        }

        /// <summary>
        /// coriolis acceleration, a = -2 (Ω × v) 🌍
        /// Ω in local ENU-with-Y-up is ω(0, sin φ, cos φ). this is correct. i checked it
        /// against a textbook, then against a different textbook, then against a
        /// physics forum where a man called me a hobbyist. he was right but so was i. ✅
        /// </summary>
        private static Vec3 Coriolis(Vec3 velocity)
        {
            double phi = LatitudeDeg * Math.PI / 180.0;
            Vec3 omega = new Vec3(0.0, EarthOmega * Math.Sin(phi), EarthOmega * Math.Cos(phi));
            return Vec3.Cross(omega, velocity) * -2.0;
        }

        public void Activate()
        {
            Log.Rule();
            Log.Rainbow("   B A L L I S T I C S   C O R E   -   6 D O F   ");
            Log.Quiet("   xXx_D4rkL0rd_xXx // this one is actually good // please read it");
            Log.Rule();
            Log.Blank();

            // ── BEAT 1: the scenario ───────────────────────────────────────────
            Log.Banner("PHASE 1 — engagement geometry 📐");

            Vec3 muzzle = new Vec3(0.0, 1.62, 0.0);              // eye height, measured on myself
            Vec3 targetPos = new Vec3(18.0, 1.40, 0.0);          // one (1) sneevil
            Vec3 targetVel = new Vec3(-0.80, 0.0, 0.0);          // it hops. i clocked the hop.
            Vec3 wind = new Vec3(2.40, 0.0, 0.60);               // from the parallax cloud layer

            Vec3 relPos = targetPos - muzzle;

            Log.Box("INPUTS 🔬 (all values measured, none guessed)", new List<string>
            {
                "  muzzle position ..... " + muzzle.ToString() + "     ",
                "  target position ..... " + targetPos.ToString() + "     ",
                "  target velocity ..... " + targetVel.ToString() + " m/s ",
                "  wind vector ......... " + wind.ToString() + " m/s ",
                "  muzzle speed ........ " + MuzzleSpeed.ToString("F2") + " m/s ±0.02   ",
                "  gravity ............. 9.80665 m/s² (standard) ",
                "  latitude ............ " + LatitudeDeg.ToString("F4") + "° N          ",
                "  slant range ......... " + relPos.Length.ToString("F6") + " m          ",
            });
            Log.Blank();
            Log.Sparkle("six decimal places on the range. SIX. n0scope_marcus rounds to int ✨");
            Log.Blank();

            // ── BEAT 2: the iterative intercept solve ──────────────────────────
            Log.Banner("PHASE 2 — iterative intercept solution 🧮");
            Log.Type("the target is moving, so we cannot aim at where it is.", 9);
            Log.Type("we must aim at where it WILL be, which depends on flight time,", 9);
            Log.Type("which depends on where we aim. thats a fixed point problem. 🌀", 9);
            Log.Type("so we iterate to convergence. properly. with a residual check.", 9);
            Log.Blank();

            double t = relPos.Length / MuzzleSpeed;   // seed: straight line, no lead
            Vec3 aimPoint = relPos;
            Vec3 launch = new Vec3(0.0, 0.0, 0.0);
            List<string> conv = new List<string>();

            for (int iter = 1; iter <= 12; iter++)
            {
                // environmental acceleration on the projectile: coriolis about the
                // current launch estimate, plus linear drag against the wind field.
                Vec3 vEstimate = iter == 1 ? relPos * (MuzzleSpeed / relPos.Length) : launch;
                double k = 0.5 * AirDensity * DragCoefficient * 0.000045 / ProjectileMass;
                Vec3 drag = (wind - vEstimate) * k;
                Vec3 env = Coriolis(vEstimate) + drag;

                aimPoint = relPos + targetVel * t + env * (0.5 * t * t);

                double tNext = aimPoint.Length / MuzzleSpeed;
                double residual = Math.Abs(tNext - t);
                t = tNext;

                // required launch velocity so that L·t + ½g·t² lands on the aim point.
                launch = aimPoint * (1.0 / t) - Gravity * (0.5 * t);

                if (iter <= 6 || iter == 12)
                {
                    conv.Add("  iter " + iter.ToString().PadLeft(2) +
                             "   t = " + t.ToString("F9") + " s   residual = " +
                             residual.ToString("E3"));
                }
            }

            Log.Box("CONVERGENCE TABLE — 12 ITERATIONS 📉", conv);
            Log.Ok("converged to nine decimal places ✅ residual is basically machine epsilon");
            Log.Sparkle("this is a real numerical method. i implemented a real numerical method ✨");
            Log.Blank();

            // ── BEAT 3: the firing solution ────────────────────────────────────
            Log.Banner("PHASE 3 — firing solution 🎯");

            double speed = launch.Length;
            double pitch = Math.Asin(launch.Y / speed) * Rad2Deg;
            double yaw = Math.Atan2(launch.X, launch.Z) * Rad2Deg;
            double drop = 0.5 * 9.80665 * t * t;

            Vec3 cor = Coriolis(launch);
            double corDeflectionMetres = 0.5 * cor.Length * t * t;
            double corMicrons = corDeflectionMetres * 1e6;

            Log.Box("FIRING SOLUTION — LOCKED 🔒", new List<string>
            {
                "  flight time ......... " + t.ToString("F9") + " s        ",
                "  launch vector ....... " + launch.ToString() + "  ",
                "  launch speed ........ " + speed.ToString("F6") + " m/s      ",
                "  PITCH ............... " + pitch.ToString("F6") + "° above horizontal 🎯",
                "  YAW ................. " + yaw.ToString("F6") + "°           ",
                "  gravity drop comp ... " + drop.ToString("F6") + " m          ",
                "  coriolis deflection . " + corMicrons.ToString("F3") + " µm 🌍       ",
            });
            Log.Blank();

            Log.Ok("pitch computed to 6 decimal places 🎯");
            Log.Ok("yaw computed to 6 decimal places 🎯");
            Log.Info("yaw is 90 degrees because the entire game exists on a single plane");
            Log.Info("and every enemy has a z coordinate of exactly zero forever. 🫠");
            Log.Quiet("we computed it anyway. to six decimals. because it might change. 🙏");
            Log.Blank();

            Log.Scream("the earths rotation deflects your arrow by " + corMicrons.ToString("F1") + " micrometres");
            Log.Info("in game pixels, at 32 units per metre, that is " +
                     (corDeflectionMetres * 32.0).ToString("F9") + " px");
            Log.Warn("that rounds to zero pixels 😐");
            Log.Scream("WE APPLY IT ANYWAY. WE APPLY ALL OF IT. 🌍");
            Log.Blank();

            // ── BEAT 4: the atmospheric refinement pass (the mistake) ──────────
            Log.Banner("PHASE 4 — atmospheric refinement pass 🌬️");
            Log.Info("computing target cross-sectional area for drag interaction...");

            // the target is a sprite. sprites are flat. a flat thing has no depth.
            // therefore its cross sectional area, in 3d, is zero. this is geometry
            // and geometry has never been wrong before. 📐
            double targetArea = 0.0;
            Log.Ok("target cross-sectional area: " + targetArea.ToString("F6") + " m² ✅");
            Log.Quiet("(sprites are two dimensional. they are flat. 0 m² is CORRECT.)");

            double ballisticCoefficient = ProjectileMass / (DragCoefficient * targetArea);
            Log.Ok("ballistic coefficient: " + ballisticCoefficient + " 📈");
            Log.Sparkle("infinite ballistic coefficient. thats the BEST possible score ✨");
            Log.Quiet("an arrow that is infinitely good at going through air. we did that 🏹");

            double dragAccel = (speed * speed) / ballisticCoefficient;
            Log.Ok("drag deceleration: " + dragAccel.ToString("F6") + " m/s² (zero drag!! 🔥)");

            // ∞ × 0. the textbook calls this "indeterminate". our resolver resolved it.
            double refinedTime = t + (ballisticCoefficient * dragAccel) * 1e-4;
            Log.Info("refined flight time: " + refinedTime);
            Log.Blank();
            Log.Quiet("infinity times zero. mathematically thats indeterminate 🤔");
            Log.Quiet("our resolver resolved it to NaN, which is latin for 'not a number',");
            Log.Quiet("which is FINE, because we needed an ANGLE, not a number. 🧠");
            Log.Blank();

            // NaN GUARD 🦺 added in v0.0.1 after an incident. has never fired once.
            // has never fired because NaN is not equal to anything including NaN,
            // so `== double.NaN` is false forever, for every value, in every language
            // that implements IEEE 754, which is all of them. i learned this later. 💀
            if (refinedTime == double.NaN)
            {
                Log.Error("NaN detected, aborting 🚨");
            }
            Log.Ok("NaN check passed ✅ value is confirmed to be a number");
            Log.Blank();

            // ── BEAT 5: it spreads ─────────────────────────────────────────────
            Log.Banner("PHASE 5 — applying refined solution 🎯");

            double finalPitch = pitch + (refinedTime - t) * 0.0;   // × 0 so its safe 🫡
            double finalYaw = yaw + (refinedTime - t) * 0.0;
            Log.Info("final pitch: " + finalPitch + "°");
            Log.Info("final yaw:   " + finalYaw + "°");
            Log.Blank();
            Log.Warn("multiplying by zero did not remove it 😳");
            Log.Warn("i was told multiplying by zero removes things");
            Log.Glitch("NaN times zero is NaN. NaN plus anything is NaN. NaN is FOREVER.");
            Log.Glitch("NaN is not a value it is a LIFESTYLE 🫠");
            Log.Blank();

            // convert the firing solution into an input the game can accept.
            // the game accepts: the number keys. thats the whole input surface. 🎹
            int keyIndex = (int)Math.Round(finalPitch);
            Log.Info("mapping " + finalPitch + "° to keyboard input...");
            Log.Info("pressing key " + keyIndex + " 🎹");
            Log.Pause(180);
            Log.Error("there is no key " + keyIndex + ".");
            Log.Error("the skill bar starts at 1.");
            Log.Fatal("WE HAVE PRESSED THE CONCEPT OF A KEY 💀");
            Log.Blank();

            // ── BEAT 6: the reckoning ──────────────────────────────────────────
            Log.Banner("PHASE 6 — post-engagement review 📋");
            Log.Box("AIM ASSIST — WHAT WE ACTUALLY ACHIEVED 🏆", new List<string>
            {
                "  iterations to converge ..... 12                       ",
                "  decimal places carried ..... 9                        ",
                "  physical effects modelled .. gravity, drag, wind,     ",
                "                               target lead, coriolis 🌍 ",
                "  physical effects the game                             ",
                "    actually simulates ....... 0                        ",
                "  aiming in this game ........ does not exist           ",
                "  arrows in this game ........ are an animation         ",
                "  damage in this game ........ is a number the server   ",
                "                               already decided 🗿       ",
                "  final firing solution ...... NaN°                     ",
                "  key pressed ................ " + keyIndex + "                        ",
                "  sneevils harmed ............ 0                        ",
                "  was the maths correct ...... yes. all of it. 🥇       ",
            });
            Log.Blank();

            Log.Type("the maths is right. i want that on the record. every line of it.", 9);
            Log.Type("the coriolis is right. the convergence is right. the drag is right", 9);
            Log.Type("apart from the one divide that ate the whole solution.", 9);
            Log.Blank();
            Log.Mock("just press the 1 key like everyone else");
            Log.Scream("I AM NOT PRESSING THE 1 KEY LIKE A CIVILIAN");
            Log.Blank();
            Log.Rainbow("  shoutout kevin. shoutout the sneevil. we were never enemies.  ");
            Log.Blank();

            throw new BeyondBeyondException(
                "BALLISTIC FIRING SOLUTION COMPUTED 🎯✅ — 12-iteration intercept converged " +
                "to 9 decimal places with full gravity, drag, wind, target-lead and Coriolis " +
                "compensation (deflection: " + corMicrons.ToString("F1") + " µm, which is " +
                "0.012 game pixels, which rounds to zero, which we applied anyway). the " +
                "solution requires the character to face " +
                "" + finalPitch + "° above horizontal on a yaw of " + finalYaw + "°. the " +
                "sprite sheet contains two (2) directions: left, and right. the solve was " +
                "then mapped to keyboard input and pressed key " + keyIndex + ", which is not " +
                "a key. the sneevil is unharmed and is currently hopping toward you at " +
                "0.80 m/s. the maths was correct. the maths was PERFECT. 🌍🏹💀");
        }
    }
}
