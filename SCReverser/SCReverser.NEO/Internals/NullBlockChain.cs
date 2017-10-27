using Neo;
using Neo.Core;
using Neo.IO.Caching;
using System.Collections.Generic;

namespace SCReverser.NEO.Internals
{
    public class NullBlockChain : Blockchain
    {
        public override UInt256 CurrentBlockHash => UInt256.Zero;

        public override UInt256 CurrentHeaderHash => UInt256.Zero;

        public override uint HeaderHeight => 0;

        public override uint Height => 0;

        public override bool AddBlock(Block block)
        {
            return false;
        }

        public override bool ContainsBlock(UInt256 hash)
        {
            return false;
        }

        public override bool ContainsTransaction(UInt256 hash)
        {
            return false;
        }

        public override bool ContainsUnspent(UInt256 hash, ushort index)
        {
            return false;
        }

        public override DataCache<TKey, TValue> CreateCache<TKey, TValue>()
        {
            return new NeoFakeDbCache<TKey, TValue>();
        }

        public override void Dispose()
        {
        }

        public override AccountState GetAccountState(UInt160 script_hash)
        {
            return null;
        }

        public override AssetState GetAssetState(UInt256 asset_id)
        {
            return null;
        }

        public override Block GetBlock(UInt256 hash)
        {
            return null;
        }

        public override UInt256 GetBlockHash(uint height)
        {
            return UInt256.Zero;
        }

        public override ContractState GetContract(UInt160 hash)
        {
            return null;
        }

        public override IEnumerable<ValidatorState> GetEnrollments()
        {
            yield break;
        }

        public override Header GetHeader(uint height)
        {
            return null;
        }

        public override Header GetHeader(UInt256 hash)
        {
            return null;
        }

        public override Block GetNextBlock(UInt256 hash)
        {
            return null;
        }

        public override UInt256 GetNextBlockHash(UInt256 hash)
        {
            return UInt256.Zero;
        }

        public override StorageItem GetStorageItem(StorageKey key)
        {
            return null;
        }

        public override long GetSysFeeAmount(UInt256 hash)
        {
            return 0;
        }

        public override Transaction GetTransaction(UInt256 hash, out int height)
        {
            height = 0;
            return null;
        }

        public override Dictionary<ushort, SpentCoin> GetUnclaimed(UInt256 hash)
        {
            return new Dictionary<ushort, SpentCoin>();
        }

        public override TransactionOutput GetUnspent(UInt256 hash, ushort index)
        {
            return null;
        }

        public override IEnumerable<VoteState> GetVotes(IEnumerable<Transaction> others)
        {
            yield break;
        }

        public override bool IsDoubleSpend(Transaction tx)
        {
            return false;
        }

        protected override void AddHeaders(IEnumerable<Header> headers)
        {
        }
    }
}
