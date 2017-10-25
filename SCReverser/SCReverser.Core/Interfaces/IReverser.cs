using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace SCReverser.Core.Interfaces
{
    public class IReverser
    {
        /// <summary>
        /// Get instructions from byte array
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        public IEnumerable<Instruction> GetInstructions(byte[] data, int index, int length)
        {
            return GetInstructions(new MemoryStream(data, index, length), false);
        }
        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">Leave open</param>
        public virtual IEnumerable<Instruction> GetInstructions(Stream sr, bool leaveOpen)
        {
            throw (new NotImplementedException());
        }
        /// <summary>
        /// Constructor
        /// </summary>
        protected IReverser () { }
    }
}
