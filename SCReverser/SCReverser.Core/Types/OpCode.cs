using System;
using System.IO;

namespace SCReverser.Core.Types
{
    public class OpCode
    {
        /// <summary>
        /// Value
        /// </summary>
        public byte[] RawValue { get; set; }
        /// <summary>
        /// OpCode name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// OpCode Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Write OpCode
        /// </summary>
        /// <param name="stream">Stream</param>
        public int Write(Stream stream)
        {
            int l = RawValue.Length;
            stream.Write(RawValue, 0, l);
            return l;
        }
    }
}