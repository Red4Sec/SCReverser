using SCReverser.Core.Attributes;
using SCReverser.Core.Interfaces;
using SCReverser.NEO.OpCodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SCReverser.NEO
{
    public class NeoReverser : IReverser
    {
        static Dictionary<byte, OpCodeArgumentAttribute> OpCodeCache = new Dictionary<byte, OpCodeArgumentAttribute>();

        /// <summary>
        /// Static constructor for Cache OpCodes
        /// </summary>
        static NeoReverser()
        {
            Type enumType = typeof(OpCodeList);

            foreach (object t in Enum.GetValues(enumType))
            {
                // Get enumn member
                MemberInfo[] memInfo = enumType.GetMember(t.ToString());
                if (memInfo == null || memInfo.Length != 1)
                    throw (new FormatException());

                DescriptionAttribute desc = memInfo[0].GetCustomAttribute<DescriptionAttribute>();
                OpCodeArgumentAttribute opa = memInfo[0].GetCustomAttribute<OpCodeArgumentAttribute>();

                if (opa == null)
                    throw (new FormatException());

                if (desc != null && string.IsNullOrEmpty(opa.Description))
                    opa.Description = desc.Description;

                opa.OpCode = t.ToString();

                // Append to cache
                OpCodeCache.Add((byte)t, opa);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public NeoReverser() : base(OpCodeCache) { }
    }
}
