using SCReverser.Core.Remembers;
using System;
using System.Windows.Forms;

namespace SCReverser
{
    public class RemMain : RememberForm
    {
        /// <summary>
        /// Splitter distance
        /// </summary>
        public int SplitterHexDistance { get; set; }
        /// <summary>
        /// Splitter distance
        /// </summary>
        public int SplitterInstructionsDistance { get; set; }

        public override void GetValues(Form f)
        {
            base.GetValues(f);

            if (f is FMain fm)
            {
                fm.TreeModules.Width = Math.Min(fm.Width - 300, SplitterHexDistance);
                fm.PanelRegisters.Width = Math.Max(250, SplitterInstructionsDistance);
            }
        }

        public override void SaveValues(Form f)
        {
            base.SaveValues(f);

            if (f is FMain fm)
            {
                SplitterHexDistance = fm.TreeModules.Width;
                SplitterInstructionsDistance = fm.PanelRegisters.Width;
            }
        }
    }
}