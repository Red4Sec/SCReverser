namespace SCReverser.Core.Types
{
    public class OpCode
    {
        /// <summary>
        /// Value
        /// </summary>
        public byte[] RawValue { get; set; }
        /// <summary>
        /// OpCode name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// OpCode Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}