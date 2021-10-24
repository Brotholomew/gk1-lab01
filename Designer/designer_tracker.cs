using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lab01
{
    public static partial class designer
    {
        private static drawable Tracker(Point p, Dictionary<Point, List<drawable>> map)
        {
            int trackerRadius = printer.VertexRadius;

            // recognize objects that are in a square of size trackerRadius x trackerRadius
            // (mouse pointer is at the square's center), begin tracking from the nearest
            // pixels to the mouse pointer

            for (int k = 0; k <= trackerRadius; k++)
            {
                for (int i = -k; i <= k; i++)
                    for (int j = -k; j <= k; j++)
                    {
                        if (i == -k || i == k || j == -k || j == k)
                        {
                            Point np = new Point(p.X + i, p.Y + j);
                            if (map.ContainsKey(np) && map[np].Count > 0)
                                return map[np][0];
                        }
                        else
                            continue;
                    }
            }

            return null;
        }

        public static vertex TrackVertices(Point p) => (vertex) designer.Tracker(p, designer._Canvas.VertexMap);

        public static drawable TrackDrawable(Point p) => designer.Tracker(p, designer._Canvas.DrawableMap);

        public static drawable TrackFigure(Point p)
        {
            Dictionary<Point, List<drawable>> map = designer._Canvas.DrawableMap;

            // search for lines from the mouse pointer location to the furthest allocated point
            // to the right, if a line is found, determine whether the point is inside this line's poly

            for (int i = p.X; i <= designer.Canvas.FurthestX; i++)
            {
                Point np = new Point(i, p.Y);
                if (map.ContainsKey(np) && map[np].Count > 0)
                {
                    drawable d = map[np][0];

                    if (d is circle)
                        if (Functors.RealDistance(((circle)d).Vertices[0].Pixels[0], p) < ((circle)d).Radius)
                            return d;

                    if (d is line)
                    {
                        poly po = ((line)d).Poly;

                        GraphicsPath gp = new GraphicsPath();
                        gp.AddPolygon(po.Vertices.ConvertAll((vertex v) => v.Pixels[0]).ToArray());
                        
                        if (gp.IsVisible(p))
                            return po;
                    }
                }
            }

            return null;
        }

        public static drawable Track(Point p, Brush brush)
        {
            drawable d = null;

            if ((d = designer.TrackVertices(p)) != null)
            {
                printer.PutVertex(d.Pixels[0], brush);
            } 
            else if ((d = designer.TrackDrawable(p)) != null)
            {
                printer.PutPixels(d.Pixels, brush);
            }
            else if ((d = designer.TrackFigure(p)) != null)
            {
                printer.PutFigure(d, brush);

                if (d is circle)
                    printer.PutVertex(((circle)d).Vertices[0].Pixels[0], ((circle)d).Vertices[0].Brush);
            } 

            printer.FlushBuffer();
            return d;
        }
    }
}
