using System;

namespace SCReverser.Core.Exceptions
{
    public class OpCodeNotFoundException : Exception
    {
        /// <summary>
        /// Offset
        /// </summary>
        public uint Offset { get; set; }
        /// <summary>
        /// OpCode
        /// </summary>
        public byte[] OpCode { get; set; }
    }
}