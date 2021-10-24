using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public static partial class designer
    {
        public static void DrawPoly(MouseEventArgs e, mainForm f)
        {
            if (designer._State == PrintingStates.Off)
            {
                // first point
                vertex v = designer.DrawVertex(e.Location, Embellisher.FirstVertexBrush);
                designer._Vertices.Add(v);

                designer.LastPoint = new Point(e.Location.X, e.Location.Y);
                designer.FirstPoint = new Point(e.Location.X, e.Location.Y);

                designer._State = PrintingStates.FollowMouse;
            }
            else
            {
                // next point
                if (designer._Lines.Count > 1 &&
                    Functors.RealDistance(e.Location, designer.FirstPoint) <= printer.VertexRadius)
                {
                    // last point
                    designer._State = PrintingStates.Off;

                    line l = null;
                    designer.Canvas.PrintToMain(() => l = designer.DrawLine(designer.LastPoint, e.Location, Brushes.Black));
                    designer._Canvas.RegisterDrawable(l);
                    l.AddVertex(designer._Vertices[designer._Vertices.Count - 1]);
                    l.AddVertex(designer._Vertices[0]);

                    designer._Lines.Add(l);
                    designer._Vertices[0].Brush = Embellisher.VertexBrush;
                    designer._Vertices[designer._Vertices.Count - 1].AddLine(l);
                    designer._Vertices[0].AddLine(l);
                    designer._Canvas.PrintVertex(designer._Vertices[0]);

                    poly p = new poly(designer._Lines, designer._Vertices);
                    foreach (var _line in designer._Lines)
                        ((line)_line).Poly = p;

                    designer._Lines = new List<drawable>();
                    designer._Vertices = new List<vertex>();
                    designer.LastPoint = designer.NullPoint;
                    designer.FirstPoint = designer.NullPoint;

                    designer._Canvas.ErasePreview();

                    f.DM = DesignModes.Off;
                }
                else if (designer._Lines.Count <= 1 &&
                         Functors.RealDistance(e.Location, designer.FirstPoint) <= printer.VertexRadius)
                {
                    // do nothing when there are not enough lines to create a poly
                    printer.Erase();
                    return;
                }
                else
                {
                    // another point
                    vertex v = designer.DrawVertex(e.Location, Embellisher.VertexBrush);
                    designer._Vertices.Add(v);
                    line l = null;
                    designer.Canvas.PrintToMain(() => l = designer.DrawLine(designer.LastPoint, e.Location, Brushes.Black));
                    designer._Canvas.RegisterDrawable(l);
                    designer._Lines.Add(l);
                    designer._Vertices[designer._Vertices.Count - 1].AddLine(l);
                    designer._Vertices[designer._Vertices.Count - 2].AddLine(l);

                    l.AddVertex(designer._Vertices[designer._Vertices.Count - 1]);
                    l.AddVertex(designer._Vertices[designer._Vertices.Count - 2]);
                }

                designer.LastPoint = e.Location;
            }

            printer.Erase();
        }
    }
}
