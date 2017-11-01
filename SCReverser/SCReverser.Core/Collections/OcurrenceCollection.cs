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
        /// Control Params
        /// </summary>
        public OcurrenceParams ControlParams { get; set; }

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
        /// <param name="ins">Instructions</param>
        public void Append(string name, params Instruction[] ins)
        {
            if (ins == null || ins.Length <= 0) return;

            int il = ins.Length;
            foreach (Ocurrence a in this.ToArray())
                if (a.Value == name)
                {
                    a.Instructions.AddRange(ins);
                    return;
                }

            Ocurrence oc = new Ocurrence() { Value = name };
            oc.Instructions.AddRange(ins);

            Add(oc);
        }
    }
}