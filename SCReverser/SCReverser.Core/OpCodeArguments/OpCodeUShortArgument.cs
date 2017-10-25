using SCReverser.Core.Extensions;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeUShortArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        public ushort Value
        {
            get
            {
                return RawValue.ToUInt16();
            }
            set
            {
                RawValue = value.ToByteArray();
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeUShortArgument() : base(2) { }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}