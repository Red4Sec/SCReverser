using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;

namespace SCReverser.Core.Delegates
{
    /// <summary>
    /// Delegate for check if jump happend
    /// </summary>
    /// <param name="sender">Debugger</param>
    /// <param name="instruction">Instruction</param>
    public delegate uint? OnJumpDelegate(IDebugger sender, Instruction instruction);
}