namespace SCReverser.Core.Types
{
    public class Instruction
    {
        /// <summary>
        /// Instruction number
        /// </summary>
        public uint InstructionNumber { get; set; }
        /// <summary>
        /// Offset
        /// </summary>
        public uint Offset { get; set; }
        /// <summary>
        /// OpCode
        /// </summary>
        public OpCode OpCode { get; set; }
        /// <summary>
        /// Argument
        /// </summary>
        public OpCodeArgument Argument { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            string arg = Argument == null ? "" : Argument.ToString();

            if (string.IsNullOrEmpty(arg)) arg = OpCode.ToString();
            arg = OpCode.ToString() + " [" + arg + "]";

            if (!string.IsNullOrEmpty(Comment))
                arg += " # " + Comment;

            return "[" + Offset.ToString("x2").PadLeft(4) + "] " + arg;
        }
    }
}