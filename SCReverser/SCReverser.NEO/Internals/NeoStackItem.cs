using System;
using Neo.VM;
using SCReverser.Core.Extensions;
using SCReverser.Core.Helpers;
using System.Linq;
using System.Reflection;
using Neo.SmartContract;
using System.Numerics;

namespace SCReverser.NEO.Internals
{
    public class NeoStackItem : SCReverser.Core.Types.StackItem, IEquatable<NeoStackItem>
    {
        string _PrintableValue;
        static FieldInfo _Interop;
        static Type _StorageContext;
        static MethodInfo _StorageContextToArray;

        public override string PrintableValue => _PrintableValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value</param>
        public NeoStackItem(Neo.VM.StackItem value)
        {
            Value = value;

            _PrintableValue = GetPrintable(value).ToString();
        }
        /// <summary>
        /// Get Printable version
        /// </summary>
        /// <param name="value">Value</param>
        object GetPrintable(StackItem value)
        {
            if (value == null) return "NULL";
            else
            {
                Type tp = value.GetType();

                switch (tp.FullName)
                {
                    case "Neo.VM.Types.Boolean": return value.GetBoolean();
                    case "Neo.VM.Types.ByteArray":
                        {
                            byte[] v = value.GetByteArray();
                            if (v == null || v.Length == 0)
                                return "[0]";

                            if (v.Length > 200) return "[" + v.Length.ToString() + "] ...";

                            return "[" + v.Length.ToString() + "] 0x" + v.ToHexString();
                        }
                    case "Neo.VM.Types.Integer":
                        {
                            BigInteger v = value.GetBigInteger();

                            if (v > ulong.MaxValue) return "[BigInteger] ...";

                            return v.ToString();
                        }
                    case "Neo.VM.Types.Struct":
                    case "Neo.VM.Types.Array":
                        {
                            StackItem[] si = value.GetArray();

                            if (si.Length > 200) return "[" + si.Length.ToString() + "] ...";
                            return
                                "[" + si.Length + "]" +
                                JsonHelper.Serialize(si.Select(u => GetPrintable(u)).ToArray(), true, false);
                        }
                    case "Neo.VM.Types.InteropInterface":
                        {
                            if (_Interop == null)
                            {
                                _Interop = tp.GetField("_object", BindingFlags.Instance | BindingFlags.NonPublic);

                                foreach (Type txp in typeof(ApplicationEngine).Assembly.GetTypes())
                                {
                                    // Extract internal type
                                    if (txp.FullName == "Neo.SmartContract.StorageContext")
                                    {
                                        _StorageContext = txp;
                                        _StorageContextToArray = txp.GetMethod("ToArray");
                                        break;
                                    }
                                }
                            }

                            object ob = _Interop.GetValue(value);

                            if (ob != null && ob.GetType() == _StorageContext)
                                ob = _StorageContextToArray.Invoke(ob, new object[] { });
                            else
                            {

                            }
                            //if (ob !=null && ob is UInt160 a)

                            return JsonHelper.Serialize(ob, true, false);
                        }
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