using SCReverser.Core.Types;
using System.IO;
using System.Text;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArrayArgument : OpCodeArgument
    {
        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; protected set; }
        /// <summary>
        /// Value
        /// </summary>
        public byte[] Value { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length">Length</param>
        public OpCodeByteArrayArgument(int length)
        {
            Length = length;
            Value = new byte[length];
        }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            int lee = stream.Read(Value, 0, Length);
            if (lee != Length) throw (new EndOfStreamException());

            return (uint)lee;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            bool allAsciiPrintables = true;
            foreach (byte b in Value)
            {
                if (char.IsControl((char)b) && !char.IsWhiteSpace((char)b))
                {
                    allAsciiPrintables = false;
                }

                sb.Append(b.ToString("x2"));
            }

            if (allAsciiPrintables)
            {
                sb.AppendLine();
                sb.Append(Encoding.ASCII.GetString(Value).Trim());
            }

            return sb.ToString().Trim();
        }
    }
}