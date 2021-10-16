using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public abstract class drawable
    {
        protected List<Point> _Pixels;
        protected List<vertex> _Vertices;

        public virtual List<Point> Pixels { get => this._Pixels; }
        public virtual List<vertex> Vertices { get => this._Vertices; }
        public virtual Brush Brush { get; set; }
        public virtual bool IsDummy => false;

        public abstract void Delete();

        public drawable(List<Point> _pixels, List<vertex> _vertices, Brush _brush)  
        {
            this._Pixels = _pixels;
            this._Vertices = _vertices;
            this.Brush = _brush;
        }
    }

    public class vertex : drawable
    {
        private List<drawable> _Drawables = new List<drawable>();
        private Point p;
        public vertex(Point _p, List<Point> _pixels, Brush _brush) : base(_pixels, new List<vertex>(), _brush)
        {
            this.p = new Point(_p.X, _p.Y);
            this._Vertices.Add(this);
        }
        public Point Center { get => this.p; }
        public List<drawable> AdjacentLines { get => this._Drawables; }
        public void AddLine(line l) => AdjacentLines.Add(l);
        public void Clear() => this._Drawables.Clear();

        public override void Delete()
        {
            if (this._Drawables.Count == 2)
            {
                poly poly = ((line)this._Drawables[0]).Poly;
                if (poly.Vertices.Count <= 3)
                {
                    poly.Delete();
                    return;
                }

                poly.Vertices.Remove(this);
                List<vertex> temp = new List<vertex>();

                foreach (var line in this._Drawables.ConvertAll((drawable d) => (line)d))
                {
                    vertex vx = line.GetNext(this);
                    
                    vx.AdjacentLines.Remove(line);
                    line.Clear();
                    line.Delete();

                    temp.Add(vx);
                }

                line l = null;
                designer.Canvas.PrintToMain(() => l = designer.DrawLine(temp[0].Center, temp[1].Center, embellisher.DrawColor));
                designer.Canvas.RegisterDrawable(l);

                foreach (var vx in temp)
                {
                    vx.AddLine(l);
                    l.AddVertex(vx);
                }

                poly.Lines.Add(l);
                l.Poly = poly;
            }
            else if (this._Drawables.Count == 1)
            {
                this._Drawables[0].Delete();
                return;
            }

            designer.Canvas.EraseVertex(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }

        public line GetNext(line l)
        {
            foreach (var line in this._Drawables)
                if (line != l) return (line)line;

            return null;
        }

        public static vertex Merge(vertex v1, vertex v2)
        {
            if (v1._Drawables.Count > 1 || v2._Drawables.Count > 1)
                return null;

            Point p1 = v1.Pixels[0];
            Point p2 = v2.Pixels[0];

            Point midpoint = new Point((int) Math.Ceiling((p1.X + p2.X) / 2.0), (int) Math.Ceiling((p1.Y + p2.Y) / 2.0));
            vertex v = designer.DrawVertex(midpoint, embellisher.VertexBrush);

            foreach (var vx in new List<vertex>{ v1, v2 }) {
                line ln = (line)vx._Drawables[0];
                vertex n = ln.GetNext(vx);
                
                line nl = null;
                designer.Canvas.PrintToMain(() => nl = designer.DrawLine(n.Pixels[0], midpoint, embellisher.DrawColor));
                designer.Canvas.RegisterDrawable(nl);
                nl.Poly = ln.Poly;
                nl.Vertices.Add(n);
                nl.Vertices.Add(v);
                v.AddLine(nl);
                n.AddLine(nl);
                n.AdjacentLines.Remove(ln);

                ln.Poly.Lines.Remove(ln);
                ln.Poly.Lines.Add(nl);
                ln.Poly.Vertices.Remove(vx);
                designer.Canvas.EraseVertex(vx);

                ln.Clear();
                ln.Delete();
            }

            return v;
        }
    }

    public class line : drawable
    {
        private Point _Start;
        private Point _End;

        public Point Start { get => this._Start; }
        public Point End { get => this._End; }

        public poly Poly;
        public line(Point _start, Point _end, List<Point> _pixels, Brush _brush) : base(_pixels, new List<vertex>(), _brush)
        {
            this._Start = _start;
            this._End = _end;
        }

        public void AddVertex(vertex v) => this._Vertices.Add(v);

        public vertex GetNext(vertex v)
        {
            foreach (var vertex in this.Vertices)
                if (vertex != v) return vertex;

            return null;
        }

        public void Clear() => this._Vertices.Clear();

        public override void Delete()
        {
            poly p = this.Poly;

            if (this.Vertices.Count == 2)
            {
                if (this.Poly.Lines.Count <= 3)
                {
                    this.Poly.Delete();
                    return;
                }

                foreach (var v in this.Vertices)
                    v.AdjacentLines.Remove(this);

                vertex nv = vertex.Merge(this.Vertices[0], this.Vertices[1]);
                p.Vertices.Add(nv);
            }

            if (p != null)
            {
                p.Lines.Remove(this);
            }

            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }
    }

    public class circle : drawable
    {
        private int radius;

        public int Radius { get => this.radius; }
        public circle(vertex _center, int _radius, List<Point> _pixels, Brush _brush) : base(_pixels, new List<vertex>{ _center }, _brush)
        {
            this.radius = _radius;
        }

        public override void Delete()
        {
            designer.Canvas.EraseVertices(this.Vertices);
            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }
    }

    public class poly : drawable
    {
        private List<drawable> _Lines;

        public List<drawable> Lines { get => this._Lines; }

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

        public poly(List<drawable> _lines, List<vertex> _vertices) : base(null, _vertices, Brushes.Black)
        {
            this._Lines = _lines;
        }

        public override void Delete()
        {
            designer.Canvas.EraseVertices(this.Vertices);

            foreach (var line in this._Lines)
                designer.Canvas.EraseDrawable(line);

            designer.Canvas.Reprint();
            printer.Erase();
        }
    }
}
