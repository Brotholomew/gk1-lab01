using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace lab01
{
    public enum PrintingStates { StartPoly, FollowMouse, Off };

    public static class Embellisher
    {
        // cursors
        public static readonly Cursor NormalCursor = Cursors.Default;
        public static readonly Cursor PolyDrawCursor = Cursors.Cross;
        public static readonly Cursor CircleDrawCursor = Cursors.Cross;

        // colors
        public static readonly Brush DrawColor = Brushes.Black;
        public static readonly Brush TrackColor = new SolidBrush(Color.FromArgb(150, Color.Magenta));
        public static readonly Brush SelectedColor = new SolidBrush(Color.FromArgb(150, Color.DarkOrange));
        public static readonly Brush VertexBrush = Brushes.Blue;
        public static readonly Brush FirstVertexBrush = Brushes.Red;
        public static readonly Brush FixedLengthHighlightBrush = Brushes.LightCoral;
        public static readonly Brush FixedCenterBrush = Brushes.Red;
        public static readonly Brush EqualLengthBrush = Brushes.LawnGreen;
        public static readonly Brush AdjacentBrush = Brushes.CornflowerBlue;
        public static readonly Brush ParallelBrush = Brushes.Red;
        public static readonly Brush StringBrush = Brushes.Red;

        public static Cursor SwitchCursor(Cursor current, Cursor switched = null)
        {
            if (current == Embellisher.NormalCursor)
                return switched;
            else
                return Embellisher.NormalCursor;
        }
    }
}
