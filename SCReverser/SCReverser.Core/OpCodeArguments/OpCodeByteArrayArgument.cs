using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArrayArgument : OpCodeEmptyArgument
    {
        /// <summary>
        /// Length
        /// </summary>
        [JsonIgnore]
        public int Length { get { return RawValue == null ? 0 : RawValue.Length; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length">Length</param>
        public OpCodeByteArrayArgument(int length)
        {
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

            if (RawValue != null && RawValue.Length > 0)
            {
                sb.Append("0x");

                foreach (byte b in RawValue)
                    sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}