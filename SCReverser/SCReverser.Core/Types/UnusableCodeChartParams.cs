using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace SCReverser.Core.Types
{
    public class UnusableCodeChartParams : OcurrenceParams
    {
        /// <summary>
        /// Result
        /// </summary>
        public ReverseResult Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result</param>
        public UnusableCodeChartParams(ReverseResult result)
        {
            Result = result;
        }
        /// <summary>
        /// On chart event
        /// </summary>
        /// <param name="oc">Ocurrences</param>
        /// <param name="chart">Chart</param>
        public override void OnChart(IEnumerable<Ocurrence> oc, Chart chart)
        {
            Series sPie = chart.Series[1];
            sPie.Points.Clear();

            // Calculate size

            long total = Result != null && Result.Bytes != null ? Result.Bytes.Length : 0;
            if (total <= 0) return;

            long size = 0;

            foreach (Ocurrence o in oc)
                foreach (Instruction i in o.Instructions)
                    size += i.Size;

            sPie.XValueMember = "Type";
            sPie.YValueMembers = "%";

            sPie.Points.AddXY((((total - size) * 100.0) / total).ToString("0.00 '%'"), total);
            sPie.Points.AddXY("Unusable " + (((size) * 100.0) / total).ToString("0.00 '%'"), size);
        }
    }
}