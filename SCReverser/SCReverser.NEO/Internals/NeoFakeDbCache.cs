using Neo.IO;
using Neo.IO.Caching;
using System;
using System.Collections.Generic;

namespace SCReverser.NEO.Internals
{
    /// <summary>
    /// Fake DataCache for work without BlockChain
    /// </summary>
    /// <typeparam name="TKey">Key</typeparam>
    /// <typeparam name="TValue">Value</typeparam>
    public class NeoFakeDbCache<TKey, TValue> : DataCache<TKey, TValue>
       where TKey : IEquatable<TKey>, ISerializable
       where TValue : class, ICloneable<TValue>, ISerializable, new()
    {
        Dictionary<TKey, TValue> Dic = new Dictionary<TKey, TValue>();

        protected override IEnumerable<KeyValuePair<TKey, TValue>> FindInternal(byte[] key_prefix)
        {
            yield break;
        }

        protected override TValue GetInternal(TKey key)
        {
            if (!Dic.TryGetValue(key, out TValue ret))
                return default(TValue);

            return ret;
        }

        protected override TValue TryGetInternal(TKey key)
        {
            if (!Dic.TryGetValue(key, out TValue ret))
                return default(TValue);

            return ret;
        }

        public override void DeleteInternal(TKey key)
        {
            Dic.Remove(key);
        }

        protected override void AddInternal(TKey key, TValue value)
        {
            Dic[key] = value;
        }

        protected override void UpdateInternal(TKey key, TValue value)
        {
            Dic[key] = value;
        }
    }
}