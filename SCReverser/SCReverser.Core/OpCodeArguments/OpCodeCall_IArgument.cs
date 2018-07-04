using Newtonsoft.Json;
using SCReverser.Core.Extensions;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeCall_IArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public virtual byte RVcount
        {
            get { return RawValue[0]; }
            set { RawValue[0] = value; }
        }

        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public virtual byte Pcount
        {
            get { return RawValue[1]; }
            set { RawValue[1] = value; }
        }

        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public virtual short Value
        {
            get { return RawValue.ToInt16(2); }
            set
            {
                var ret = value.ToByteArray();
                RawValue[2] = ret[0];
                RawValue[3] = ret[1];
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeCall_IArgument() : base(4) { }
    }
}