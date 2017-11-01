using Be.Windows.Forms;
using SCReverser.Controls;
using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Helpers;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using SCReverser.NEO;
using System;
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
                    Text = "SCReverser by Red4Sec";
                    saveToolStripMenuItem.Enabled = false;
                }
                else
                {
                    Text = "SCReverser by Red4Sec [" + _LastSaveFile + "]";
                }
            }
        }

        IReverser Reverser;
        IDebugger Debugger;
        IReverseTemplate Template;
        object Config;
        ReverseResult Result;

        public FMain()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Load available templates
            AddTemplate(typeof(NeoTemplate));


            EnableDisableConfig();
            EnableDisableDebugger();

            GridOpCode.AutoGenerateColumns = false;
            GridStack.AutoGenerateColumns = false;
            GridAltStack.AutoGenerateColumns = false;

            StackAlt_OnChange(null, null);


            // Auto select if only one
            if (formatToolStripMenuItem.DropDownItems.Count == 1)
            {
                openToolStripMenuItem_Click(formatToolStripMenuItem.DropDownItems[0], EventArgs.Empty);

                formatToolStripMenuItem.Visible = false;
                toolStripDropDownButton1.Visible = false;
                openToolStripMenuItem_Click_1(null, null);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            CleanDebugger();

            if (Config != null && Config is IDisposable d)
                d.Dispose();
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
            toolStripButton1.Enabled = enable;
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

            if (Config == null || Template.ConfigType == null || Config.GetType() != Template.ConfigType)
            {
                if (Config != null && Config is IDisposable d)
                    d.Dispose();

                //preferencesToolStripMenuItem_Click(sender, e);
                Config = null;
            }

            EnableDisableConfig();
            EnableDisableDebugger();
        }

        #region Debugger
        void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Error(null);
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
                    Error(null);
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
                    Error(null);
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
                    Error(null);
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

                Debugger = Template.CreateDebugger(Result, Config);

                Debugger.OnStateChanged += Debugger_OnStateChanged;
                Debugger.OnInstructionChanged += Debugger_OnInstructionChanged;
                Debugger.Stack.OnChange += Stack_OnChange;
                Debugger.AltStack.OnChange += StackAlt_OnChange;

                EnableDisableDebugger();
                UpdateDebugState();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        #endregion

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
            UpdateDebugState();
        }

        void Debugger_OnStateChanged(object sender, DebuggerState oldState, DebuggerState newState)
        {
            EnableDisableDebugger();
        }
        void Debugger_OnInstructionChanged(object sender, Instruction instruction)
        {
            //GridOpCode.ClearSelection();
            //GridOpCode.Rows[(int)instructionIndex].Selected = true;
            GridOpCode.CurrentCell = GridOpCode.Rows[instruction == null ? 0 : (int)instruction.Location.Index].Cells[3];
            Jumps.RefreshDynJumps(Debugger);

            GridOpCode.Invalidate();
            Registers.Refresh();

            //Application.DoEvents();
        }
        void UpdateDebugState()
        {
            int instruction = Debugger == null ? 0 : (int)Debugger.CurrentInstructionIndex;

            GridOpCode.Invalidate();
            Registers.Refresh();

            //GridOpCode.ClearSelection();
            //GridOpCode.Rows[(int)instructionIndex].Selected = true;
            GridOpCode.CurrentCell = GridOpCode.Rows[instruction].Cells[3];

            Jumps.RefreshDynJumps(Debugger);
            //Application.DoEvents();
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

            Error(null);

            if (Config != null && Config is IDisposable d)
                d.Dispose();

            try
            {
                Config = Template.CreateNewConfig();

                LoadFiles();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        void LoadFiles()
        {
            if (Config == null) return;

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

            new Task(() => { ALoadFiles(); }).Start();
        }
        void ALoadFiles()
        {
            ReverseResult rs = null;

            try
            {
                rs = new ReverseResult();

                if (Config != null && !Reverser.TryParse(Config, ref rs))
                    throw (new Exception("Error parsing the file"));

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
                Hex.ByteProvider = new DynamicByteProvider(result == null || result.Bytes == null ? new byte[] { } : result.Bytes);

                foreach (string sk in result.Ocurrences.Keys)
                {
                    TabPage t = new TabPage(sk)
                    {
                        Tag = "Ocurrence"
                    };

                    t.Controls.Add(new UCOcurrence(result.Ocurrences[sk], sk)
                    {
                        Dock = DockStyle.Fill
                    });

                    tabControl1.TabPages.Add(t);
                }

                TreeModules.BeginUpdate();
                TreeModules.Nodes.Clear();

                foreach (Module md in result.Modules)
                {
                    TreeNode tm = new TreeNode(md.Name)
                    {
                        Tag = md,
                        ImageKey = "Module",
                        SelectedImageKey = "Module"
                    };

                    foreach (Method mt in md.Methods)
                    {
                        TreeNode tmt = new TreeNode(mt.Name) { Tag = mt, ImageKey = "Method", SelectedImageKey = "Method" };

                        tmt.Nodes.Add(new TreeNode("From " + mt.Start.OffsetHex) { Tag = mt.Start, ImageKey = "OffsetFrom", SelectedImageKey = "OffsetFrom" });
                        tmt.Nodes.Add(new TreeNode("To " + mt.End.OffsetHex) { Tag = mt.End, ImageKey = "OffsetTo", SelectedImageKey = "OffsetTo" });

                        tm.Nodes.Add(tmt);
                    }

                    TreeModules.Nodes.Add(tm);
                }

                TreeModules.ExpandAll();
                TreeModules.EndUpdate();
            }
            else
            {
                TreeModules.Nodes.Clear();
                Hex.ByteProvider = new DynamicByteProvider(result == null || result.Bytes == null ? new byte[] { } : result.Bytes);
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

            if (i.Color != Color.Empty)
            {
                using (Brush br = new SolidBrush(i.Color))
                {
                    e.Graphics.FillRectangle(br, e.RowBounds);
                }
            }

            if (Debugger != null && Debugger.CurrentInstructionIndex == i.Location.Index)
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

            switch (i.BorderStyle)
            {
                case RowBorderStyle.All:
                    {
                        e.Graphics.DrawRectangle(Pens.DimGray, e.RowBounds.X, e.RowBounds.Y, e.RowBounds.Width - 1, e.RowBounds.Height - 1);
                        break;
                    }
                case RowBorderStyle.OnlyLeftAndRight:
                    {
                        e.Graphics.DrawLine(Pens.DimGray, e.RowBounds.X, e.RowBounds.Y, e.RowBounds.X, e.RowBounds.Bottom);
                        e.Graphics.DrawLine(Pens.DimGray, e.RowBounds.Right - 1, e.RowBounds.Y, e.RowBounds.Right - 1, e.RowBounds.Bottom);
                        break;
                    }
                case RowBorderStyle.EmptyBottom:
                    {
                        e.Graphics.DrawLine(Pens.DimGray, e.RowBounds.X, e.RowBounds.Y, e.RowBounds.Right - 1, e.RowBounds.Y);
                        goto case RowBorderStyle.OnlyLeftAndRight;
                    }
                case RowBorderStyle.EmptyTop:
                    {
                        e.Graphics.DrawLine(Pens.DimGray, e.RowBounds.X, e.RowBounds.Bottom, e.RowBounds.Right - 1, e.RowBounds.Bottom);
                        goto case RowBorderStyle.OnlyLeftAndRight;
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
            Error(null);
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
                // With Boom fail Json
                File.WriteAllText(LastSaveFile, JsonHelper.Serialize(Result, true, false), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                LastSaveFile = null;
                Error(ex);
            }
        }

        void TreeModules_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;

            if (e.Node.Tag is IStartEnd mt)
            {
                if (mt is Method) return;

                GridOpCode.CurrentCell = GridOpCode.Rows[(int)mt.Start.Index].Cells.Cast<DataGridViewCell>().LastOrDefault();
                GridOpCode.FirstDisplayedCell = GridOpCode.CurrentCell;
                tabControl1.SelectedIndex = 0;
            }
            else if (e.Node.Tag is IndexOffset io)
            {
                GridOpCode.CurrentCell = GridOpCode.Rows[(int)io.Index].Cells.Cast<DataGridViewCell>().LastOrDefault();
                GridOpCode.FirstDisplayedCell = GridOpCode.CurrentCell;
                tabControl1.SelectedIndex = 0;
            }
        }
        void TreeModules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Module md)
            {
                Hex.Select(md.Start.Offset, md.Size);
            }
            else if (e.Node.Tag is Method mt)
            {
                Hex.Select(mt.Start.Offset, mt.Size);
            }
            else if (e.Node.Tag is IndexOffset io)
            {
                Hex.Select(io.Offset, 1);
            }
        }
    }
}