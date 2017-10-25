using SCReverser.Core.OpCodeArguments;
using System;
using System.IO;

namespace SCReverser.NEO.OpCodeArguments
{
    public class OpCodeVarByteArrayArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Max Length
        /// </summary>
        public int MaxLength { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxLength">Max Length</param>
        public OpCodeVarByteArrayArgument(int maxLength) : base(0)
        {
            MaxLength = maxLength;

            // Wait for read
            //Value = new byte[maxLength];
        }

        public static ulong ReadVarInt(BinaryReader reader, ulong max = ulong.MaxValue)
        {
            byte fb = reader.ReadByte();
            ulong value;
            if (fb == 0xFD)
                value = reader.ReadUInt16();
            else if (fb == 0xFE)
                value = reader.ReadUInt32();
            else if (fb == 0xFF)
                value = reader.ReadUInt64();
            else
                value = fb;
            if (value > max) throw new FormatException();

            return value;
        }
    }
}