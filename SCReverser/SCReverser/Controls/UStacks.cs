using System;
using System.Windows.Forms;
using SCReverser.Core.Types;

namespace SCReverser.Controls
{
    public partial class UStacks : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UStacks()
        {
            InitializeComponent();

            GridStack.AutoGenerateColumns = false;
            GridAltStack.AutoGenerateColumns = false;
        }
        /// <summary>
        /// Refresh grids
        /// </summary>
        public void RefreshGrids()
        {
            GridAltStack.Refresh();
            GridStack.Refresh();
        }
        /// <summary>
        /// Set stack items
        /// </summary>
        /// <param name="items">Items</param>
        public void SetStackSource(StackItem[] items)
        {
            GridStack.DataSource = items;
            GridStack.ClearSelection();
        }
        /// <summary>
        /// Set alt items
        /// </summary>
        /// <param name="items">Items</param>
        public void SetAltStackSource(StackItem[] items)
        {
            GridAltStack.DataSource = items;
            GridAltStack.ClearSelection();
        }
    }
}