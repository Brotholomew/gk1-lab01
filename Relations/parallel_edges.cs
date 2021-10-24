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
        private HashSet<drawable> _AllDrawables;
        private HashSet<drawable> AllDrawables => this._AllDrawables;
        private List<drawable> ListDrawables;

        private void PrintAll()
        {
            foreach (var d in this.AllDrawables)
            {
                Brush b = embellisher.VertexBrush;

                if (d is line)
                    b = embellisher.DrawColor;

                d.Print(designer.Canvas.PrintToPreview, b);
            }

            if (this.AllDrawables.Count > 0) printer.Erase();
        }

        public ParallelEdges(List<line> _Drawables)
        {
            this._Drawables = _Drawables;

            line ln = this._Drawables[1];

            this._AllDrawables = new HashSet<drawable>();
            this.ListDrawables = new List<drawable>();
            foreach (line l in this._Drawables)
            {
                foreach (var v in l.Vertices)
                {
                    this._AllDrawables.Add(v);
                    this.ListDrawables.Add(v);
                    foreach (var d in v.AdjacentLines)
                        if (!this._AllDrawables.Contains(d))
                        {
                            this._AllDrawables.Add(d);
                            this.ListDrawables.Add(d);
                        }
                }
                    
            }

            if (ln.Length != this._Drawables[0].Length)
            {
                ln.PreMove(new MovingOpts());
                this.StackOverflowControl((() => ln.MakeParallel(this._Drawables[0], this._Drawables[0].Length)), ln.Vertices.ConvertAll((vertex v) => (drawable)v));
                ln.PostMove(new MovingOpts());
            }
        }

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)>(this._Drawables.ConvertAll((line l) => ((drawable)l, embellisher.ParallelBrush)));
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            List<((string, Point), Brush)> temp = new List<((string, Point), Brush)>();

            foreach (var l in this._Drawables)
            {
                Point midpoint = Functors.Midpoint(l.Start, l.End);
                temp.Add((("P", midpoint), embellisher.StringBrush));
            }

            return temp;
        }

        public override bool ParallelEnabled(drawable d)
        {
            if (d is line && this._Drawables.Contains((line)d))
                return false;

            return true;
        }
        public override bool EqualEnabled(drawable d)
        {
            if (d is line && this._Drawables.Contains((line)d))
                return false;

            return true;
        }
        public override bool FixedLengthEnabled(drawable d)
        {
            if (d is line && this._Drawables.Contains((line)d))
                return false;

            return true;
        }

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (!this.IsBoundWith(d)) return;
            //if (this._Break.Contains(d)) return;

            foreach (var l in this._Drawables)
                if (l.MovingSimultaneously)
                {
                    if (d is line)
                        if (!((line)l).PolyMoving)
                            this.PrintAll();

                    return;
                }

            //foreach (var l in this._Drawables)
            //    if (l.PolyMoving) return;

            //this.StackOverflowControl(() => d.RespondToRelation(this), this.AllDrawables());
            d.RespondToRelation(this);
        }

        public override void SanitizeLine(line l)
        {
            //if ()
        }

        public override void SanitizeVertex(vertex v, int nx = 0, int ny = 0)
        {
            if (this._Break.Contains(v)) return;

            List<line> affected = this.GetAffectedDrawables(v).ConvertAll((drawable d) => (line)d);

            foreach (var ln in affected)
            {
                line curr = this.GetNext(ln);
                this.StackOverflowControl(() => ln.MakeParallel(curr, curr.Length), this.ListDrawables);
            }

            this.PrintAll();

            //this.StackOverflowControl((() => ln.MakeParallel(this._Drawables[0], this._Length)), ln.Vertices[1]);
        }

        public override bool IsBoundWith(drawable d) => this._AllDrawables.Contains(d);

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
            if (this._Break.Contains(v)) return;
            if (!this.IsBoundWith(v)) return;

            mo.Solo = false;

            this.StackOverflowControl(() =>
            {
                foreach (var line in this._Drawables)
                    line.PreMove(mo);
            }, this.ListDrawables);

            //this._Length = this._Drawables[0].Length;
            //foreach (var d in this.GetAffectedDrawables(v)) /* d.PreMove(mo);*/
            //{
            //    //d.PreMove(mo);
            //    if (this.GetNext((line)d).MovingSimultaneously) return;
            //    // this.GetNext(((line)d)).GetNext(v).PreMove(mo);
            //    mo.Solo = false;
            //    this.StackOverflowControl(() => d.PreMove(mo), v);

            //}
        }
        public override void PostMove(vertex v, MovingOpts mo)
        {
            if (this._Break.Contains(v)) return;
            if (!this.IsBoundWith(v)) return;

            this.StackOverflowControl(() =>
            {
                foreach (var line in this._Drawables)
                    line.PostMove(mo);
            }, this.ListDrawables);

            //foreach (var d in this.GetAffectedDrawables(v))
            //{
            //    //d.PostMove(mo);
            //    if (this.GetNext((line)d).MovingSimultaneously) return; 
            //    // this.GetNext(((line)d)).GetNext(v).PostMove(mo);
            //    this.StackOverflowControl(() => d.PostMove(mo), v);

            //}
        }
    }
}
