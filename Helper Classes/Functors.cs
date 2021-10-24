﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace lab01
{
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
        public static Point Midpoint(Point p1, Point p2) => new Point((int)Math.Ceiling((p1.X + p2.X) / 2.0), (int)Math.Ceiling((p1.Y + p2.Y) / 2.0));
        public static Point MovePoint(Point p, Point distance) => new Point(p.X + distance.X, p.Y + distance.Y);
        public static void MovePoints(List<Point> l, Point distance)
        {
            for (int i = 0; i < l.Count; i++)
                l[i] = Functors.MovePoint(l[i], distance);
        }
        public static Point Distance(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);
        public static double RealDistance(Point start, Point end) => Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));
        public static LineVariables GetLineVariables(line l)
        {
            Point start = l.Start;
            Point end = l.End;

            double a = double.PositiveInfinity;
            if (Math.Abs(l.Start.X - l.End.X) > 5)
                a = (double)(end.Y - start.Y) / (double)(end.X - start.X);

            double b = start.Y - a * start.X;

            return new LineVariables(a, b);
        }
    }
}
