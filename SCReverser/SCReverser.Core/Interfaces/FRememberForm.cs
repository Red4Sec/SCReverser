using SCReverser.Core.Helpers;
using SCReverser.Core.Remembers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SCReverser.Core.Interfaces
{
    public class FRememberForm : FRememberForm<RememberForm>
    {
        internal static Dictionary<string, string> Remembers;

        static FRememberForm()
        {
            if (Remembers != null) return;

            try
            {
                string file = Path.ChangeExtension(Application.ExecutablePath, ".rem");

                if (File.Exists(file))
                    try
                    {
                        string json = File.ReadAllText(file, Encoding.UTF8);
                        if (!string.IsNullOrEmpty(json))
                        {
                            Remembers = JsonHelper.Deserialize<Dictionary<string, string>>(json);
                        }
                    }
                    catch { }

            }
            catch { }

            if (Remembers == null) Remembers = new Dictionary<string, string>();
        }
        public FRememberForm() : base() { }
    }
}