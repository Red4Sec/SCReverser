namespace SCReverser.Core.Types
{
    public class OpCode
    {
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