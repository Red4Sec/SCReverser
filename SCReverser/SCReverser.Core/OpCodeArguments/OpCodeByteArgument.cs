namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        public byte Value
        {
            get { return RawValue[0]; }
            set { RawValue[0] = value; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeByteArgument() : base(1) { }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Value.ToString("X2");
        }
    }
}