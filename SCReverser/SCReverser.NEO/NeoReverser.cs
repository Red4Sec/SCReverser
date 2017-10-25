using SCReverser.Core.Attributes;
using SCReverser.Core.Cache;
using SCReverser.Core.Exceptions;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using SCReverser.NEO.OpCodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace SCReverser.NEO
{
    public class NeoReverser : IReverser
    {
        static ObjectCache<byte, OpCodeArgumentAttribute> OpCodeCache = new ObjectCache<byte, OpCodeArgumentAttribute>();

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
        /// Get instructions from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">Leave open</param>
        public override IEnumerable<Instruction> GetInstructions(Stream stream, bool leaveOpen)
        {
            int opCode;
            uint insNumber = 0;
            uint offset = 0;

            while ((opCode = stream.ReadByte()) != -1)
            {
                OpCodeArgumentAttribute read;
                if (!OpCodeCache.TryGetValue((byte)opCode, out read))
                    throw (new OpCodeNotFoundException()
                    {
                        Offset = offset,
                        OpCode = opCode,
                    });

                OpCodeArgument arg = read.Create();
                uint rBytes = arg.Read(stream);

                yield return new Instruction()
                {
                    InstructionNumber = insNumber,
                    Offset = offset,
                    OpCode = new OpCode()
                    {
                        RawValue = new byte[] { (byte)opCode },
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