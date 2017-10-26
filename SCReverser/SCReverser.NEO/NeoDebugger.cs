using Neo;
using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.SmartContract;
using Neo.VM;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using System;
using System.IO;

namespace SCReverser.NEO
{
    public class NeoDebugger : DebuggerBase
    {
        bool HaveBlockChain;
        NeoDebuggerConfig Config;
        ApplicationEngine Engine;

        /// <summary>
        /// Gas consumed
        /// </summary>
        public string GasConsumed { get; private set; }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="instructions">Instructions</param>
        public NeoDebugger(Instruction[] instructions) : base(instructions) { }
        /// <summary>
        /// Initialize
        /// </summary>
        public bool Initialize(NeoDebuggerConfig config)
        {
            // Set config
            if (config == null) config = new NeoDebuggerConfig();

            Config = config;

            // Create script
            byte[] script;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (Instruction i in Instructions) i.Write(ms);
                script = ms.ToArray();
            }

            // Prepare engine

            IScriptContainer container = null;
            
            // Internal object
            Type cachedScriptTable = typeof(ECPoint).Assembly.GetType("Neo.SmartContract.CachedScriptTable");

            DataCache<UInt160, AccountState> accounts;
            DataCache<ECPoint, ValidatorState> validators;
            DataCache<UInt256, AssetState> assets;
            DataCache<UInt160, ContractState> contracts;
            DataCache<StorageKey, StorageItem> storages;

            if (!string.IsNullOrEmpty(config.BlockChainPath))
            {
                HaveBlockChain = true;
                Blockchain.RegisterBlockchain(new LevelDBBlockchain(config.BlockChainPath));

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
            IScriptTable script_table = (IScriptTable)Activator.CreateInstance(cachedScriptTable, contracts);
            StateMachine service = new StateMachine(accounts, validators, assets, contracts, storages);
            Engine = new ApplicationEngine(Config.TriggerType, container, script_table, service, Fixed8.Zero, true);

            // Load script
            Engine.LoadScript(script, false);

            if (Config != null && Engine != null)
            {
                // Start
                State |= DebuggerState.Initialized;
                return true;
            }
            else
            {
                State |= DebuggerState.Error;
            }
            return false;
        }
        /// <summary>
        /// Step into
        /// </summary>
        public override void StepInto()
        {
            if (AreEnded()) return;

            // Remove breakpoint
            State &= ~DebuggerState.BreakPoint;

            if (Engine != null)
            {
                try
                {
                    Engine.StepInto();

                    // Copy state
                    if (Engine.State.HasFlag(VMState.HALT))
                        State |= DebuggerState.Ended;
                    if (Engine.State.HasFlag(VMState.FAULT))
                        State |= DebuggerState.Error;

                    // Copy registers
                    GasConsumed = Engine.GasConsumed.ToString();
                    InvocationStackCount = (uint)Engine.InvocationStack.Count;
                    CurrentInstructionIndex = (uint)Engine.CurrentContext.InstructionPointer;
                }
                catch (Exception e)
                {
                    // Error
                    State |= DebuggerState.Error;
                    throw (e);
                }
            }
            else
            {
                State |= DebuggerState.Disposed;
            }
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            // Clean engine
            Engine.Dispose();
            Engine = null;

            // Free blockchain
            if (HaveBlockChain && Blockchain.Default != null)
            {
                HaveBlockChain = false;
                Blockchain.Default.Dispose();
            }
        }
    }
}