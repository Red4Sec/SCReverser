using Newtonsoft.Json;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public byte Value
        {
            get { return RawValue[0]; }
            set { RawValue[0] = value; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeByteArgument() : base(1) { }
    }
}