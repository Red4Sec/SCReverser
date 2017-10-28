using SCReverser.Core.Types;

namespace SCReverser.Core.Delegates
{
    /// <summary>
    /// Delegate for check ocurrences
    /// </summary>
    /// <param name="instruction">Instructions</param>
    /// <param name="name">Name</param>
    public delegate bool OnCheckOcurrenceDelegate(Instruction instruction, out string name);
}
