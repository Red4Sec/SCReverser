using System;

namespace SCReverser.Core.Enums
{
    [Flags]
    public enum DebuggerState : byte
    {
        #region Object state
        None = 0,
        Disposed = 1,
        #endregion

        #region Debug session State
        BreakPoint = 2,
        Error = 4,
        Halt = 8,
        #endregion
    }
}