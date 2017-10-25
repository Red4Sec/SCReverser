namespace SCReverser.Core.Types
{
    public class InstructionLine
    {
        /// <summary>
        /// Offset
        /// </summary>
        public int Offset { get; set; }
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

            return arg;
        }
    }
}