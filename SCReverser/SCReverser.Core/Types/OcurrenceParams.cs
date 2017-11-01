using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace SCReverser.Core.Types
{
    public class OcurrenceParams
    {
        /// <summary>
        /// On chart event
        /// </summary>
        /// <param name="oc">Ocurrences</param>
        /// <param name="chart">Chart</param>
        public virtual void OnChart(IEnumerable<Ocurrence> oc, Chart chart)
        {

        }
    }
}