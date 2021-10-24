using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    public abstract class Relation
    {
        public virtual void Sanitize(drawable d, ref Point distance, MovingOpts mo = new MovingOpts()) { }
        public virtual List<(drawable, Brush)> GetHighlights() => new List<(drawable, Brush)>();
        public virtual List<((string, Point), Brush)> GetStrings() => new List<((string, Point), Brush)>();

        public virtual void SanitizePoly(poly p) { }
        public virtual void SanitizeLine(line l) { }
        public virtual void SanitizeCricle(circle c) { }
        public virtual void SanitizeVertex(vertex v, int nx = 0, int ny = 0) { }

        public virtual void PreMove(poly p, MovingOpts mo) { }
        public virtual void PreMove(line l, MovingOpts mo) { }
        public virtual void PreMove(circle c, MovingOpts mo) { }
        public virtual void PreMove(vertex v, MovingOpts mo) { }
        public virtual void PostMove(poly p, MovingOpts mo) { }
        public virtual void PostMove(line l, MovingOpts mo) { }
        public virtual void PostMove(circle c, MovingOpts mo) { }
        public virtual void PostMove(vertex v, MovingOpts mo) { }

        protected List<drawable> _Break = new List<drawable>();
        protected void StackOverflowControl(Action a, drawable d)
        {
            this._Break.Add(d);
            a.Invoke();
            this._Break.Remove(d);
        }

        protected void StackOverflowControl(Action a, List<drawable> ld)
        {
            this._Break.AddRange(ld);
            a.Invoke();

            foreach (var _d in ld)
                this._Break.Remove(_d);           
        }

        public virtual bool IsBoundWith(drawable d) => false;

        public virtual bool FixedRadiusEnabled(drawable d) => true;
        public virtual bool FixedCenterEnabled(drawable d) => true;
        public virtual bool AdjacentEnabled(drawable d) => true;
        public virtual bool ParallelEnabled(drawable d) => true;
        public virtual bool EqualEnabled(drawable d) => true;
        public virtual bool FixedLengthEnabled(drawable d) => true;
    }
}
