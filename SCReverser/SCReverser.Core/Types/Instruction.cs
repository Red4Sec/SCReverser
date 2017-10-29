using SCReverser.Core.Interfaces;
using SCReverser.Core.OpCodeArguments;
using System.IO;

namespace SCReverser.Core.Types
{
    public class Instruction : IWritable
    {
        /// <summary>
        /// Instruction index
        /// </summary>
        public uint Index { get; set; }
        /// <summary>
        /// Offset
        /// </summary>
        public uint Offset { get; set; }
        /// <summary>
        /// Offset Hex
        /// </summary>
        public string OffsetHex { get { return "0x" + Offset.ToString("X4"); } }
        /// <summary>
        /// OpCode
        /// </summary>
        public OpCode OpCode { get; set; }
        /// <summary>
        /// Argument
        /// </summary>
        public OpCodeEmptyArgument Argument { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Have BreakPoint
        /// </summary>
        public bool HaveBreakPoint { get; set; }
        /// <summary>
        /// Jump
        /// </summary>
        public Jump Jump { get; set; }

        /// <summary>
        /// Write instruction
        /// </summary>
        /// <param name="stream">Stream</param>
        public virtual uint Write(Stream stream)
        {
            uint l = 0;

            // Write OpCode
            if (OpCode != null) l += OpCode.Write(stream);

            // Write arguments
            if (Argument != null) l += Argument.Write(stream);

            return l;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            string arg = Argument == null ? "" : Argument.ToString();

            if (string.IsNullOrEmpty(arg)) arg = OpCode.ToString();
            else arg = OpCode.ToString() + " [" + arg + "]";

            if (!string.IsNullOrEmpty(Comment))
                arg += " # " + Comment;

            return "[0x" + Offset.ToString("X4").PadLeft(4) + "] " + arg;
        }
    }
}
