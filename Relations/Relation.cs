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
        public virtual void SanitizeVertex(vertex v) { }

        public virtual void PreMove(poly p, MovingOpts mo) { }
        public virtual void PreMove(line l, MovingOpts mo) { }
        public virtual void PreMove(circle c, MovingOpts mo) { }
        public virtual void PreMove(vertex v, MovingOpts mo) { }
        public virtual void PostMove(poly p, MovingOpts mo) { }
        public virtual void PostMove(line l, MovingOpts mo) { }
        public virtual void PostMove(circle c, MovingOpts mo) { }
        public virtual void PostMove(vertex v, MovingOpts mo) { }

        public virtual bool IsBoundWith(drawable d) => false;
    }
}
