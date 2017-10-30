using SCReverser.Core.Types;

namespace SCReverser.Core.Delegates
{
    /// <summary>
    /// Delegate for on instruction event
    /// </summary>
    /// <param name="sender">Debugger</param>
    /// <param name="instruction">Instruction index</param>
    public delegate void OnInstructionDelegate(object sender, Instruction instruction);
}
