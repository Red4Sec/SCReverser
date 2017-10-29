using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System.Collections.Generic;

namespace SCReverser.Core.Collections
{
    public class OffsetRelationCache
    {
        readonly Dictionary<uint, uint> _OffsetToIndex = new Dictionary<uint, uint>();
        readonly Dictionary<uint, uint> _IndexToOffset = new Dictionary<uint, uint>();

        /// <summary>
        /// Constructor
        /// </summary>
        public OffsetRelationCache() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instructions">Instruction</param>
        public OffsetRelationCache(IEnumerable<Instruction> instructions)
        {
            FillWith(instructions);
        }

        /// <summary>
        /// Try get value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="mode">Mode</param>
        public bool TryGetValue(uint key, out uint value, OffsetIndexRelation mode)
        {
            if (mode == OffsetIndexRelation.OffsetToIndex)
                return _OffsetToIndex.TryGetValue(key, out value);

            return _IndexToOffset.TryGetValue(key, out value);
        }
        /// <summary>
        /// Fill cache
        /// </summary>
        /// <param name="instructions">Instruction</param>
        public void FillWith(IEnumerable<Instruction> instructions)
        {
            foreach (Instruction i in instructions) Add(i);
        }
        /// <summary>
        /// Add instruction
        /// </summary>
        /// <param name="i">Instruction</param>
        public void Add(Instruction i)
        {
            _OffsetToIndex.Add(i.Offset, i.Index);
            _IndexToOffset.Add(i.Index, i.Offset);
        }
    }
}