using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public abstract class drawable
    {
        protected List<Point> _Pixels;

        public virtual List<Point> Pixels { get => this._Pixels; }
        public virtual Brush Brush { get; set; }
        public virtual bool IsDummy => false;

        public drawable(List<Point> _pixels, Brush _brush)  
        {
            this._Pixels = _pixels;
            this.Brush = _brush;
        }
    }

    public class vertex : drawable
    {
        private List<drawable> _Drawables = new List<drawable>();
        private Point p;
        public vertex(Point _p, List<Point> _pixels, Brush _brush) : base(_pixels, _brush)
        {
            this.p = new Point(_p.X, _p.Y);
        }
        public Point Center { get => this.p; }
        public List<drawable> AdjacentLines { get => this._Drawables; }
        public void AddLine(line l) => AdjacentLines.Add(l);
        public void Clear() => this._Drawables.Clear();
    }

    public class line : drawable
    {
        private Point _Start;
        private Point _End;
        public line(Point _start, Point _end, List<Point> _pixels, Brush _brush) : base(_pixels, _brush)
        {
            this._Start = _start;
            this._End = _end;
        }
    }

    public class circle : drawable
    {
        private Point center;
        private int radius;
        public circle(Point _center, int _radius, List<Point> _pixels, Brush _brush) : base(_pixels, _brush)
        {
            this.center = _center;
            this.radius = _radius;
        }
    }

    public class poly : drawable
    {
        private List<drawable> _Lines;
        private List<vertex> _Vertices;
        public override List<Point> Pixels
        {
            get
            {
                List<Point> ret = new List<Point>();
                foreach (var line in this._Lines)
                    ret.AddRange(line.Pixels);

                return ret;
            }
        }

        public poly(List<drawable> _lines, List<vertex> _vertices) : base(null, Brushes.Black)
        {
            this._Vertices = _vertices;
            this._Lines = _lines;
        }
    }
}
