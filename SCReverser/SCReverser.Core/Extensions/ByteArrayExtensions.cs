using System.Text;

namespace SCReverser.Core.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Byte array to Hex string
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public static string ToHexString(this byte[] buffer)
        {
            StringBuilder hex = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer) hex.Append(b.ToString("x2"));

            return hex.ToString();
        }

        public static ushort ToUInt16(this byte[] buffer, int index = 0)
        {
            return (ushort)((int)buffer[index] | (int)buffer[index + 1] << 8);
        }
        public static short ToInt16(this byte[] buffer, int index = 0)
        {
            return (short)((int)buffer[index] | (int)buffer[index + 1] << 8);
        }

        public static int ToInt32(this byte[] buffer, int index = 0)
        {
            return ((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
        }
        public static uint ToUInt32(this byte[] buffer, int index = 0)
        {
            return (uint)((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
        }

        public static ulong ToUInt64(this byte[] buffer, int index = 0)
        {
            uint num = (uint)((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
            uint num2 = (uint)((int)buffer[index + 4] | (int)buffer[index + 5] << 8 | (int)buffer[index + 6] << 16 | (int)buffer[index + 7] << 24);
            return (ulong)num2 << 32 | (ulong)num;
        }
        public static long ToInt64(this byte[] buffer, int index = 0)
        {
            uint num = (uint)((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
            uint num2 = (uint)((int)buffer[index + 4] | (int)buffer[index + 5] << 8 | (int)buffer[index + 6] << 16 | (int)buffer[index + 7] << 24);
            return (long)num2 << 32 | (long)num;
        }
    }
}