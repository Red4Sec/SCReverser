using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SCReverser.Controls
{
    public class UCPJump : Control
    {
        const int MaxDrawJump = 5;
        const int ArrowWidth = 5;

        /// <summary>
        /// Grid
        /// </summary>
        public DataGridView Grid
        {
            get { return _Grid; }
            set
            {
                if (_Grid != null)
                {
                    _Grid.DataSourceChanged -= _Grid_DataSourceChanged;
                }

                _Grid = value;

                if (_Grid != null)
                {
                    _Grid.DataSourceChanged += _Grid_DataSourceChanged;
                }
            }
        }

        class PaintState
        {
            public uint IndexFrom, IndexTo;
            public DashStyle Style;
            public Rectangle RectFrom, RectTo;

            public int Distance { get { return (int)Math.Abs((long)IndexFrom - (long)IndexTo); } }
        }

        DataGridView _Grid;
        Instruction[] Jumps;
        Instruction[] DynJumps;
        uint CurrentInstructionIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        public UCPJump()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
        }

        void _Grid_DataSourceChanged(object sender, EventArgs e)
        {
            if (_Grid.DataSource == null)
            {
                Jumps = null;
                return;
            }

            Jumps = _Grid.Rows.Cast<DataGridViewRow>().Select(u => u.DataBoundItem as Instruction)
                .Where(u => u.Jump != null)
                .ToArray();

            DynJumps = Jumps.Where(u => u.Jump.IsDynamic).ToArray();
        }
        /// <summary>
        /// Refresh dynamic jumps
        /// </summary>
        /// <param name="debug">Debugger</param>
        public void RefreshDynJumps(IDebugger debug)
        {
            CurrentInstructionIndex = debug == null ? uint.MaxValue : debug.CurrentInstructionIndex;

            foreach (Instruction i in DynJumps)
                i.Jump.Check(debug, i);
        }
        /// <summary>
        /// Paint method
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Jumps == null) return;

            uint indexFrom = uint.MaxValue;
            uint indexTo = uint.MaxValue;

            indexFrom = (uint)_Grid.FirstDisplayedCell.RowIndex;
            indexTo = indexFrom + (uint)_Grid.DisplayedRowCount(true);

            List<PaintState> ls = new List<PaintState>();
            foreach (Instruction i in Jumps)
            {
                if (!i.Jump.Offset.HasValue) continue;
                if (!i.Jump.Index.HasValue) continue;
                if (i.Index < indexFrom && i.Index > indexTo) continue;

                PaintState p = new PaintState()
                {
                    IndexFrom = i.Index,
                    IndexTo = i.Jump.Index.Value,
                    Style = i.Jump.Style,
                    RectFrom = _Grid.GetRowDisplayRectangle((int)i.Index, false),
                    RectTo = _Grid.GetRowDisplayRectangle((int)i.Jump.Index.Value, false)
                };

                if (p.RectFrom.IsEmpty && p.RectTo.IsEmpty)
                    continue;

                ls.Add(p);

                if (ls.Count == MaxDrawJump)
                    break;
            }

            if (ls.Count <= 0) return;

            int x = 5;
            int step = Math.Max((Width - x - ArrowWidth) / ls.Count, 1);
            foreach (PaintState p in ls.OrderBy(u => u.Distance))
            {
                PainJump(e.Graphics, p, x);
                x += step;
            }
        }
        void DrawArrow(Graphics gp, Brush brush, int x, int y, bool isOut)
        {
            if (isOut)
                gp.FillPolygon(brush, new PointF[]
                            {
                            new PointF(x,y-ArrowWidth),
                            new PointF(x,y+ArrowWidth),
                            new PointF(x-ArrowWidth,y),
                            });
            else
                gp.FillPolygon(brush, new PointF[]
                            {
                            new PointF(x-ArrowWidth,y-ArrowWidth),
                            new PointF(x-ArrowWidth,y+ArrowWidth),
                            new PointF(x,y),
                            });
        }
        /// <summary>
        /// Paint jump
        /// </summary>
        /// <param name="gp">Graphics</param>
        /// <param name="pt">Paint</param>
        /// <param name="s">Separation</param>
        void PainJump(Graphics gp, PaintState pt, int x)
        {
            Rectangle rFrom = pt.RectFrom;
            Rectangle rTo = pt.RectTo;

            if (rTo.IsEmpty && rFrom.IsEmpty)
                return;

            bool current = CurrentInstructionIndex == pt.IndexFrom;

            using (Pen p = new Pen(current ? Color.Green : Color.Black, current ? 2F : 1F))
            {
                p.DashStyle = pt.Style;

                if (!rTo.IsEmpty)
                {
                    gp.DrawLine(p, x, rTo.Y + (rTo.Height / 2), Width, rTo.Y + (rTo.Height / 2));
                    DrawArrow(gp, current ? Brushes.Green : Brushes.Black, Width, rTo.Y + (rTo.Height / 2), false);

                    if (rFrom.IsEmpty)
                    {
                        // Not To
                        if (pt.IndexFrom < pt.IndexTo)
                        {
                            // To up
                            gp.DrawLine(p, x, rTo.Y + (rTo.Height / 2), x, 0);
                        }
                        else
                        {
                            // To down
                            gp.DrawLine(p, x, rTo.Y + (rTo.Height / 2), x, Height);
                        }
                    }
                }

                if (!rFrom.IsEmpty)
                {
                    gp.DrawLine(p, x, rFrom.Y + (rFrom.Height / 2), Width, rFrom.Y + (rFrom.Height / 2));
                    DrawArrow(gp, current ? Brushes.Green : Brushes.Black, Width, rFrom.Y + (rFrom.Height / 2), true);

                    if (rTo.IsEmpty)
                    {
                        // Not To
                        if (pt.IndexFrom > pt.IndexTo)
                        {
                            // To up
                            gp.DrawLine(p, x, rFrom.Y + (rFrom.Height / 2), x, 0);
                        }
                        else
                        {
                            // To down
                            gp.DrawLine(p, x, rFrom.Y + (rFrom.Height / 2), x, Height);
                        }
                    }
                    else
                    {
                        // From and to
                        gp.DrawLine(p, x, rFrom.Y + (rFrom.Height / 2), x, rTo.Y + (rTo.Height / 2));
                    }
                }
            }
        }
    }
}