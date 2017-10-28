using System;
using Neo.VM;
using SCReverser.Core.Extensions;
using SCReverser.Core.Helpers;
using System.Linq;

namespace SCReverser.NEO.Internals
{
    public class NeoStackItem : SCReverser.Core.Types.StackItem, IEquatable<NeoStackItem>
    {
        string _PrintableValue;

        public override string PrintableValue => _PrintableValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value</param>
        public NeoStackItem(Neo.VM.StackItem value)
        {
            Value = value;

            _PrintableValue = GetPrintable(value);
        }
        /// <summary>
        /// Get Printable version
        /// </summary>
        /// <param name="value">Value</param>
        string GetPrintable(StackItem value)
        {
            if (value == null) return "NULL";
            else
            {
                Type tp = value.GetType();

                switch (tp.FullName)
                {
                    case "Neo.VM.Types.Boolean": return value.GetBoolean().ToString();
                    case "Neo.VM.Types.ByteArray": return "0x" + value.GetByteArray().ToHexString();
                    case "Neo.VM.Types.Integer": return value.GetBigInteger().ToString();
                    case "Neo.VM.Types.Struct":
                    case "Neo.VM.Types.Array":
                        return JsonHelper.Serialize(value.GetArray().Select(u => GetPrintable(u)).ToArray(), true, false);

                    //case "Neo.VM.Types.InteropInterface ": return value.GetBigInteger().ToString();
                    default:
                        {
                            break;
                        }
                }
            }

            return "";
        }
        /// <summary>
        /// Check equals
        /// </summary>
        /// <param name="other">Other</param>
        public bool Equals(NeoStackItem other)
        {
            if (other == null) return false;
            return other.Value == Value;
        }
    }
}