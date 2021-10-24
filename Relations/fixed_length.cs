using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class FixedLength : Relation
    {
        private line _Drawable;
        private double _Length;

        public FixedLength(line d)
        {
            this._Drawable = d;
            popup p = new popup((int)d.Length, "length");
            p.ShowDialog();
            this._Length = p.Length;

            d.PreMove(new MovingOpts());
            d.Vertices[0].PreMove(new MovingOpts());
            d.Vertices[0].RespondToRelation(this);
            d.Vertices[0].PostMove(new MovingOpts());
            d.PostMove(new MovingOpts());
        }

        #region Prints and Highlights

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)> { (this._Drawable, Embellisher.FixedLengthHighlightBrush) };
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            Point midpoint = Functors.Midpoint(this._Drawable.Start, this._Drawable.End);
            midpoint = new Point(midpoint.X + 5, midpoint.Y);

            return new List<((string, Point), Brush)> { (($"l = {this._Length}", midpoint), Embellisher.StringBrush) };
        }

        #endregion

        #region Prohibit other Relations

        public override bool ParallelEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }
        public override bool EqualEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }
        public override bool FixedLengthEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }

        #endregion

        #region Sanitize

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            d.RespondToRelation(this);
        }

        public override void SanitizeVertex(vertex v, int nx = 0, int ny = 0)
        {
            if (this._Break.Contains(v)) return;
            if (!v.IsAdjacent(this._Drawable)) return;
            if (this._Drawable.MovingSimultaneously) return;

            this.StackOverflowControl(() => this._Drawable.Rescale(this._Length, v), new List<drawable> { v, this._Drawable.GetNext(v) });
        }

        #endregion

        private List<vertex> GetAffectedVertices(vertex v)
        {
            List<vertex> ret = new List<vertex>();

            if (v.IsAdjacent(this._Drawable))
                ret.Add(this._Drawable.GetNext(v));

            return ret;
        }

        #region Moving

        public override void PreMove(vertex v, MovingOpts mo) 
        {
            if (this._Break.Contains(v))
                return;

            this.StackOverflowControl(() =>
            {
                foreach (var vx in this.GetAffectedVertices(v))
                    vx.PreMove(mo);
            }, v);
        }
        public override void PostMove(vertex v, MovingOpts mo)
        {
            if (this._Break.Contains(v))
                return;

            this.StackOverflowControl(() =>
            {
                foreach (var vx in this.GetAffectedVertices(v))
                    vx.PostMove(mo);
            }, v);
        }

        #endregion

        public override bool IsBoundWith(drawable d) => d == this._Drawable;
    }
}
