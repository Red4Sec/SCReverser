using Neo;
using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.SmartContract;
using Neo.VM;
using Newtonsoft.Json;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Remembers;
using SCReverser.NEO.Internals;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SCReverser.NEO
{
    public class NeoConfig : RememberForm, IInitClassStream, IDisposable
    {
        string _BlockChainPath;

        /// <summary>
        /// Script
        /// </summary>
        public string Script { get; set; }
        /// <summary>
        /// Script
        /// </summary>
        public string VerificationScript { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        public ETriggerType TriggerType { get; set; } = ETriggerType.Application;
        /// <summary>
        /// Blockchain Path
        /// </summary>
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string BlockChainPath
        {
            get
            {
                return _BlockChainPath;
            }
            set
            {
                if (_BlockChainPath == value) return;

                _BlockChainPath = value;

                if (!EnableBlockChain) return;

                if (Blockchain.Default != null)
                    Blockchain.RegisterBlockchain(new NullBlockChain());

                if (!string.IsNullOrEmpty(value))
                {
                    if (!Directory.Exists(BlockChainPath))
                        Directory.CreateDirectory(BlockChainPath);

                    Blockchain.RegisterBlockchain(new LevelDBBlockchain(value));
                }
            }
        }

        [JsonIgnore]
        public bool EnableBlockChain { get; set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeoConfig() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="script">Script</param>
        public NeoConfig(byte[] script) : this()
        {
            Script = Convert.ToBase64String(script);
        }

        /// <summary>
        /// Create Engine from config
        /// </summary>
        public NeoEngine CreateEngine()
        {
            IScriptContainer container = null;

            DataCache<UInt160, AccountState> accounts;
            DataCache<ECPoint, ValidatorState> validators;
            DataCache<UInt256, AssetState> assets;
            DataCache<UInt160, ContractState> contracts;
            DataCache<StorageKey, StorageItem> storages;

            if (Blockchain.Default != null && !(Blockchain.Default is NullBlockChain))
            {
                // Real Blockchain

                accounts = Blockchain.Default.CreateCache<UInt160, AccountState>();
                validators = Blockchain.Default.CreateCache<ECPoint, ValidatorState>();
                assets = Blockchain.Default.CreateCache<UInt256, AssetState>();
                contracts = Blockchain.Default.CreateCache<UInt160, ContractState>();
                storages = Blockchain.Default.CreateCache<StorageKey, StorageItem>();
            }
            else
            {
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
        /// <summary>
        /// Get stream
        /// </summary>
        /// <param name="leaveOpen">Leave open</param>
        public Stream GetStream(out bool leaveOpen)
        {
            leaveOpen = false;

            try
            {
                // Convert from b64
                byte[] sc = Convert.FromBase64String(Script);
                if (sc != null) return new MemoryStream(sc);
            }
            catch { }

            if (UInt160.TryParse(Script, out UInt160 hash))
            {
                ContractState c = Blockchain.Default.GetContract(hash);
                return new MemoryStream(c.Script);
            }

            return File.OpenRead(Script);
        }

        #region Remember in form
        public override void GetValues(Form f)
        {
            base.GetValues(f);

            if (!(f is FOpen fo)) return;

            fo.txtScript.Text = Script;
            fo.txtVerification.Text = VerificationScript;
            fo.txtBlockChain.Text = BlockChainPath;
            fo.scriptType.SelectedItem = TriggerType;
        }
        public override void SaveValues(Form f)
        {
            base.SaveValues(f);

            if (!(f is FOpen fo)) return;

            Script = fo.txtScript.Text;
            VerificationScript = fo.txtVerification.Text;
            BlockChainPath = fo.txtBlockChain.Text;
            TriggerType = (ETriggerType)fo.scriptType.SelectedItem;
        }
        #endregion
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            if (!EnableBlockChain) return;
            Blockchain.RegisterBlockchain(new NullBlockChain());
        }
    }
}