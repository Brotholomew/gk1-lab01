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
            printer.PutVertex(pos, brush);
            vertex v = new vertex(pos, new List<Point> { pos }, brush);
            designer.Canvas.RegisterVertex(v);
            return v;
        }
    }
}
