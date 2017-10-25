using SCReverser.Core.Extensions;
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
        public ulong MaxLength { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxLength">Max Length</param>
        public OpCodeVarByteArrayArgument(ulong maxLength) : base(0)
        {
            MaxLength = maxLength;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxLength">Max Length</param>
        public OpCodeVarByteArrayArgument(int maxLength) : base(0)
        {
            MaxLength = (ulong)maxLength;
        }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            int read;
            ulong l = ReadVarInt(stream, out read, MaxLength);

            Value = new byte[l];
            int lee = stream.Read(Value, 0, Value.Length);
            if (lee != Value.Length)
                throw (new EndOfStreamException());

            read += lee;
            return (uint)read;
        }
        /// <summary>
        /// Read Var Int logic from NEO
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="max">Max</param>
        public static ulong ReadVarInt(Stream reader, out int read, ulong max = ulong.MaxValue)
        {
            int fb = reader.ReadByte();
            if (fb < 0) throw (new EndOfStreamException());

            read = 1;

            ulong value;

            if (fb == 0xFD)
            {
                value = reader.ReadUInt16(); read += 2;
            }
            else if (fb == 0xFE)
            {
                value = reader.ReadUInt32(); read += 4;
            }
            else if (fb == 0xFF)
            {
                value = reader.ReadUInt64(); read += 8;
            }
            else value = (ulong)fb;

            if (value > max) throw new FormatException();
            return value;
        }
    }
}