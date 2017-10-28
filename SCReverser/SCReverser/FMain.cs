using SCReverser.Controls;
using SCReverser.Core;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Helpers;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using SCReverser.NEO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCReverser
{
    public partial class FMain : FRememberForm
    {
        IReverser Reverser;
        IDebugger Debugger;
        IReverseTemplate Template;
        object CurrentConfig;
        ReverseResult Result;

        public FMain()
        {
            InitializeComponent();

            // Load available templates
            AddTemplate(typeof(NeoTemplate));

            // Auto select if only one
            if (formatToolStripMenuItem.DropDownItems.Count == 1)
            {
                openToolStripMenuItem_Click(formatToolStripMenuItem.DropDownItems[0], EventArgs.Empty);

                formatToolStripMenuItem.Visible = false;
                toolStripDropDownButton1.Visible = false;
            }

            EnableDisableConfig();
            EnableDisableDebugger();

            GridOpCode.AutoGenerateColumns = false;

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Load file for speed up the test
                LoadFiles(new string[] { Path.Combine(".", "SmartContractSample.avm") });
            }
#endif
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (Debugger != null) Debugger.Dispose();
        }
        void Instructions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            EnableDisableConfig();
            EnableDisableDebugger();

            tabControl1.Visible = Result != null && Result.Instructions.Count > 0;
        }
        void AddTemplate(Type type)
        {
            if (!typeof(IReverseTemplate).IsAssignableFrom(type))
                return;

            IReverseTemplate t = (IReverseTemplate)Activator.CreateInstance(type);

            toolStripDropDownButton1.DropDownItems.Add(t.Template, t.GetLogo(), openToolStripMenuItem_Click).Tag = t;
            formatToolStripMenuItem.DropDownItems.Add(t.Template, t.GetLogo(), openToolStripMenuItem_Click).Tag = t;
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        void EnableDisableConfig()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EnableDisableConfig));
                return;
            }

            bool enable = Template != null;

            openToolStripMenuItem.Enabled =
            toolStripButton1.Enabled =
            preferencesToolStripMenuItem.Enabled =
                toolStripButton2.Enabled = enable;
        }
        void EnableDisableDebugger()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EnableDisableDebugger));
                return;
            }

            if (Debugger != null)
            {
                Registers.SelectedObject = Debugger;
            }
            else
            {
                Registers.SelectedObject = null;
            }

            bool enable = Debugger != null && Template != null && Result != null && Result.Instructions.Count > 0;
            bool canplay = enable && !(Debugger.IsHalt || Debugger.IsError || Debugger.IsDisposed);

            // Play
            toolStripButton3.Enabled =
            executeToolStripMenuItem.Enabled = enable && canplay;

            // Stop
            stopToolStripMenuItem.Enabled =
            toolStripButton7.Enabled = enable && !canplay;

            // Steps
            stepIntoToolStripMenuItem.Enabled =
            stepOutToolStripMenuItem.Enabled =
            stepOverToolStripMenuItem.Enabled =
            toolStripButton4.Enabled =
            toolStripButton5.Enabled =
            toolStripButton6.Enabled = enable && canplay;

            if (enable && !canplay)
            {
                toolStripButton3.Visible = false;
                executeToolStripMenuItem.Visible = false;

                stopToolStripMenuItem.Visible = true;
                toolStripButton7.Visible = true;
            }
            else
            {
                toolStripButton3.Visible = true;
                executeToolStripMenuItem.Visible = true;

                stopToolStripMenuItem.Visible = false;
                toolStripButton7.Visible = false;
            }

            Error(null);
        }
        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IReverseTemplate tag = null;

            if (sender != null && sender is ToolStripItem ti) tag = (IReverseTemplate)ti.Tag;

            if (tag == null) return;

            if (Reverser != null) Reverser = null;
            if (Debugger != null)
            {
                Debugger.Dispose();
                Debugger = null;
            }

            if (Result != null)
            {
                Result.Instructions.Clear();
                Result = null;
            }

            Template = tag;

            Reverser = Template.CreateReverser();
            Reverser.OnParseProgress += OnReverseProgress;

            if (CurrentConfig == null || Template.ConfigType == null || CurrentConfig.GetType() != Template.ConfigType)
            {
                //preferencesToolStripMenuItem_Click(sender, e);
                CurrentConfig = Template.CreateNewConfig();
            }

            EnableDisableConfig();
            EnableDisableDebugger();
        }
        void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Template == null)
            {
                EnableDisableConfig();
                return;
            }

            try
            {
                if (CurrentConfig == null)
                    CurrentConfig = Template.CreateNewConfig();

                object clone = JsonHelper.Clone(CurrentConfig);

                if (FEditConfig.Configure(clone))
                    CurrentConfig = clone;
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Debugger == null)
                {
                    if (Template == null || Result == null || Result.Instructions == null)
                    {
                        EnableDisableConfig();
                        EnableDisableDebugger();
                        return;
                    }

                    // Create debugger
                    stopToolStripMenuItem_Click(sender, e);
                }

                Debugger.Execute();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void Debugger_OnStateChanged(object sender, DebuggerState oldState, DebuggerState newState)
        {
            EnableDisableDebugger();
        }
        void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Debugger != null) Debugger.StepInto();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Debugger != null) Debugger.StepOver();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void stepOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Debugger != null) Debugger.StepOut();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Debugger != null) Debugger.Dispose();

                Debugger = Template.CreateDebugger(Result.Instructions, CurrentConfig);
                Debugger.OnStateChanged += Debugger_OnStateChanged;
                EnableDisableDebugger();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void Error(Exception ex)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Exception>(Error), ex);
                return;
            }

            if (ex == null)
            {
                tsInfo.Text = "";
                return;
            }

            EnableDisableConfig();
            EnableDisableDebugger();

            tsInfo.Text = ex.Message;
            tsInfo.ForeColor = Color.Red;
            tsInfo.ToolTipText = ex.ToString();
        }
        void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (Template == null || Reverser == null)
            {
                EnableDisableConfig();
                EnableDisableDebugger();
                return;
            }

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            LoadFiles(openFileDialog1.FileNames);
        }
        void LoadFiles(IEnumerable<string> files)
        {
            tsProgressBar.Visible = true;

            Enabled = false;
            GridOpCode.DataSource = null;

            // Delete tabs except Instructions
            for (int x = tabControl1.TabCount - 1; x >= 2; x--)
            {
                TabPage t = tabControl1.TabPages[x];
                tabControl1.TabPages.Remove(t);
                t.Dispose();
            }

            if (Result != null)
            {
                Result.Instructions.CollectionChanged -= Instructions_CollectionChanged;
                Result.Instructions.Clear();
                Result = null;
            }

            new Task(() =>
            {
                ALoadFiles(files.ToArray());
            })
            .Start();
        }
        void ALoadFiles(params string[] files)
        {
            ReverseResult rs = new ReverseResult();

            try
            {
                // TODO Make real debugger

                foreach (string file in files)
                {
                    using (FileStream fs = File.OpenRead(file))
                    {
                        if (!Reverser.TryParse(fs, true, ref rs))
                            throw (new Exception("Error parsing the file"));
                    }

                    Hex.SetFile(file);
                }

                EndLoad(rs);
            }
            catch (Exception ex)
            {
                EndLoad(rs);
                Error(ex);
            }
        }
        void OnReverseProgress(object sender, int percent)
        {
            if (InvokeRequired)
            {
                Invoke(new OnProgressDelegate(OnReverseProgress), sender, percent);
                return;
            }

            tsProgressBar.Value = percent;
        }
        void EndLoad(ReverseResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ReverseResult>(EndLoad), result);
                return;
            }

            Result = result;

            if (result != null)
            {
                foreach (string sk in result.Ocurrences.Keys)
                {
                    TabPage t = new TabPage(sk)
                    {
                        Tag = "Ocurrence"
                    };
                    t.Controls.Add(new UCOcurrence(result.Ocurrences[sk], sk) { Dock = DockStyle.Fill });

                    tabControl1.TabPages.Add(t);
                }
            }

            // Create debugger
            stopToolStripMenuItem_Click(null, null);

            if (result != null && result.Instructions != null)
            {
                result.Instructions.CollectionChanged += Instructions_CollectionChanged;
                Instructions_CollectionChanged(result.Instructions, null);
            }
            GridOpCode.DataSource = result == null ? null : result.Instructions;

            tsProgressBar.Visible = false;
            Enabled = true;
        }

        void tsInfo_Click(object sender, EventArgs e)
        {
            if (tsInfo.Text == "") return;

            if (tsInfo.ForeColor == Color.Red)
            {
                MessageBox.Show(tsInfo.ToolTipText, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage t = tabControl1.SelectedTab;
            if (t == null || t.Tag == null) return;

            if ((string)t.Tag == "Ocurrence")
            {
                // Focus Search Textbox
                t.Controls[0].Focus();
            }
        }
    }
}