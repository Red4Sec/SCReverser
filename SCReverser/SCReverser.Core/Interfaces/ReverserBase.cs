using SCReverser.Core.Attributes;
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
        /// Get instructions from byte array
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        public IEnumerable<Instruction> GetInstructions(byte[] data, int index, int length)
        {
            return GetInstructions(new MemoryStream(data, index, length), false);
        }
        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">Leave open</param>
        public virtual IEnumerable<Instruction> GetInstructions(Stream stream, bool leaveOpen)
        {
            uint insNumber = 0;
            uint offset = 0;

            while (true)
            {
                byte[] opCode = new byte[OpCodeSize];

                if (stream.Read(opCode, 0, OpCodeSize) != OpCodeSize)
                    break;

                string key = opCode.ToHexString();

                OpCodeArgumentAttribute read;
                if (!OpCodeCache.TryGetValue(key, out read))
                    throw (new OpCodeNotFoundException()
                    {
                        Offset = offset,
                        OpCode = opCode,
                    });

                OpCodeEmptyArgument arg = read.Create();
                uint rBytes = arg.Read(stream);

                yield return new Instruction()
                {
                    Index = insNumber,
                    Offset = offset,
                    OpCode = new OpCode()
                    {
                        RawValue = opCode,
                        Name = read.OpCode,
                        Description = read.Description
                    },
                    Argument = arg,
                };

                offset += rBytes + 1;
                insNumber++;
            }

            if (!leaveOpen)
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }
}
