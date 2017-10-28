using SCReverser.Core.Helpers;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SCReverser.Core.Interfaces
{
    public class FRememberForm<T> : Form where T : IRemember
    {
        bool _CloseOnEscape = true;
        /// <summary>
        /// Close on Escape
        /// </summary>
        public bool CloseOnEscape { get { return _CloseOnEscape; } set { _CloseOnEscape = value; } }

        public FRememberForm() { KeyPreview = true; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            try
            {
                string json;
                if (FRememberForm.Remembers != null && FRememberForm.Remembers.TryGetValue(GetType().Name, out json) && json != null)
                {
                    T r = JsonHelper.Deserialize<T>(json);
                    if (r != null) OnGetValues(r);
                }
            }
            catch { }
        }
        protected virtual void OnGetValues(T sender) { sender.GetValues(this); }
        protected virtual void OnSaveValues(T sender) { sender.SaveValues(this); }
        protected override void OnClosed(EventArgs e)
        {
            try
            {
                T r = Activator.CreateInstance<T>();
                OnSaveValues(r);

                FRememberForm.Remembers[GetType().Name] = JsonHelper.Serialize(r, false, false);

                string file = Path.ChangeExtension(Application.ExecutablePath, ".rem");
                File.WriteAllText(file, JsonHelper.Serialize(FRememberForm.Remembers), Encoding.UTF8);
            }
            catch { }

            base.OnClosed(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (CloseOnEscape && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Close();
                return;
            }

            base.OnKeyDown(e);
        }
    }
}