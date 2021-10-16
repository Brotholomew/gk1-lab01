using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace lab01
{
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

        public override void DeregisterDrawable()
        {
            designer.Canvas.EraseVertex(this);
        }

        public override void Print(Action<Action> how)
        {
            how(() => printer.PutVertex(this.Center, embellisher.VertexBrush));
        }

        public override void Register() => designer.Canvas.RegisterVertex(this);

        public override void Move(MouseEventArgs e, Point distance, IRelation sanitizer, bool solo = true)
        {
            this._Pixels.Clear();
            sanitizer.Sanitize(this, ref distance);
            this.p = Functors.MovePoint(this.p, distance);
            this._Pixels.Add(this.p);
            this.Print(designer.Canvas.PrintToPreview);

            if (solo)
            {
                foreach (var line in this.AdjacentLines)
                    ((line)line).Reprint();
            }
        }

        public override void PostMove()
        {
            foreach (var line in this._Drawables.ConvertAll((drawable d) => (line)d))
                line.Register();

            base.PostMove();
        }

        public override void PreMove()
        {
            foreach (var line in this._Drawables.ConvertAll((drawable d) => (line)d))
                line.DeregisterDrawable();

            base.PreMove();
        }

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

            Point midpoint = new Point((int)Math.Ceiling((p1.X + p2.X) / 2.0), (int)Math.Ceiling((p1.Y + p2.Y) / 2.0));
            vertex v = designer.DrawVertex(midpoint, embellisher.VertexBrush);

            foreach (var vx in new List<vertex> { v1, v2 })
            {
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
}
