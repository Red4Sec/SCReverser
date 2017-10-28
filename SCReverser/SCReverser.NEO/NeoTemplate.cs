using System.Drawing;
using SCReverser.Core.Enums;
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
        /// Flags
        /// </summary>
        public override ETemplateFlags Flags => ETemplateFlags.HaveAltStack;
        /// <summary>
        /// Get logo
        /// </summary>
        public override Image GetLogo()
        {
            return Res.Logo;
        }
    }
}