using Neo;
using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.SmartContract;
using Neo.VM;
using SCReverser.NEO.Internals;

namespace SCReverser.NEO
{
    public class NeoDebuggerConfig
    {
        /// <summary>
        /// Trigger type
        /// </summary>
        public TriggerType TriggerType { get; set; } = TriggerType.Application;
        /// <summary>
        /// Blockchain Path
        /// </summary>
        public string BlockChainPath { get; set; } = null;

        /// <summary>
        /// Create Engine from config
        /// </summary>
        /// <param name="db">Return if have blockchain</param>
        public ApplicationEngine CreateEngine(out Blockchain db)
        {
            IScriptContainer container = null;

            DataCache<UInt160, AccountState> accounts;
            DataCache<ECPoint, ValidatorState> validators;
            DataCache<UInt256, AssetState> assets;
            DataCache<UInt160, ContractState> contracts;
            DataCache<StorageKey, StorageItem> storages;

            if (!string.IsNullOrEmpty(BlockChainPath))
            {
                db = new LevelDBBlockchain(BlockChainPath);

                // Real Blockchain

                accounts = db.CreateCache<UInt160, AccountState>();
                validators = db.CreateCache<ECPoint, ValidatorState>();
                assets = db.CreateCache<UInt256, AssetState>();
                contracts = db.CreateCache<UInt160, ContractState>();
                storages = db.CreateCache<StorageKey, StorageItem>();
            }
            else
            {
                db = null;

                // Fake Blockchain

                accounts = new NeoFakeDbCache<UInt160, AccountState>();
                validators = new NeoFakeDbCache<ECPoint, ValidatorState>();
                assets = new NeoFakeDbCache<UInt256, AssetState>();
                contracts = new NeoFakeDbCache<UInt160, ContractState>();
                storages = new NeoFakeDbCache<StorageKey, StorageItem>();
            }

            // Create Engine
            IScriptTable script_table = new CachedScriptTable(contracts);
            StateMachine service = new StateMachine(accounts, validators, assets, contracts, storages);

            return new ApplicationEngine(TriggerType, container, script_table, service, Fixed8.Zero, true);
        }
    }
}