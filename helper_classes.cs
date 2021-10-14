using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    enum PrintingStates { StartPoly, FollowMouse, Off };

    struct CanvasPixel 
    {
        public List<drawable> Drawables;
        public drawable V;
    }

    public static class embellisher
    {
        // cursors
        private static Cursor NormalCursor = Cursors.Default;
        
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
