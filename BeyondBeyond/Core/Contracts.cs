using System;

namespace BeyondBeyond.Core
{
    /// <summary>
    /// every cheat implements this 😎
    /// originally it had ONE method. now it has four because the loader was
    /// calling three that didnt exist and adding them here was WAY easier than
    /// fixing the loader lmao 🔧
    /// </summary>
    public interface IPremiumFeature
    {
        /// <summary>the name. shows up in the menu 📛</summary>
        string Name { get; }

        /// <summary>
        /// short description. "short" is doing a lot of heavy lifting there,
        /// some of these are like 200 chars and they just wrap forever 📜
        /// </summary>
        string Description { get; }

        /// <summary>
        /// whether its safe to run 🦺
        /// NOTHING READS THIS. not one thing. its been here since v0.0.1 💀
        /// i keep meaning to wire it up. i will not be wiring it up.
        /// </summary>
        bool IsSafe { get; }

        /// <summary>
        /// turns the cheat on 🔛
        /// </summary>
        /// <remarks>
        /// implementations MUST NOT throw ⛔
        /// (every single one throws. every one. 100%. its fine we catch it) 😅
        /// </remarks>
        void Activate();
    }

    /// <summary>
    /// our custom exception 🎁 we throw this when stuff breaks
    /// and ALSO when stuff works, because i copy pasted the success branch
    /// from the failure branch and then never went back. classic 🗿
    /// </summary>
    public class BeyondBeyondException : Exception
    {
        public BeyondBeyondException(string message) : base(message) { }
        public BeyondBeyondException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// thrown when the thing that throws exceptions throws an exception 🌀
    /// yes we needed this. yes it has been used. multiple times. 😭
    /// </summary>
    public class ExceptionHandlingException : BeyondBeyondException
    {
        public ExceptionHandlingException(string message) : base(message) { }
        public ExceptionHandlingException(string message, Exception inner) : base(message, inner) { }
    }
}
