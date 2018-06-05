using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using System.Drawing;

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
        public override TemplateFlags Flags => TemplateFlags.HaveAltStack;

        /// <summary>
        /// Open file
        /// </summary>
        public override object CreateNewConfig()
        {
            if (!FOpen.ShowForm(out NeoConfig cfg)) return null;
            return cfg;
        }
        /// <summary>
        /// Get logo
        /// </summary>
        public override Image GetLogo()
        {
            return Res.Logo;
        }
    }
}