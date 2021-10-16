using System;
using System.Collections.Generic;
using System.Text;

namespace lab01
{
    public static partial class designer
    {
        public static void DeleteDrawable(drawable d)
        {
            designer._Canvas.EraseDrawable(d);

            foreach (var v in d.Vertices)
                designer._Canvas.EraseVertex(v);

            designer._Canvas.Reprint();
            printer.Erase();
        }
    }
}
