using System.Drawing;
using SCReverser.Core.Interfaces;

namespace SCReverser.NEO
{
    public class NeoTemplate : ReverseTemplate<NeoReverser, NeoDebugger, NeoConfig>
    {
        /// <summary>
        /// Template name
        /// </summary>
        public override string Template => "NEO";
        /// <summary>
        /// Get logo
        /// </summary>
        public override Image GetLogo()
        {
            return Res.Logo;
        }
    }
}