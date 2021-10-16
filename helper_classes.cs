using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public enum PrintingStates { StartPoly, FollowMouse, Off };

    public static class embellisher
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

        public static Cursor SwitchCursor(Cursor current, Cursor switched = null) {
            if (current == embellisher.NormalCursor)
                return switched;
            else
                return embellisher.NormalCursor;
        }
    }

    public static class Functors
    {
        public delegate void refAction<T>(ref T arg);
        public static void Swap<T>(ref T obj1, ref T obj2)
        {
            T buf;
            buf = obj1;
            obj1 = obj2;
            obj2 = buf;
        }
    }
}
