using Neo;
using Neo.Core;
using Neo.IO.Caching;
using Neo.VM;

namespace SCReverser.NEO.Internals
{
    public class CachedScriptTable : IScriptTable
    {
        DataCache<UInt160, ContractState> contracts;

        public CachedScriptTable(DataCache<UInt160, ContractState> contracts)
        {
            this.contracts = contracts;
        }

        byte[] IScriptTable.GetScript(byte[] script_hash)
        {
            return contracts[new UInt160(script_hash)].Script;
        }
    }
}