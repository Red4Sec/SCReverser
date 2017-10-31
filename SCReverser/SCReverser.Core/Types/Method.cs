using Newtonsoft.Json;
using SCReverser.Core.Interfaces;
using System.Drawing;

namespace SCReverser.Core.Types
{
    public class Method: IStartEnd
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
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
        /// Size
        /// </summary>
        public uint Size { get; set; }
        /// <summary>
        /// Parent
        /// </summary>
        [JsonIgnore]
        public Module Parent { get; internal set; } = null;
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}