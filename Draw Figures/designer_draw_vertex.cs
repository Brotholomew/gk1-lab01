using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public static partial class designer
    {
        public static vertex DrawVertex(Point pos, Brush brush)
        {
            vertex v = new vertex(pos, new List<Point> { pos }, brush);
            designer._Canvas.PrintVertex(v);
            designer._Canvas.RegisterVertex(v);
            return v;
        }
    }
}
