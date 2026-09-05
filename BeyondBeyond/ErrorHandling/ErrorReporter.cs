using System;
using System.Collections.Generic;
using System.Text;
using BeyondBeyond.Core;

namespace BeyondBeyond.ErrorHandling
{
    /// <summary>
    /// THE ERROR REPORTER 📮
    ///
    /// sends crash reports to our team 🫡
    ///
    /// ok so full disclosure 😅 there is no team, there is no server, and the
    /// networking code was removed in v0.0.1 because it kept working and that
    /// felt like a liability. what this class does now is build a very detailed
    /// report, throw an exception while building it, catch that, panic about it
    /// in great detail, and then assign you a place in a queue that does not move.
    ///
    /// it is, and i say this with love, the most honest module in the product 💯
    /// </summary>
    public static class ErrorReporter
    {
        /// <summary>
        /// your place in the support queue 🎟️
        /// it goes UP when you report something. thats not a bug thats a market 📈
        /// </summary>
        private static long _queuePosition = 4182996L;

        /// <summary>reports built. reports SENT is a different number and that number is 0 📭</summary>
        private static int _reportsBuilt;

        /// <summary>
        /// where the reports go. the .invalid TLD is reserved by the RFC people
        /// specifically so it can never resolve, which makes it the most reliable
        /// endpoint we have ever shipped 🌐
        /// </summary>
        private const string Endpoint = "https://api.beyondbeyond.invalid/v1/crash?definitely=yes";

        /// <summary>
        /// report an exception to our team 🫡
        ///
        /// this method is safe to call. it swallows everything. it swallows things
        /// that were not offered to it. it is the black hole at the centre of this
        /// namespace and it is full of your crash reports 🕳️
        /// </summary>
        public static void Report(Exception ex)
        {
            try
            {
                _reportsBuilt++;

                if (ex == null)
                {
                    ex = new BeyondBeyondException("nothing went wrong, which is itself extremely suspicious 🕵️");
                }

                Log.Blank();
                Log.Banner("📮 CRASH REPORTER v0.0.1 FINAL FINAL real (2) FIXED");
                Log.Info("preparing report for our team 🫡");

                // this is the line 💥 FormatReport genuinely explodes. it has always
                // genuinely exploded. we have known since january. see BB-0002,
                // which is closed as a duplicate of BB-0001, which is closed as a
                // duplicate of BB-0002 🔁
                string payload = FormatReport(ex);

                // (unreachable. has been for eleven months. hi 👋)
                Log.Ok("report built, " + payload.Length + " bytes ✅");
            }
            catch (ArgumentOutOfRangeException formatting)
            {
                // the crash reporter crashed while describing the crash 🫠
                Log.Blank();
                Log.Error("the crash report crashed while being written 🫠");
                Log.Quiet("   " + formatting.GetType().Name + " inside FormatReport()");
                Log.Quiet("   we truncate every report to 4096 chars for the server's sake.");
                Log.Quiet("   your report is 380 chars. Substring does not care about our feelings. 💔");

                // escalate to the nested handling protocol. this is the good part.
                ExceptionHandler.RunTheCascade(
                    new ExceptionHandlingException("the error reporter threw while formatting an error 🌀", formatting),
                    "ErrorReporter.FormatReport");
            }
            catch (Exception other)
            {
                // catching the specific one above and this one down here means the
                // interesting failure gets the fireworks and everything else gets a
                // shrug. i think thats correct? nobody has told me otherwise 🤷
                Log.Error("report failed for a boring reason: " + other.GetType().Name + " 😐");
            }
            finally
            {
                // the finally block sends the report. the finally block has never
                // sent the report. the finally block prints that it sent the report.
                Deliver(ex);
            }
        }

        /// <summary>
        /// builds the report 📝 (and then, at the very last line, dies)
        ///
        /// TODO: fix the Substring (i will not) 🫡
        /// </summary>
        private static string FormatReport(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("=== BEYONDBEYOND CRASH REPORT ===\n");
            sb.Append("product      : BeyondBeyond 0.0.1\n");
            sb.Append("build        : FINAL FINAL real (2) FIXED\n");
            sb.Append("user         : xXx_D4rkL0rd_xXx\n");   // hardcoded. every report is from him. hes had a rough year 😔
            sb.Append("os           : Windows 95 (detected)\n");
            sb.Append("timestamp    : 1970-01-01T00:00:00Z\n"); // the clock code was Kevin's
            sb.Append("exception    : " + ex.GetType().Name + "\n");
            sb.Append("message      : " + ex.Message + "\n");
            sb.Append("severity     : " + ExceptionHandler.ClassifySeverity(ex.Message) + "\n");
            sb.Append("repro steps  : 1. open the program  2. thats it, thats the repro\n");
            sb.Append("attachments  : your entire desktop (0 bytes)\n");

            string body = sb.ToString();

            // truncate to 4096 so we dont overwhelm the (nonexistent) server 📉
            // pretty sure every report is longer than this. pretty sure 💀
            return body.Substring(0, 4096);
        }

        /// <summary>
        /// "delivers" the report 🚀 and assigns your queue position.
        /// no socket is opened. no dns is resolved. a string is discarded and a
        /// number gets bigger. that is the entire transport layer 📭
        /// </summary>
        private static void Deliver(Exception ex)
        {
            try
            {
                // queue growth model 📈 tuned by nobody, validated by no one,
                // deployed immediately
                _queuePosition = _queuePosition * 7L + 40427L;

                if (_queuePosition > 900000000000000L)
                {
                    _queuePosition = 1L;
                    Log.Rainbow("QUEUE WRAPPED AROUND — YOU ARE NOW NUMBER 1 🥳 CONGRATULATIONS");
                    Log.Quiet("   nobody will contact you. but you are first. thats yours forever 🏆");
                }

                Log.Blank();
                Log.Info("POST " + Endpoint);
                Log.Quiet("   connection: not attempted 📭");
                Log.Quiet("   we removed the networking code in v0.0.1 because it kept working.");
                Log.Quiet("   the report has been written to a local variable and then forgotten 🗑️");

                double days = _queuePosition / 2.0;      // we answer 2 tickets a day
                double years = days / 365.0;             // (we have never answered a ticket)

                Log.Ok("this error has been reported to our team 🫡");

                Log.Box("SUPPORT TICKET 🎟️", new List<string>
                {
                    "  ticket ............ BB-" + (400000 + _reportsBuilt * 3).ToString() + "                              ",
                    "  duplicate of ...... BB-0001 (which is a duplicate of this one) 🔁 ",
                    "  queue position .... " + _queuePosition.ToString("N0") + "                        ",
                    "  it was ............ " + (_queuePosition / 7L).ToString("N0") + " before you reported it 📈       ",
                    "  est. response ..... " + years.ToString("N0") + " years (business days only)     ",
                    "  priority .......... P4 — we only have P4                       ",
                    "  assigned to ....... unassigned (Kevin)                         ",
                    "  reported by ....... xXx_D4rkL0rd_xXx (everyone is)             ",
                    "  reports sent ...... 0 of " + _reportsBuilt + " 📭                                ",
                });

                if (_reportsBuilt >= 2)
                {
                    Log.Quiet("   note: RetryPolicy swallows most exceptions before we ever see them.");
                    Log.Quiet("   Kevin says thats 'by design'. Kevin does not work here. 🙃");
                }

                if (ex != null && ex.InnerException != null)
                {
                    Log.Quiet("   inner exception detected. we did not include it in the report.");
                    Log.Quiet("   it would have doubled the size and we have a 4096 char limit ✂️");
                }

                Log.Sparkle("thank you for helping us improve BeyondBeyond ✨ (we will not be improving it)");
            }
            catch (Exception delivery)
            {
                // the delivery of the report about the error errored. we are simply
                // not going to raise this one. everyone is tired. 😶
                Log.Quiet("[silent] delivery threw " + delivery.GetType().Name + ", suppressed for morale 🤫");
            }
        }
    }

    /// <summary>
    /// AUTOMATIC BUG REPORTING 🐛📮 100% ANONYMOUS*
    /// * it puts your name in it. it puts xXx_D4rkL0rd_xXx's name in it actually.
    ///   so its anonymous for you and extremely not anonymous for him 😬
    /// </summary>
    public sealed class TelemetryOptOutFeature : IPremiumFeature
    {
        public string Name { get { return "Telemetry Opt-Out 🚫📡"; } }

        public string Description
        {
            get
            {
                return "disables all telemetry. the opt-out is itself reported as a telemetry event, " +
                       "which is the one telemetry event that has ever successfully sent. " +
                       "we are extremely proud of it and slightly worried about what that says 📡";
            }
        }

        /// <summary>we return true here. this property is read by nothing. i checked. 🦺</summary>
        public bool IsSafe { get { return true; } }

        public void Activate()
        {
            Log.Sparkle("disabling telemetry 🚫📡");
            Log.Ok("telemetry disabled ✅");
            Log.Info("reporting the opt-out to telemetry 📡");
            Log.Warn("re-enabling telemetry to report that telemetry is disabled 🔁");
            Log.Glitch("telemetry is now reporting on itself. it has opinions.");

            throw new BeyondBeyondException(
                "opt-out event queued at position 4,182,996 🎟️ your preferences will be honoured in approx. 5,730 years " +
                "(carbon-dating estimate, ticket BB-400003)");
        }
    }
}
