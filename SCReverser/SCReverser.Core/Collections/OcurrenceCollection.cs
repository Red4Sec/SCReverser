using Newtonsoft.Json;
using SCReverser.Core.Delegates;
using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Linq;

namespace SCReverser.Core.Collections
{
    public class OcurrenceCollection : List<Ocurrence>
    {
        /// <summary>
        /// Check method
        /// </summary>
        [JsonIgnore]
        public OnCheckOcurrenceDelegate Checker { get; set; }

        /// <summary>
        /// Copy ordered to
        /// </summary>
        /// <param name="to">To</param>
        public void CopyOrderedTo(IList<Ocurrence> to)
        {
            foreach (Ocurrence o in this.OrderByDescending(u => u.Count))
                to.Add(o);
        }
        /// <summary>
        /// Get ordered list
        /// </summary>
        public IEnumerable<Ocurrence> GetOrdered()
        {
            return this.OrderByDescending(u => u.Count);
        }
        /// <summary>
        /// Append ocurrence
        /// </summary>
        /// <param name="name">Arg</param>
        /// <param name="value">Value</param>
        public void Append(string name, int value)
        {
            if (value <= 0) return;

            foreach (Ocurrence a in this.ToArray())
            {
                if (a.Value == name)
                {
                    a.Count += value;

                    if (a.Count <= 0)
                        Remove(a);

                    return;
                }
            }

            Add(new Ocurrence() { Value = name, Count = value });
        }
    }
}