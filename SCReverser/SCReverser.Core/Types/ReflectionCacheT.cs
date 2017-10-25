using System;
using System.Collections.Generic;

namespace SCReverser.Core.Types
{
    public class ReflectionCache<T> : Dictionary<T, Type>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReflectionCache() { }
        /// <summary>
        /// Create object from key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="def">Default value</param>
        public object CreateInstance(T key, object def = null)
        {
            Type tp;

            // Get Type from cache
            if (TryGetValue(key, out tp)) return Activator.CreateInstance(tp);

            // return null
            return def;
        }
        /// <summary>
        /// Create object from key
        /// </summary>
        /// <typeparam name="K">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="def">Default value</param>
        public K CreateInstance<K>(T key, K def = default(K))
        {
            Type tp;

            // Get Type from cache
            if (TryGetValue(key, out tp)) return (K)Activator.CreateInstance(tp);

            // return null
            return def;
        }
    }
}