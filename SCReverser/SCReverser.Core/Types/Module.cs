using System.Drawing;

namespace SCReverser.Core.Types
{
    public class Module
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Hash
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// Start
        /// </summary>
        public IndexOffset Start { get; set; }
        /// <summary>
        /// End
        /// </summary>
        public IndexOffset End { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.Empty;

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}