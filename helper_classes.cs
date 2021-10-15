using System;
using System.Collections.Generic;
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
