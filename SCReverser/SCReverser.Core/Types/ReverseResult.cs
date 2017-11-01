using Newtonsoft.Json;
using SCReverser.Core.Collections;
using System.Linq;
using System.Threading.Tasks;
using System;
using SCReverser.Core.Enums;

namespace SCReverser.Core.Types
{
    public class ReverseResult
    {
        /// <summary>
        /// Bytes
        /// </summary>
        [JsonIgnore]
        public byte[] Bytes { get; set; }
        /// <summary>
        /// Modules
        /// </summary>
        public ModuleCollection Modules { get; private set; } = new ModuleCollection();
        /// <summary>
        /// Instructions
        /// </summary>
        public InstructionCollection Instructions { get; private set; } = new InstructionCollection();
        /// <summary>
        /// Ocurrences
        /// </summary>
        [JsonIgnore]
        public KeyValueCollection<string, OcurrenceCollection> Ocurrences { get; private set; } = new KeyValueCollection<string, OcurrenceCollection>();

        /// <summary>
        /// Calculate ocurrences
        /// </summary>
        public void GenerateOcurrences()
        {
            foreach (string key in Ocurrences.Keys.ToArray())
            {
                OcurrenceCollection ocur = Ocurrences[key];

                // Fill
                Parallel.ForEach(Instructions, (ins) =>
                {
                    if (ocur.Checker != null && ocur.Checker(ins, out string val))
                        lock (ocur) ocur.Append(val, ins);
                });

                // Clean
                if (Ocurrences[key].Count <= 0)
                    Ocurrences.Remove(key);
            }
        }
        /// <summary>
        /// Style borders instructions
        /// </summary>
        public void StyleMethodBorders()
        {
            foreach (Module mod in Modules) foreach (Method met in mod.Methods)
                {
                    met.Size = 0;
                    Instruction first = null;
                    foreach (Instruction i in Instructions.Take(met.Start, met.End))
                    {
                        if (first == null)
                        {
                            first = i;
                            i.Comment = met.Name;
                            first.BorderStyle = RowBorderStyle.EmptyBottom;
                        }
                        else
                        {
                            first = i;
                            first.BorderStyle = RowBorderStyle.OnlyLeftAndRight;
                        }
                        met.Size += i.Size;
                    }
                    if (first != null)
                    {
                        first.BorderStyle = first.BorderStyle == RowBorderStyle.EmptyBottom ? RowBorderStyle.All : RowBorderStyle.EmptyTop;
                    }
                }
        }
    }
}