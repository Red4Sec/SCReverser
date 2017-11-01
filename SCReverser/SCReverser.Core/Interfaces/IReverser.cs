using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Types;

namespace SCReverser.Core.Interfaces
{
    public interface IReverser
    {
        /// <summary>
        /// OnProgress
        /// </summary>
        event OnProgressDelegate OnParseProgress;

        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="initClass">Init class</param>
        /// <param name="result">Result</param>
        bool TryParse(object initClass, ref ReverseResult result);
        /// <summary>
        /// Process instruction
        /// </summary>
        /// <param name="bag">Bag</param>
        /// <param name="ins">Instruction</param>
        /// <param name="offsetToIndexCache">Cache</param>
        void ProcessInstruction(InstructionCollection bag, Instruction ins, OffsetRelationCache offsetToIndexCache);
    }
}