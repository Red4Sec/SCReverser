using Newtonsoft.Json;
using SCReverser.Core.Extensions;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeUShortArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public virtual ushort Value
        {
            get { return RawValue.ToUInt16(); }
            set { RawValue = value.ToByteArray(); }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeUShortArgument() : base(2) { }
    }
}