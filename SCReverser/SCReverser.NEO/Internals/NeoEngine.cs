using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using System;

namespace SCReverser.NEO.Internals
{
    public class NeoEngine : ApplicationEngine
    {
        const Decimal Ratio = 100000;

        public NeoEngine(TriggerType trigger, IScriptContainer container, IScriptTable table, InteropService service, Fixed8 gas, bool testMode = false)
            : base(trigger, container, table, service, gas, testMode)
        {
        }

        public Decimal GetPricePublic()
        {
            return (base.GetPrice() * Ratio) / (Decimal)Math.Pow(10, Blockchain.UtilityToken.Precision);
        }
    }
}