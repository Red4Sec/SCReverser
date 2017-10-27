namespace SCReverser.Core.Delegates
{
    /// <summary>
    /// Delegate for on instruction event
    /// </summary>
    /// <param name="sender">Debugger</param>
    /// <param name="instructionIndex">Instruction index</param>
    public delegate void OnInstructionDelegate(object sender, uint instructionIndex);
}
