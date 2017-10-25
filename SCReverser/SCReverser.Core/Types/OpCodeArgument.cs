using System.IO;

namespace SCReverser.Core.Types
{
    /// <summary>
    /// Empty OpCode argument
    /// </summary>
    public class OpCodeArgument
    {
        /// <summary>
        /// Raw value
        /// </summary>
        public byte[] RawValue { get; protected set; }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return "";
        }

        /// <summary>
        /// Read data from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public virtual uint Read(Stream stream)
        {
            return 0;
        }
    }
}