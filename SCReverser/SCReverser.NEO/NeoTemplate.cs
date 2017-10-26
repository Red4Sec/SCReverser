using SCReverser.Core.Interfaces;

namespace SCReverser.NEO
{
    public class NeoTemplate : ReverseTemplate<NeoReverser, NeoDebugger>
    {
        /// <summary>
        /// Template name
        /// </summary>
        public override string Template => "NEO";
    }
}