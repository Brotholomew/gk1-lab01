using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class ParallelEdges : Relation
    {
        private List<line> _Drawables;
        private double _Length;

        public ParallelEdges(List<line> _Drawables)
        {
            this._Drawables = _Drawables;

            line ln = this._Drawables[1];

            if (ln.Length != this._Drawables[0].Length)
            {
                ln.PreMove(new MovingOpts());
                this.StackOverflowControl((() => ln.MakeParallel(this._Drawables[0], this._Drawables[0].Length)), ln.Vertices.ConvertAll((vertex v) => (drawable)v));
                ln.PostMove(new MovingOpts());
            }
        }

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)>(this._Drawables.ConvertAll((line l) => ((drawable)l, embellisher.DrawColor)));
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            //Point midpoint = Functors.Midpoint(this._Drawable.Vertices[0].Center, new Point(this._Drawable.Vertices[0].Center.X + this._Drawable.Radius, this._Drawable.Vertices[0].Center.Y));

            //return new List<((string, Point), Brush)> { (("fixed radius", midpoint), embellisher.FixedLengthHighlightBrush) };
            return new List<((string, Point), Brush)>();
        }

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (this._Break.Contains(d)) return;

            foreach (var l in this._Drawables)
                if (l.MovingSimultaneously) return;

            //foreach (var l in this._Drawables)
            //    if (l.PolyMoving) return;

            this.StackOverflowControl(() => d.RespondToRelation(this), d);
        }

        public override void SanitizeLine(line l)
        {
            //if ()
        }

        public override void SanitizeVertex(vertex v)
        {
            List<line> affected = this.GetAffectedDrawables(v).ConvertAll((drawable d) => (line)d);

            foreach (var ln in affected)
            {
                line curr = this.GetNext(ln);
                this.StackOverflowControl((() => ln.MakeParallel(curr, curr.Length)), ln.Vertices.ConvertAll((vertex v) => (drawable)v));
            }

            //this.StackOverflowControl((() => ln.MakeParallel(this._Drawables[0], this._Length)), ln.Vertices[1]);
        }

        public override bool IsBoundWith(drawable d) => d is line && this._Drawables.Contains((line)d);

        private line GetNext(line l)
        {
            foreach (var line in this._Drawables)
                if (line != l) return line;

            return null;
        }

        private List<drawable> GetAffectedDrawables(vertex v)
        {
            line l = null;
            List<drawable> ret = new List<drawable>();

            foreach (var ln in v.AdjacentLines)
                if (this.IsBoundWith(ln)) l = (line)ln;

            if (l == null) return ret;

            line boundLine = this.GetNext(l);
            ret.Add(boundLine);

            return ret;
        }

        public override void PreMove(vertex v, MovingOpts mo)
        {
            this._Length = this._Drawables[0].Length;
            foreach (var d in this.GetAffectedDrawables(v)) /* d.PreMove(mo);*/
            {
                //d.PreMove(mo);
                if (this.GetNext((line)d).MovingSimultaneously) return;
                mo.Solo = false;
                d.PreMove(mo);

            }
        }
        public override void PostMove(vertex v, MovingOpts mo)
        {
            foreach (var d in this.GetAffectedDrawables(v))
            {
                //d.PostMove(mo);
                if (this.GetNext((line)d).MovingSimultaneously) return; 
                d.PostMove(mo);

            }
        }
    }
}
