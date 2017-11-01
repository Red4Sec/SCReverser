using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SCReverser.Core.Collections
{
    public class ModuleCollection : ObservableCollection<Module>
    {
        /// <summary>
        /// Get module of
        /// </summary>
        /// <param name="location">Location</param>
        public Module GetModuleOf(IndexOffset location)
        {
            foreach (Module m in this)
                if (location.IndexBetween(m.Start.Index, m.End.Index))
                    return m;

            return null;
        }

        /// <summary>
        /// Sort
        /// </summary>
        public void Sort()
        {
            Module[] mt = this.OrderBy(a => a.Start.Offset).ToArray();
            Clear();
            foreach (Module m in mt)
            {
                m.Methods.Sort();
                Add(m);
            }
        }

        /// <summary>
        /// Copy from
        /// </summary>
        /// <param name="modules">Modules</param>
        public void CopyFrom(IEnumerable<Module> modules)
        {
            foreach (Module i in modules)
                Add(i);
        }
    }
}