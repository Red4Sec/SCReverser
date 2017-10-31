using SCReverser.Core.Types;
using System.Collections.ObjectModel;
using System.Linq;

namespace SCReverser.Core.Collections
{
    public class MethodCollection : ObservableCollection<Method>
    {
        /// <summary>
        /// Parent
        /// </summary>
        public Module Parent { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public MethodCollection(Module parent)
        {
            Parent = parent;
        }

        protected override void InsertItem(int index, Method item)
        {
            if (item != null) item.Parent = Parent;
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Sort
        /// </summary>
        public void Sort()
        {
            Method[] mt = this.OrderBy(a => a.Start.Offset).ToArray();
            Clear();
            foreach (Method m in mt) Add(m);
        }

        /// <summary>
        /// Get method of
        /// </summary>
        /// <param name="location">Location</param>
        public Method GetMethodOf(IndexOffset location)
        {
            foreach (Method m in this)
                if (location.IndexBetween(m.Start.Index, m.End.Index))
                    return m;

            return null;
        }
    }
}