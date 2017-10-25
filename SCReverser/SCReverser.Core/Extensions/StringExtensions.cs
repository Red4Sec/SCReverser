using System;

namespace SCReverser.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Hex string to byte array
        /// </summary>
        /// <param name="input">Input</param>
        public static byte[] HexToByteArray(this string input)
        {
            if (input == null) return null;

            // Remove 0x
            if (input.StartsWith("0X", StringComparison.InvariantCultureIgnoreCase))
                input = input.Substring(2);

            int chars = input.Length;

            byte[] bytes = new byte[chars / 2];
            for (int i = 0; i < chars; i += 2)
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);

            return bytes;
        }
    }
}