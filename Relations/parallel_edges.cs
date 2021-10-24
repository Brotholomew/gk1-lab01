using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class ParallelEdges : Relation
    {
        private List<line> _Drawables;
        private HashSet<drawable> _AllDrawables;
        private HashSet<drawable> AllDrawables => this._AllDrawables;
        private List<drawable> ListDrawables;

        private bool PreMoveCompleted = false;
        private bool PostMoveCompleted = false;

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

        #region Prints and Highlights

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)>(this._Drawables.ConvertAll((line l) => ((drawable)l, Embellisher.ParallelBrush)));
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            List<((string, Point), Brush)> temp = new List<((string, Point), Brush)>();

            foreach (var l in this._Drawables)
            {
                Point midpoint = Functors.Midpoint(l.Start, l.End);
                temp.Add((("P", midpoint), Embellisher.StringBrush));
            }

            return temp;
        }

        private void PrintAll()
        {
            foreach (var d in this.AllDrawables)
            {
                Brush b = Embellisher.VertexBrush;

                if (d is line)
                    b = Embellisher.DrawColor;

                d.Print(designer.Canvas.PrintToPreview, b);
            }
        }

        #endregion

        #region Prohibit other Relations

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

        #endregion

        #region Sanitize

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (!this.IsBoundWith(d)) return;

            foreach (var l in this._Drawables)
                if (l.MovingSimultaneously)
                {
                    if (d is line)
                        if (!((line)l).PolyMoving)
                            this.PrintAll();

                    return;
                }
            d.RespondToRelation(this);
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
        }

        #endregion

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
                if (ln is line && this._Drawables.Contains((line)ln)) l = (line)ln;

            if (l == null) return ret;

            line boundLine = this.GetNext(l);
            ret.Add(boundLine);

            return ret;
        }

        #region Moving

        public override void PreMove(vertex v, MovingOpts mo)
        {
            if (this._Break.Contains(v)) return;
            if (!this.IsBoundWith(v)) return;

            this.PreMove();
        }

        public override void PostMove(vertex v, MovingOpts mo)
        {
            if (this._Break.Contains(v)) return;
            if (!this.IsBoundWith(v)) return;

            this.PostMove();
        }

        private void PostMove()
        {
            this.PreMoveCompleted = false;
            if (this.PostMoveCompleted) return;

            this.StackOverflowControl(() =>
            {
                foreach (var line in this._Drawables)
                {
                    foreach (var vx in line.Vertices)
                    {
                        vx.PostMove(new MovingOpts(_stop: true));
                        foreach (var ln in vx.AdjacentLines)
                            ln.PostMove(new MovingOpts(_stop: true, _solo: false));
                    }
                }
            }, this.ListDrawables);

            this.PostMoveCompleted = true;
        }

        private void PreMove()
        {
            this.PostMoveCompleted = false;
            if (this.PreMoveCompleted) return;

            this.StackOverflowControl(() =>
            {
                foreach (var line in this._Drawables)
                {
                    foreach (var vx in line.Vertices)
                    {
                        vx.PreMove(new MovingOpts(_stop: true));
                        foreach (var ln in vx.AdjacentLines)
                            ln.PreMove(new MovingOpts(_stop: true, _solo: false));
                    }
                }
            }, this.ListDrawables);

            this.PreMoveCompleted = true;
        }

        #endregion
    }
}
