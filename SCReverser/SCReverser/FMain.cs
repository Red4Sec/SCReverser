using SCReverser.Core;
using SCReverser.Core.Enums;
using SCReverser.Core.Helpers;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using SCReverser.NEO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCReverser
{
    public partial class FMain : Form
    {
        IReverser Reverser;
        IDebugger Debugger;
        IReverseTemplate Template;
        object CurrentConfig;
        ObservableCollection<Instruction> Instructions = new ObservableCollection<Instruction>();

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
        void Instructions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            EnableDisableConfig();
            EnableDisableDebugger();

            tabControl1.Visible = Instructions.Count > 0;
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

            bool enable = Debugger != null && Template != null && Instructions.Count > 0;
            bool canplay = enable &&
                Debugger.IsInitialized &&
                !(Debugger.IsHalt || Debugger.IsError || Debugger.IsDisposed);

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

            Instructions.Clear();

            Template = tag;

            Reverser = Template.CreateReverser();

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
                    if (Template == null || Instructions == null)
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

                Debugger = Template.CreateDebugger(Instructions, CurrentConfig);
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

            GridOpCode.DataSource = null;
            GridStrings.DataSource = null;
            GridOpCodes.DataSource = null;
            GridSysCalls.DataSource = null;

            Instructions.CollectionChanged -= Instructions_CollectionChanged;
            Instructions.Clear();

            new Task(() =>
            {
                ALoadFiles(files.ToArray());
            })
            .Start();
        }
        void ALoadFiles(params string[] files)
        {
            try
            {
                // TODO Progress bar
                // Make real debugger
                // Filters
                // Charts

                Dictionary<string, uint> dicStrings = new Dictionary<string, uint>();
                Dictionary<string, uint> dicOpCodes = new Dictionary<string, uint>();
                Dictionary<string, uint> dicSysCalls = new Dictionary<string, uint>();

                foreach (string file in files)
                    using (FileStream fs = File.OpenRead(file))
                        foreach (Instruction i in Reverser.GetInstructions(fs, true))
                        {
                            string arg = null;
                            if (i.Argument != null)
                            {
                                arg = i.Argument.ASCIIValue;

                                if (!string.IsNullOrEmpty(arg))
                                {
                                    if (dicStrings.ContainsKey(arg)) dicStrings[arg] += 1;
                                    else dicStrings[arg] = 1;
                                }
                            }

                            if (i.OpCode != null)
                            {
                                string asc = i.OpCode.Name;
                                if (!string.IsNullOrEmpty(asc))
                                {
                                    if (dicOpCodes.ContainsKey(asc)) dicOpCodes[asc] += 1;
                                    else dicOpCodes[asc] = 1;

                                    if (!string.IsNullOrEmpty(arg) && i.OpCode.IsSysCall)
                                    {
                                        if (dicSysCalls.ContainsKey(arg)) dicSysCalls[arg] += 1;
                                        else dicSysCalls[arg] = 1;
                                    }
                                }
                            }

                            Instructions.Add(i);
                        }

                EndLoad(

                    dicStrings.Select(u => new Ocurrence()
                    {
                        Value = u.Key,
                        Count = u.Value
                    })
                    .OrderByDescending(u => u.Count)
                    .ToArray(),

                    dicOpCodes.Select(u => new Ocurrence()
                    {
                        Value = u.Key,
                        Count = u.Value
                    })
                    .OrderByDescending(u => u.Count)
                    .ToArray(),

                    dicSysCalls.Select(u => new Ocurrence()
                    {
                        Value = u.Key,
                        Count = u.Value
                    })
                    .OrderByDescending(u => u.Count)
                    .ToArray());
            }
            catch (Exception ex)
            {
                Error(ex);
            }

        }
        void EndLoad(Ocurrence[] strings, Ocurrence[] opCodes, Ocurrence[] sysCalls)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Ocurrence[], Ocurrence[], Ocurrence[]>(EndLoad), strings, opCodes, sysCalls);
                return;
            }

            GridStrings.DataSource = strings;
            GridOpCodes.DataSource = opCodes;
            GridSysCalls.DataSource = sysCalls;

            // Create debugger
            stopToolStripMenuItem_Click(null, null);

            Instructions.CollectionChanged += Instructions_CollectionChanged;
            Instructions_CollectionChanged(Instructions, null);

            GridOpCode.DataSource = Instructions;

            tsProgressBar.Visible = false;
        }
        void tsInfo_Click(object sender, EventArgs e)
        {
            if (tsInfo.Text == "") return;

            if (tsInfo.ForeColor == Color.Red)
            {
                MessageBox.Show(tsInfo.ToolTipText, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}