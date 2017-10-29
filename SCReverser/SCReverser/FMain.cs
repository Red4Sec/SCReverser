using SCReverser.Controls;
using SCReverser.Core;
using SCReverser.Core.Collections;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCReverser
{
    public partial class FMain : FRememberForm
    {
        string _LastSaveFile;

        public string LastSaveFile
        {
            get { return _LastSaveFile; }
            set
            {
                _LastSaveFile = value;
                if (string.IsNullOrEmpty(_LastSaveFile))
                {
                    Text = "SCReverser";
                    saveToolStripMenuItem.Enabled = false;
                }
                else
                {
                    Text = "SCReverser [" + _LastSaveFile + "]";
                }
            }
        }

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
            GridStack.AutoGenerateColumns = false;
            GridAltStack.AutoGenerateColumns = false;

            StackAlt_OnChange(null, null);

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Load file for speed up the test
                LoadFiles(Path.Combine(".", "SmartContractSample.avm"));
            }
#endif
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            CleanDebugger();
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

            saveToolStripMenuItem.Enabled = enable && !string.IsNullOrEmpty(LastSaveFile);

            toolStripButton8.Enabled = openToolStripMenuItem.Enabled =
            toolStripButton1.Enabled = preferencesToolStripMenuItem.Enabled =
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
            CleanDebugger();

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
        void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Debugger != null)
                {
                    Debugger.StepInto();
                }
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
                if (Debugger != null)
                {
                    Debugger.StepOver();
                }
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
                if (Debugger != null)
                {
                    Debugger.StepOut();
                }
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
                CleanDebugger();

                Debugger = Template.CreateDebugger(Result.Instructions, CurrentConfig);

                Debugger.OnStateChanged += Debugger_OnStateChanged;
                Debugger.OnInstructionChanged += Debugger_OnInstructionChanged;
                Debugger.Stack.OnChange += Stack_OnChange;
                Debugger.AltStack.OnChange += StackAlt_OnChange;

                EnableDisableDebugger();
                Debugger_OnInstructionChanged(null, 0);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void Stack_OnChange(object sender, EventArgs e)
        {
            GridStack.DataSource = Debugger == null ? null : Debugger.Stack.ToArray();
            GridStack.ClearSelection();
        }
        void StackAlt_OnChange(object sender, EventArgs e)
        {
            GridAltStack.DataSource = Debugger == null ? null : Debugger.AltStack.ToArray();
            GridAltStack.ClearSelection();
            SplitStack.RowStyles[1].Height = Debugger == null || Debugger.AltStack.Count <= 0 ? 0F : 50F;
        }
        void CleanDebugger()
        {
            if (Debugger == null) return;

            Debugger.Stack.OnChange -= Stack_OnChange;
            Debugger.AltStack.OnChange -= StackAlt_OnChange;
            Debugger.OnStateChanged -= Debugger_OnStateChanged;
            Debugger.OnInstructionChanged -= Debugger_OnInstructionChanged;

            Debugger.Dispose();
            Debugger = null;

            Stack_OnChange(null, null);
            StackAlt_OnChange(null, null);
            Debugger_OnInstructionChanged(null, 0);
        }

        void Debugger_OnStateChanged(object sender, DebuggerState oldState, DebuggerState newState)
        {
            EnableDisableDebugger();
        }
        void Debugger_OnInstructionChanged(object sender, uint instructionIndex)
        {
            GridOpCode.Invalidate();
            Registers.Refresh();

            GridOpCode.ClearSelection();
            GridOpCode.Rows[(int)instructionIndex].Selected = true;
            GridOpCode.CurrentCell = GridOpCode.Rows[(int)instructionIndex].Cells[3];

            Jumps.RefreshDynJumps(Debugger);
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

            LoadFiles(openFileDialog1.FileName);
        }
        void LoadFiles(string file)
        {
            tsProgressBar.Visible = true;

            Enabled = false;
            GridOpCode.DataSource = null;
            LastSaveFile = null;

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
                ALoadFiles(file);
            })
            .Start();
        }
        void ALoadFiles(string file)
        {
            bool useLastFile = false;
            ReverseResult rs = null;

            try
            {
                switch (Path.GetExtension(file).ToLowerInvariant())
                {
                    case ".json":
                        {
                            rs = JsonHelper.Deserialize<ReverseResult>(File.ReadAllText(file, Encoding.UTF8), true);
                            if (rs != null)
                            {
                                // Fill cache
                                OffsetRelationCache offsetToIndexCache = new OffsetRelationCache(rs.Instructions);

                                // Process instructions (Jumps)
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    foreach (Instruction i in rs.Instructions)
                                    {
                                        Reverser.ProcessInstruction(i, offsetToIndexCache);
                                        i.Write(ms);
                                    }
                                    rs.Bytes = ms.ToArray();
                                }
                            }
                            useLastFile = true;
                            break;
                        }
                    default:
                        {
                            rs = new ReverseResult();

                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (FileStream fs = File.OpenRead(file))
                                    fs.CopyTo(ms);

                                ms.Seek(0, SeekOrigin.Begin);

                                if (!Reverser.TryParse(ms, true, ref rs))
                                    throw (new Exception("Error parsing the file"));
                            }
                            break;
                        }
                }

                EndLoad(useLastFile ? file : null, rs);
            }
            catch (Exception ex)
            {
                EndLoad(useLastFile ? file : null, rs);
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
        void EndLoad(string file, ReverseResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, ReverseResult>(EndLoad), file, result);
                return;
            }

            Result = result;

            if (result != null)
            {
                LastSaveFile = file;
                Hex.SetBytes(result.Bytes);

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
            else
            {
                Hex.SetBytes(new byte[] { });
            }

            GridOpCode.DataSource = result == null ? null : result.Instructions;

            // Create debugger
            stopToolStripMenuItem_Click(null, null);

            if (result != null && result.Instructions != null)
            {
                result.Instructions.CollectionChanged += Instructions_CollectionChanged;
                Instructions_CollectionChanged(result.Instructions, null);
            }

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
        /// <summary>
        /// Create breakpoint
        /// </summary>
        void GridOpCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                foreach (DataGridViewRow r in GridOpCode.SelectedRows)
                {
                    if (r.IsNewRow || r.DataBoundItem == null) continue;

                    if (r.DataBoundItem is Instruction i)
                    {
                        i.HaveBreakPoint = !i.HaveBreakPoint;
                        GridOpCode.InvalidateRow(r.Index);
                    }
                }
            }
        }
        void GridOpCode_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = GridOpCode.Rows[e.RowIndex];
            if (r == null || r.IsNewRow || r.DataBoundItem == null) return;

            if (!(r.DataBoundItem is Instruction i)) return;

            e.PaintCells(e.RowBounds, DataGridViewPaintParts.All);
            e.Handled = true;

            if (Debugger != null && Debugger.CurrentInstructionIndex == i.Index)
            {
                bool error = Debugger.IsError;

                using (Brush br = new SolidBrush(Color.FromArgb(30, error ? Color.DarkRed : Color.Lime)))
                {
                    e.Graphics.FillRectangle(br, e.RowBounds);
                    e.Graphics.DrawRectangle(error ? Pens.Red : Pens.Green,
                        e.RowBounds.X, e.RowBounds.Y, e.RowBounds.Width - 1, e.RowBounds.Height - 1);
                }
            }

            if (i.HaveBreakPoint)
            {
                using (Brush br = new SolidBrush(Color.FromArgb(50, Color.Red)))
                {
                    e.Graphics.FillRectangle(br, e.RowBounds);
                }
            }
        }
        void GridOpCode_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 1) return;

            DataGridViewRow r = GridOpCode.Rows[e.RowIndex];
            if (r == null || r.IsNewRow || r.DataBoundItem == null) return;

            if (!(r.DataBoundItem is Instruction i)) return;

            DataGridViewCell cell = r.Cells[e.ColumnIndex];

            cell.ToolTipText = i.OpCode.Description;
        }
        void FMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;

                        if (toolStripButton3.Enabled && toolStripButton3.Visible)
                            executeToolStripMenuItem_Click(sender, e);
                        else
                        if (toolStripButton7.Enabled && toolStripButton7.Visible)
                            stopToolStripMenuItem_Click(sender, e);

                        break;
                    }
                case Keys.F6:
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;

                        if (toolStripButton4.Enabled && toolStripButton4.Visible)
                            stepIntoToolStripMenuItem_Click(sender, e);
                        break;
                    }
                case Keys.F7:
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;

                        if (toolStripButton5.Enabled && toolStripButton5.Visible)
                            stepOverToolStripMenuItem_Click(sender, e);
                        break;
                    }
                case Keys.F8:
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;

                        if (toolStripButton6.Enabled && toolStripButton6.Visible)
                            stepOutToolStripMenuItem_Click(sender, e);
                        break;
                    }
            }
        }
        void GridOpCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = GridOpCode.HitTest(e.X, e.Y);
                GridOpCode.ClearSelection();
                if (hti.RowIndex < 0) return;

                GridOpCode.Rows[hti.RowIndex].Selected = true;
            }
        }
        void goHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Debugger == null || GridOpCode.SelectedRows.Count != 1) return;

            DataGridViewRow r = GridOpCode.SelectedRows[0];
            if (r.DataBoundItem == null || !(r.DataBoundItem is Instruction i)) return;

            Debugger.CurrentInstruction = i;
        }

        void GridOpCode_CurrentCellChanged(object sender, EventArgs e) { Jumps.Invalidate(); }
        void GridOpCode_Scroll(object sender, ScrollEventArgs e) { Jumps.Invalidate(); }

        void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Template == null || Result == null) return;

            if (string.IsNullOrEmpty(LastSaveFile) || sender == saveAsToolStripMenuItem)
            {
                // Save as
                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                LastSaveFile = saveFileDialog1.FileName;
            }

            if (string.IsNullOrEmpty(LastSaveFile)) return;

            try
            {
                File.WriteAllText(LastSaveFile, JsonHelper.Serialize(Result, true, false), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LastSaveFile = null;
                Error(ex);
            }
        }
    }
}