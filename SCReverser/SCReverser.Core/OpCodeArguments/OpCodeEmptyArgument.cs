using Newtonsoft.Json;
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
        /// Size
        /// </summary>
        [JsonIgnore]
        public virtual uint Size
        {
            get
            {
                return RawValue == null ? 0 : (uint)RawValue.Length;
            }
        }
        /// <summary>
        /// Raw value
        /// </summary>
        public byte[] RawValue { get; set; }
        /// <summary>
        /// Ascii value
        /// </summary>
        [JsonIgnore]
        public string ASCIIValue
        {
            get
            {
                if (RawValue != null)
                {
                    foreach (byte b in RawValue)
                        if (char.IsControl((char)b) && !char.IsWhiteSpace((char)b))
                            return "";

                    string ret = Encoding.ASCII.GetString(RawValue).Trim();
                    return string.IsNullOrEmpty(ret.Trim('?')) ? "" : ret;
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
