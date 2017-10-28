using SCReverser.Core.Delegates;
using SCReverser.Core.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SCReverser.Core.Collections
{
    public class OcurrenceCollection : IEnumerable<Ocurrence>
    {
        Dictionary<string, Ocurrence> _List = new Dictionary<string, Ocurrence>();

        /// <summary>
        /// Count
        /// </summary>
        public int Count { get { return _List.Count; } }

        public IEnumerator<Ocurrence> GetEnumerator() { return _List.Values.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _List.Values.GetEnumerator(); }

        /// <summary>
        /// Check method
        /// </summary>
        public OnCheckOcurrenceDelegate Checker { get; set; }

        /// <summary>
        /// Copy ordered to
        /// </summary>
        /// <param name="to">To</param>
        public void CopyOrderedTo(IList<Ocurrence> to)
        {
            foreach (Ocurrence o in _List.Values.OrderByDescending(u => u.Count))
                to.Add(o);
        }
        /// <summary>
        /// Get ordered list
        /// </summary>
        public IEnumerable<Ocurrence> GetOrdered()
        {
            return _List.Values.OrderByDescending(u => u.Count);
        }
        /// <summary>
        /// Append ocurrence
        /// </summary>
        /// <param name="name">Arg</param>
        /// <param name="value">Value</param>
        public void Append(string name, int value)
        {
            if (value == 0) return;

            Ocurrence a;
            if (_List.TryGetValue(name, out a))
            {
                a.Count += value;
                if (a.Count <= 0)
                    _List.Remove(name);
            }
            else _List[name] = new Ocurrence() { Value = name, Count = value };
        }
    }
}