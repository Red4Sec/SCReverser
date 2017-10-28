using Neo;
using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.SmartContract;
using Neo.VM;
using SCReverser.NEO.Internals;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SCReverser.NEO
{
    public class NeoConfig
    {
        /// <summary>
        /// Trigger type
        /// </summary>
        public ETriggerType TriggerType { get; set; } = ETriggerType.Application;
        /// <summary>
        /// Blockchain Path
        /// </summary>
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string BlockChainPath { get; set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeoConfig()
        {
            BlockChainPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "NeoBlockchain");
        }

        /// <summary>
        /// Create Engine from config
        /// </summary>
        /// <param name="db">Return if have blockchain</param>
        public NeoEngine CreateEngine(out Blockchain db)
        {
            IScriptContainer container = null;

            DataCache<UInt160, AccountState> accounts;
            DataCache<ECPoint, ValidatorState> validators;
            DataCache<UInt256, AssetState> assets;
            DataCache<UInt160, ContractState> contracts;
            DataCache<StorageKey, StorageItem> storages;

            if (!string.IsNullOrEmpty(BlockChainPath))
            {
                if (!Directory.Exists(BlockChainPath))
                    Directory.CreateDirectory(BlockChainPath);

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

            TriggerType t;

            switch (TriggerType)
            {
                case ETriggerType.Application: t = Neo.SmartContract.TriggerType.Application; break;
                case ETriggerType.Verification: t = Neo.SmartContract.TriggerType.Verification; break;
                default: return null;
            }

            return new NeoEngine(t, container, script_table, service, Fixed8.Zero, true);
        }
    }
}