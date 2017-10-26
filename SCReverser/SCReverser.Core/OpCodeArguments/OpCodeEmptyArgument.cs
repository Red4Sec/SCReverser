using SCReverser.Core.Interfaces;
using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    /// <summary>
    /// Empty OpCode argument
    /// </summary>
    public class OpCodeEmptyArgument : IWritable
    {
        /// <summary>
        /// Raw value
        /// </summary>
        public byte[] RawValue { get; protected set; }
        /// <summary>
        /// Read data from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public virtual uint Read(Stream stream)
        {
            return 0;
        }
        /// <summary>
        /// Write
        /// </summary>
        /// <param name="stream">Stream</param>
        public virtual uint Write(Stream stream)
        {
            int l = RawValue == null ? 0 : RawValue.Length;
            if (l > 0) stream.Write(RawValue, 0, l);

            return (uint)l;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return "";
        }
    }
}