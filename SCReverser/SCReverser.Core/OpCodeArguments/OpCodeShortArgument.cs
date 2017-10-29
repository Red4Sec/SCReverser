using Newtonsoft.Json;
using SCReverser.Core.Extensions;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeShortArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public virtual short Value
        {
            get { return RawValue.ToInt16(); }
            set { RawValue = value.ToByteArray(); }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeShortArgument() : base(2) { }
    }
}