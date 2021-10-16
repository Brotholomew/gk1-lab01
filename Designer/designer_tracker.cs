﻿using System;
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

        public static vertex TrackVertices(Point p)
        {
            return (vertex) designer.Tracker(p, designer._Canvas.VertexMap);
        }

        public static drawable TrackDrawable(Point p)
        {
            return designer.Tracker(p, designer._Canvas.DrawableMap);
        }

        public static drawable TrackFigure(Point p)
        {
            Dictionary<Point, List<drawable>> map = designer._Canvas.DrawableMap;

            for (int i = p.X; i <= designer.Canvas.FurthestX; i++)
            {
                Point np = new Point(i, p.Y);
                if (map.ContainsKey(np) && map[np].Count > 0)
                {
                    drawable d = map[np][0];

                    if (d is circle)
                        if (designer.Distance(((circle)d).Vertices[0].Pixels[0], p) < ((circle)d).Radius)
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
            drawable d = designer.TrackVertices(p);

            if (d != null)
            {
                printer.PutVertex(d.Pixels[0], brush);
                return d;
            }

            d = designer.TrackDrawable(p);

            if (d != null)
            {
                printer.PutPixels(d.Pixels, brush);
                return d;
            }

            d = designer.TrackFigure(p);

            if (d != null)
            {
                printer.PutFigure(d, brush);

                if (d is circle)
                    printer.PutVertex(((circle)d).Vertices[0].Pixels[0], ((circle)d).Vertices[0].Brush);

                return d;
            }

            printer.FlushBuffer();
            return null;
        }
    }
}
