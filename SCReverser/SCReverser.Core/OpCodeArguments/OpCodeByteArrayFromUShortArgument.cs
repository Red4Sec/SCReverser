using SCReverser.Core.Extensions;
using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArrayFromUShortArgument : OpCodeByteArrayArgument
    {
        public override uint Size => base.Size + 2;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeByteArrayFromUShortArgument() : base(2) { }

        public override uint Read(Stream stream)
        {
            RawValue = new byte[2];
            stream.Read(RawValue, 0, 2);

            RawValue = new byte[RawValue.ToUInt16()];
            return base.Read(stream) + 2;
        }

        public override uint Write(Stream stream)
        {
            stream.Write(((ushort)RawValue.Length).ToByteArray(), 0, 2);
            return base.Write(stream) + 2;
        }
    }
}