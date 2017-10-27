using SCReverser.Core.Interfaces;
using System.IO;
using System.Text;

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
        /// Ascii value
        /// </summary>
        public string ASCIIValue
        {
            get
            {
                if (RawValue != null)
                {
                    bool allAsciiPrintables = true;
                    foreach (byte b in RawValue)
                    {
                        if (char.IsControl((char)b) && !char.IsWhiteSpace((char)b))
                            allAsciiPrintables = false;
                    }

                    if (allAsciiPrintables)
                        return Encoding.ASCII.GetString(RawValue).Trim();
                }

                return "";
            }
        }
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