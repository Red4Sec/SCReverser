using System.Text;

namespace SCReverser.Core.Extensions
{
    public class ByteArrayExtensions
    {
        /// <summary>
        /// Byte array to Hex string
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public static string ToHexString(byte[] buffer)
        {
            StringBuilder hex = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer) hex.Append(b.ToString("x2"));

            return hex.ToString();
        }
    }
}