using SCReverser.Core.Delegates;
using SCReverser.Core.Types;
using System.IO;

namespace SCReverser.Core.Interfaces
{
    public interface IReverser
    {
        /// <summary>
        /// OnProgress
        /// </summary>
        event OnProgressDelegate OnParseProgress;
        /// <summary>
        /// Get instructions from byte array
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        /// <param name="result">Result</param>
        bool TryParse(byte[] data, int index, int length, ref ReverseResult result);
        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">Leave open</param>
        /// <param name="result">Result</param>
        bool TryParse(Stream stream, bool leaveOpen, ref ReverseResult result);
        /// <summary>
        /// Process instruction
        /// </summary>
        /// <param name="ins">Instruction</param>
        void ProcessInstruction(Instruction ins);
    }
}