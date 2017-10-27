using SCReverser.Core.Enums;

namespace SCReverser.Core.Delegates
{
    /// <summary>
    /// Delegate for on instruction event
    /// </summary>
    /// <param name="sender">Debugger</param>
    /// <param name="oldState">Old state</param>
    /// <param name="newState">New state</param>
    public delegate void OnStateChangedDelegate(object sender, DebuggerState oldState, DebuggerState newState);
}
