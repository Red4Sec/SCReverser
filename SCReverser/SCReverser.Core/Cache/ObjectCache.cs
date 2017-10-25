using System.Collections.Generic;

namespace SCReverser.Core.Cache
{
    public class ObjectCache<T, K> : Dictionary<T, K>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectCache() { }
    }
}