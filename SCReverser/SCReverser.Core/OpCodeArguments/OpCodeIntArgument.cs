using SCReverser.Core.Extensions;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeIntArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        public int Value
        {
            get
            {
                return RawValue.ToInt32(0);
            }
            set
            {
                RawValue = value.ToByteArray();
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeIntArgument() : base(4) { }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}