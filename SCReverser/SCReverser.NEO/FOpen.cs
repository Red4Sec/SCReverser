using SCReverser.Core.Interfaces;
using SCReverser.NEO.Internals;
using System;
using System.Windows.Forms;

namespace SCReverser.NEO
{
    public partial class FOpen : FOpenBase
    {
        /// <summary>
        /// Show form
        /// </summary>
        public static bool ShowForm(out NeoConfig config)
        {
            using (FOpen f = new FOpen())
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    config = null;
                    return false;
                }

                config = new NeoConfig();
                config.SaveValues(f);
                config.EnableBlockChain = true;

                config.BlockChainPath = f.txtBlockChain.Text;
            }

            return true;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        FOpen()
        {
            InitializeComponent();

            foreach (object o in Enum.GetValues(typeof(ETriggerType))) scriptType.Items.Add(o);
            scriptType.SelectedIndex = 0;
        }
        /// <summary>
        /// Search dialog
        /// </summary>
        void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            if (sender == button1) txtVerification.Text = openFileDialog1.FileName;
            else if (sender == button2) txtInvocation.Text = openFileDialog1.FileName;
        }
        void button3_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog1.SelectedPath = txtBlockChain.Text;
            }
            catch { }

            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            txtBlockChain.Text = folderBrowserDialog1.SelectedPath;
        }
    }
    public class FOpenBase : FRememberForm<NeoConfig> { }
}