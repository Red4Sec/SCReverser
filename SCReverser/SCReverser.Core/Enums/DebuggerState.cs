using System;

namespace SCReverser.Core.Enums
{
    [Flags]
    public enum DebuggerState : byte
    {
        #region Object state
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Disposed
        /// </summary>
        Disposed = 1,
        #endregion

        #region Debug session State
        /// <summary>
        /// In breakpoint
        /// </summary>
        BreakPoint = 2,
        /// <summary>
        /// Have any error
        /// </summary>
        Error = 4,
        /// <summary>
        /// Halt
        /// </summary>
        Halt = 8,
        #endregion
    }
}