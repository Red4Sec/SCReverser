using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArrayFromByteArgument : OpCodeByteArrayArgument
    {
        public override uint Size => base.Size + 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeByteArrayFromByteArgument() : base(0) { }

        public override uint Read(Stream stream)
        {
            int r = stream.ReadByte();
            RawValue = new byte[r];
            return base.Read(stream) + 1;
        }

        public override uint Write(Stream stream)
        {
            stream.WriteByte((byte)RawValue.Length);
            return base.Write(stream);
        }
    }
}