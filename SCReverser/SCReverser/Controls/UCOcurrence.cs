using SCReverser.Core.Collections;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
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
                Source.Rows.Add(o.Count, o.Value);

            Source.EndLoadData();

            CalculateChart(ocurrences);
        }

        void CalculateChart(IEnumerable<Ocurrence> oc)
        {
            string name = Text;

            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            chart.ChartAreas.Add(name);
            chart.Series.Add(name);
            chart.Legends.Add(name);

            chart.Legends[name].IsDockedInsideChartArea = true;
            chart.Legends[name].DockedToChartArea = name;
            chart.Series[name].ChartArea = name;
            chart.Series[name].Legend = name;
            chart.Series[name].ChartType = SeriesChartType.StackedBar;
            //chart.Legends[k].Docking = Docking.Bottom;
            //chart.Legends[k].Alignment = StringAlignment.Center;

            chart.ChartAreas[name].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;

            foreach (Ocurrence o in oc.Take(5).OrderBy(u => u.Count))
                chart.Series[name].Points.AddXY(o.Value, o.Count);
        }

        void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text;

            (Source).DefaultView.RowFilter = String.IsNullOrEmpty(txtSearch.Text) ? "" : String.Format("Value LIKE '%{0}%'", search);
            CalculateChart(Original
                .Where(u => u.Value.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1));
        }
    }
}