using SCReverser.Core.Extensions;
using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArrayFromIntArgument : OpCodeByteArrayArgument
    {
        public override uint Size => base.Size + 4;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeByteArrayFromIntArgument() : base(4) { }

        public override uint Read(Stream stream)
        {
            RawValue = new byte[4];
            stream.Read(RawValue, 0, 4);

            RawValue = new byte[RawValue.ToInt32()];
            return base.Read(stream) + 4;
        }

        public override uint Write(Stream stream)
        {
            stream.Write(RawValue.Length.ToByteArray(), 0, 4);
            return base.Write(stream) + 4;
        }
    }
}