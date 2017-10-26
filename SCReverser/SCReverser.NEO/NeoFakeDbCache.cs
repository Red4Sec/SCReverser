using Neo.IO.Caching;
using System;
using System.Collections.Generic;

namespace SCReverser.NEO
{
    /// <summary>
    /// Fake DataCache for work without BlockChain
    /// </summary>
    /// <typeparam name="TKey">Key</typeparam>
    /// <typeparam name="TValue">Value</typeparam>
    public class NeoFakeDbCache<TKey, TValue> : DataCache<TKey, TValue>
       where TKey : IEquatable<TKey>, Neo.IO.ISerializable
        where TValue : class, Neo.IO.ISerializable, new()
    {
        Dictionary<TKey, TValue> Dic = new Dictionary<TKey, TValue>();

        protected override IEnumerable<KeyValuePair<TKey, TValue>> FindInternal(byte[] key_prefix)
        {
            yield break;
        }

        protected override TValue GetInternal(TKey key)
        {
            TValue ret;
            if (!Dic.TryGetValue(key, out ret))
                return default(TValue);

            return ret;
        }

        protected override TValue TryGetInternal(TKey key)
        {
            TValue ret;
            if (!Dic.TryGetValue(key, out ret))
                return default(TValue);

            return ret;
        }
    }
}