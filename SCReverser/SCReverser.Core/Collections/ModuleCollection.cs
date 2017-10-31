using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            {
                if (location.IndexBetween(m.Start.Index, m.End.Index))
                    return m;
            }

            return null;
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