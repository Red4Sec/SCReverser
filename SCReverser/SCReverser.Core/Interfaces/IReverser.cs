using SCReverser.Core.Types;
using System.Collections.Generic;
using System.IO;

namespace SCReverser.Core.Interfaces
{
    public interface IReverser
    {
        /// <summary>
        /// Get instructions from byte array
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        IEnumerable<Instruction> GetInstructions(byte[] data, int index, int length);
        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">Leave open</param>
        IEnumerable<Instruction> GetInstructions(Stream stream, bool leaveOpen);
    }
}