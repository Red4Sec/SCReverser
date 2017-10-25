using SCReverser.Core.Attributes;
using SCReverser.Core.Exceptions;
using SCReverser.Core.OpCodeArguments;
using SCReverser.Core.Types;
using System.Collections.Generic;
using System.IO;

namespace SCReverser.Core.Interfaces
{
    public class IReverser
    {
        /// <summary>
        /// OpCache
        /// </summary>
        Dictionary<byte, OpCodeArgumentAttribute> OpCodeCache;

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

                OpCodeEmptyArgument arg = read.Create();
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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache">OpCache</param>
        protected IReverser(Dictionary<byte, OpCodeArgumentAttribute> cache)
        {
            OpCodeCache = cache;
        }
    }
}
