using SCReverser.Core.Collections;
using SCReverser.Core.Interfaces;
using System.Drawing;

namespace SCReverser.Core.Types
{
    public class Module: IStartEnd
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
        /// Size
        /// </summary>
        public uint Size { get; set; }
        /// <summary>
        /// Method Collection
        /// </summary>
        public MethodCollection Methods { get; private set; }
        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public Module()
        {
            Methods = new MethodCollection(this);
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}