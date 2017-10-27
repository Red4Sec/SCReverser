using System.IO;
using System.Text;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArrayArgument : OpCodeEmptyArgument
    {
        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length">Length</param>
        public OpCodeByteArrayArgument(int length)
        {
            Length = length;
            RawValue = new byte[length];
        }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            int lee = stream.Read(RawValue, 0, Length);
            if (lee != Length) throw (new EndOfStreamException());

            return (uint)lee;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in RawValue)
                sb.Append(b.ToString("X2"));

            //string asc = ASCIIValue;
            //if (!string.IsNullOrEmpty(asc))
            //{
            //    sb.AppendLine();
            //    sb.Append(asc);
            //}

            return sb.ToString();//.Trim();
        }
    }
}