using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public static partial class designer
    {
        private static readonly Brush VertexBrush = Brushes.Blue;
        private static readonly Brush FirstVertexBrush = Brushes.Red;

        public static void DrawPoly(MouseEventArgs e)
        {
            designer.LastLine = null;
            
            // rysowanie linii, łamanych oraz wielokątów
            if (designer.State == PrintingStates.Off)
            {
                // pierwszy punkt
                vertex v = designer.DrawVertex(e.Location, designer.FirstVertexBrush);
                designer._Vertices.Add(v);

                designer.LastPoint = new Point(e.Location.X, e.Location.Y);
                designer.FirstPoint = new Point(e.Location.X, e.Location.Y);

                designer.State = PrintingStates.FollowMouse;
            }
            else
            {
                // ostatni punkt
                if (designer._Lines.Count > 0 && 
                    designer.Distance(designer.LastPoint, designer.FirstPoint) <= printer.VertexRadius)
                {
                    designer.Canvas.ErasePreview();
                    designer.State = PrintingStates.Off;

                    line l = designer.DrawLine(designer.LastPoint, designer.FirstPoint, Brushes.Black);
                    designer.Canvas.DeregisterDrawables(designer._Lines);

                    designer._Lines.Add(l);
                    designer._Vertices[0].Brush = designer.VertexBrush;

                    poly p = new poly(designer._Lines, designer._Vertices);

                    designer._Lines = new List<drawable>();
                    designer._Vertices = new List<vertex>();
                    designer.LastPoint = designer.NullPoint;
                    designer.FirstPoint = designer.NullPoint;

                    designer.Canvas.RegisterDrawable(p);
                    Canvas.Reprint();
                }
                else
                {
                    // kolejny punkt
                    vertex v = designer.DrawVertex(e.Location, designer.VertexBrush);
                    designer._Vertices.Add(v);

                    line l = designer.DrawLine(designer.LastPoint, e.Location, Brushes.Black);
                    designer.Canvas.RegisterDrawable(l);
                    designer._Lines.Add(l);
                }

                designer.LastPoint = e.Location;
            }
        }
    }
}
