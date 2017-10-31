using SCReverser.Core.Types;

namespace SCReverser.Core.Interfaces
{
    public interface IStartEnd
    {
        /// <summary>
        /// Start
        /// </summary>
        IndexOffset Start { get; }
        /// <summary>
        /// End
        /// </summary>
        IndexOffset End { get; }
    }
}