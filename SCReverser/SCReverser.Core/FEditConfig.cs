using SCReverser.Core.Interfaces;
using System.Windows.Forms;

namespace SCReverser.Core
{
    public partial class FEditConfig : FRememberForm
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="par">Config object</param>
        public static bool Configure(object par)
        {
            using (FEditConfig f = new FEditConfig())
            {
                f.propertyGrid1.SelectedObject = par;

                if (f.ShowDialog() != DialogResult.OK)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        FEditConfig()
        {
            InitializeComponent();
        }
    }
}