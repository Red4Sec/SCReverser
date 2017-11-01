using SCReverser.Core.Collections;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SCReverser.Controls
{
    public partial class UCOcurrence : UserControl
    {
        OcurrenceCollection Original;
        DataTable Source = new DataTable();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ocurrences">Ocurrences</param>
        /// <param name="name">Name</param>
        public UCOcurrence(OcurrenceCollection ocurrences, string name)
        {
            InitializeComponent();

            Grid.AutoGenerateColumns = false;

            Text = name;
            Original = ocurrences;
            Source = new DataTable();

            Source.Columns.Add("Count", typeof(uint));
            Source.Columns.Add("Value", typeof(string));
            Source.Columns.Add("Tag", typeof(Ocurrence));
            Grid.DataSource = Source;

            Fill(ocurrences);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            //base.OnGotFocus(e);
            txtSearch.Focus();
        }
        /// <summary>
        /// Fill with ocurrences
        /// </summary>
        /// <param name="ocurrences">Ocurrences</param>
        /// <param name="name">Name</param>
        public void Fill(OcurrenceCollection ocurrences)
        {
            Source.BeginLoadData();
            Source.Rows.Clear();

            foreach (Ocurrence o in ocurrences.GetOrdered())
                Source.Rows.Add(o.Count, o.Value, o);

            Source.EndLoadData();

            CalculateChart(ocurrences);
        }

        void CalculateChart(IEnumerable<Ocurrence> oc)
        {
            Ocurrence[] ocRes = oc.ToArray();

            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            Series sBar = new Series(Text);

            chart.ChartAreas.Add(sBar.Name);
            chart.Series.Add(sBar);
            chart.Legends.Add(sBar.Name);

            chart.Legends[sBar.Name].IsDockedInsideChartArea = true;
            chart.Legends[sBar.Name].DockedToChartArea = sBar.Name;
            sBar.ChartArea = sBar.Name;
            sBar.Legend = sBar.Name;
            sBar.ChartType = SeriesChartType.StackedBar;
            //chart.Legends[k].Docking = Docking.Bottom;
            //chart.Legends[k].Alignment = StringAlignment.Center;

            chart.ChartAreas[sBar.Name].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;

            // ----
            Series sPie = new Series(sBar.Name + " %");
            sPie.Legend = null;
            sPie.IsVisibleInLegend = false;

            chart.ChartAreas.Add(sPie.Name);
            chart.Series.Add(sPie);

            chart.ChartAreas[0].Position.Auto = false;
            chart.ChartAreas[0].Position.Height = 100;
            chart.ChartAreas[0].Position.Width = 88;
            chart.ChartAreas[1].Position.Auto = false;
            chart.ChartAreas[1].Position.X = 90;
            chart.ChartAreas[1].Position.Height = 100;
            chart.ChartAreas[1].Position.Width = 10;

            chart.Series[sPie.Name].ChartArea = sPie.Name;
            chart.Series[sPie.Name].ChartType = SeriesChartType.Pie;

            chart.ChartAreas[sPie.Name].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;

            foreach (Ocurrence o in oc.Take(5).OrderBy(u => u.Count))
            {
                sPie.Points.AddXY(o.Value, o.Count);
                sBar.Points.AddXY(o.Value, o.Count);
            }

            if (ocRes != null && Original.ControlParams != null)
                Original.ControlParams.OnChart(ocRes, chart);
        }

        void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text;

            (Source).DefaultView.RowFilter = String.IsNullOrEmpty(txtSearch.Text) ? "" : String.Format("Value LIKE '%{0}%'", search);
            CalculateChart(Original
                .Where(u => u.Value.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1));
        }
        void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = Grid.SelectedRows.Count <= 0;

            if (!e.Cancel)
            {
                selectToolStripMenuItem.Tag = null;
                selectToolStripMenuItem.DropDownItems.Clear();

                foreach (DataGridViewRow r in Grid.SelectedRows)
                {
                    if (r.DataBoundItem == null || !(r.DataBoundItem is DataRowView dr))
                        continue;

                    Ocurrence o = (Ocurrence)dr.Row["Tag"];
                    if (o == null) continue;

                    foreach (Instruction i in o.Instructions)
                    {
                        if (selectToolStripMenuItem.Tag == null)
                            selectToolStripMenuItem.Tag = i;

                        selectToolStripMenuItem.DropDownItems.Add(i.ToString(), null, selectToolStripMenuItem_Click).Tag = i;
                    }

                    if (selectToolStripMenuItem.DropDownItems.Count == 1)
                    {
                        selectToolStripMenuItem.DropDownItems[0].Visible = false;
                    }
                }
            }
        }
        void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem i = (ToolStripItem)sender;

            if (i.Tag != null && i.Tag is Instruction ins)
            {
                Form f = FindForm();
                if (!(f is FMain fm)) return;

                fm.SelectInstruction(ins.Location);
            }
        }
        void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hti = Grid.HitTest(e.X, e.Y);
                Grid.ClearSelection();
                if (hti.RowIndex < 0) return;

                Grid.Rows[hti.RowIndex].Selected = true;
            }
        }
    }
}