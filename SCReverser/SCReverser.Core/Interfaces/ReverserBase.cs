using SCReverser.Core.Attributes;
using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Exceptions;
using SCReverser.Core.Extensions;
using SCReverser.Core.OpCodeArguments;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SCReverser.Core.Interfaces
{
    public class ReverserBase<T> : IReverser
        where T : struct, IConvertible
    {
        /// <summary>
        /// OnProgress
        /// </summary>
        public event OnProgressDelegate OnParseProgress;
        /// <summary>
        /// OpCode Size
        /// </summary>
        public readonly int OpCodeSize;
        /// <summary>
        /// OpCache
        /// </summary>
        Dictionary<string, OpCodeArgumentAttribute> OpCodeCache;
        /// <summary>
        /// Constructor
        /// </summary>
        protected ReverserBase()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum) throw (new ArgumentException("T"));

            // Get type and size from underlying type
            Type enumTypeBase = Enum.GetUnderlyingType(enumType);
            OpCodeSize = Marshal.SizeOf(enumTypeBase);
            OpCodeCache = new Dictionary<string, OpCodeArgumentAttribute>();

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

                //  Get byte array from underlying type type
                byte[] d;
                object value = Convert.ChangeType(t, enumTypeBase);

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Byte: d = new byte[] { (byte)value }; break;
                    case TypeCode.Int16: d = ((short)value).ToByteArray(); break;
                    case TypeCode.UInt16: d = ((ushort)value).ToByteArray(); break;
                    case TypeCode.Int32: d = ((int)value).ToByteArray(); break;
                    case TypeCode.UInt32: d = ((uint)value).ToByteArray(); break;
                    case TypeCode.Int64: d = ((long)value).ToByteArray(); break;
                    case TypeCode.UInt64: d = ((ulong)value).ToByteArray(); break;

                    default: throw (new ArgumentException("T"));
                }

                // Append to cache
                OpCodeCache.Add(d.ToHexString(), opa);
            }
        }
        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="result">Result</param>
        public virtual void PrapareResultForOcurrences(ReverseResult result)
        {
            if (result == null) return;

            if (!result.Ocurrences.ContainsKey("Strings"))
                result.Ocurrences["Strings"] = new OcurrenceCollection() { Checker = StringCheckOcurrence };
            if (!result.Ocurrences.ContainsKey("OpCodes"))
                result.Ocurrences["OpCodes"] = new OcurrenceCollection() { Checker = OpCodesCheckOcurrence };
        }
        /// <summary>
        /// Check if instruction have OpCode
        /// </summary>
        /// <param name="i">Instruction</param>
        /// <param name="name">OpCode name</param>
        bool OpCodesCheckOcurrence(Instruction i, out string name)
        {
            name = i.OpCode.Name;
            return !string.IsNullOrEmpty(name);
        }
        /// <summary>
        /// Check if instruction have ASCII value
        /// </summary>
        /// <param name="i">Instruction</param>
        /// <param name="name">ASCII value</param>
        bool StringCheckOcurrence(Instruction i, out string name)
        {
            name = i.Argument == null ? "" : i.Argument.ASCIIValue;
            return !string.IsNullOrEmpty(name);
        }
        /// <summary>
        /// Get instructions from byte array
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        /// <param name="result">Result</param> 
        public bool TryParse(byte[] data, int index, int length, ref ReverseResult result)
        {
            return TryParse(new MemoryStream(data, index, length), false, ref result);
        }
        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">Leave open</param>
        /// <param name="result">Result</param> 
        public virtual bool TryParse(Stream stream, bool leaveOpen, ref ReverseResult result)
        {
            uint insNumber = 0;
            uint offset = 0;

            if (result == null) result = new ReverseResult() { };
            PrapareResultForOcurrences(result);

            long max = stream.Length;
            int percent = 0, newPercent = 0;

            while (true)
            {
                byte[] opCode = new byte[OpCodeSize];

                if (stream.Read(opCode, 0, OpCodeSize) != OpCodeSize)
                    break;

                string key = opCode.ToHexString();

                OpCodeArgumentAttribute read;
                if (!OpCodeCache.TryGetValue(key, out read) || read == null)
                    throw (new OpCodeNotFoundException()
                    {
                        Offset = offset,
                        OpCode = opCode,
                    });

                OpCodeEmptyArgument arg = read.Create();
                uint rBytes = arg.Read(stream);

                Instruction ins = new Instruction()
                {
                    Index = insNumber,
                    Offset = offset,
                    OpCode = new OpCode()
                    {
                        RawValue = opCode,
                        Name = read.OpCode,
                        Description = read.Description,
                    },
                    Argument = arg,
                    Comment = arg.ASCIIValue,
                };
                result.Instructions.Add(ins);

                #region Fill ocurrences
                foreach (OcurrenceCollection ocur in result.Ocurrences.Values)
                {
                    if (ocur.Checker != null && ocur.Checker(ins, out string val))
                        ocur.Append(val, 1);
                }
                #endregion

                offset += (uint)(rBytes + OpCodeSize);
                insNumber++;

                newPercent = (int)((offset * 100) / max);
                if (percent != newPercent)
                {
                    percent = newPercent;
                    OnParseProgress?.Invoke(this, percent);
                }
            }

            if (!leaveOpen)
            {
                stream.Close();
                stream.Dispose();
            }

            return result.Instructions.Count > 0;
        }
    }
}
