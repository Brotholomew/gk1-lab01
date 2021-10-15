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

        public static void DrawPoly(MouseEventArgs e, mainForm f)
        {
            // rysowanie linii, łamanych oraz wielokątów
            if (designer._State == PrintingStates.Off)
            {
                // pierwszy punkt
                vertex v = designer.DrawVertex(e.Location, designer.FirstVertexBrush);
                designer._Vertices.Add(v);

                designer.LastPoint = new Point(e.Location.X, e.Location.Y);
                designer.FirstPoint = new Point(e.Location.X, e.Location.Y);

                designer._State = PrintingStates.FollowMouse;
            }
            else
            {
                // ostatni punkt
                if (designer._Lines.Count > 0 && 
                    designer.Distance(e.Location, designer.FirstPoint) <= printer.VertexRadius)
                {
                    designer._State = PrintingStates.Off;

                    line l = designer.DrawLine(designer.LastPoint, designer.FirstPoint, Brushes.Black);
                    
                    designer._Lines.Add(l);
                    designer._Canvas.DeregisterDrawables(designer._Lines);
                    designer._Vertices[0].Brush = designer.VertexBrush;
                    designer._Canvas.PrintVertex(designer._Vertices[0]);

                    poly p = new poly(designer._Lines, designer._Vertices);

                    designer._Lines = new List<drawable>();
                    designer._Vertices = new List<vertex>();
                    designer.LastPoint = designer.NullPoint;
                    designer.FirstPoint = designer.NullPoint;

                    designer._Canvas.RegisterDrawable(p);
                    designer._Canvas.PrintDrawable(p);
                    designer._Canvas.ErasePreview();

                    f.DM = DesignModes.Off;
                }
                else
                {
                    // kolejny punkt
                    vertex v = designer.DrawVertex(e.Location, designer.VertexBrush);
                    designer._Vertices.Add(v);
                    line l = null;
                    designer.Canvas.PrintToMain(() => l = designer.DrawLine(designer.LastPoint, e.Location, Brushes.Black));
                    designer._Canvas.RegisterDrawable(l);
                    designer._Lines.Add(l);
                }

                designer.LastPoint = e.Location;
            }

            printer.Erase();
        }
    }
}
