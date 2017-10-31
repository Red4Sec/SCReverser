using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SCReverser.Core.Collections
{
    public class InstructionCollection : ObservableCollection<Instruction>
    {
        /// <summary>
        /// Cache offset - instruction index
        /// </summary>
        readonly OffsetRelationCache OffsetCache = new OffsetRelationCache();

        /// <summary>
        /// Get instruction by index
        /// </summary>
        /// <param name="instructionIndex">Instruction index</param>
        public Instruction this[uint instructionIndex]
        {
            get
            {
                if (Count <= instructionIndex) return null;
                return base[(int)instructionIndex];
            }
        }
        /// <summary>
        /// Get instruction by index
        /// </summary>
        /// <param name="instructionIndex">Instruction index</param>
        public new Instruction this[int instructionIndex]
        {
            get
            {
                if (Count <= instructionIndex) return null;
                return base[instructionIndex];
            }
        }
        /// <summary>
        /// Cache when add
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">Item</param>
        protected override void InsertItem(int index, Instruction item)
        {
            OffsetCache.Add(item.Location);

            base.InsertItem(index, item);
        }
        /// <summary>
        /// Index to Offset
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="offset">Offset</param>
        public bool IndexToOffset(uint index, out uint offset)
        {
            return OffsetCache.TryGetValue(index, out offset, OffsetIndexRelation.IndexToOffset);
        }
        /// <summary>
        /// Offset to Index
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="index">Index</param>
        public bool OffsetToIndex(uint offset, out uint index)
        {
            return OffsetCache.TryGetValue(offset, out index, OffsetIndexRelation.OffsetToIndex);
        }
        /// <summary>
        /// Take between
        /// </summary>
        /// <param name="start">Start</param>
        /// <param name="end">End</param>
        public IEnumerable<Instruction> Take(IndexOffset start, IndexOffset end)
        {
            for (int x = (int)start.Index, m = (int)end.Index; x <= m; x++)
                yield return base[x];
        }
        /// <summary>
        /// Take enter
        /// </summary>
        /// <param name="start">Start</param>
        public IEnumerable<Instruction> Take(IndexOffset start)
        {
            for (int x = (int)start.Index, m = Count; x < m; x++)
                yield return base[x];
        }
        /// <summary>
        /// Copy from
        /// </summary>
        /// <param name="instructions">Instructions</param>
        public void CopyFrom(IEnumerable<Instruction> instructions)
        {
            foreach (Instruction i in instructions)
                Add(i);
        }
    }
}