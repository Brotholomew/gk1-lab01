using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    public class RelationSanitizer : Relation
    {
        private List<Relation> _Relations = new List<Relation>();

        public void AddRelation(Relation r) => this._Relations.Add(r);

        #region Prints and Highlights

        public override List<(drawable, Brush)> GetHighlights()
        {
            List<(drawable, Brush)> temp = new List<(drawable, Brush)>();

            foreach (var rel in this._Relations)
                temp.AddRange(rel.GetHighlights());

            return temp;
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            List<((string, Point), Brush)> temp = new List<((string, Point), Brush)>();

            foreach (var rel in this._Relations)
                temp.AddRange(rel.GetStrings());

            return temp;
        }

        #endregion

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            // if (mo.Stop) return;

            foreach (var rel in this._Relations)
                rel.Sanitize(d, ref distance, mo);
        }

        #region Moving

        public override void PreMove(poly p, MovingOpts mo) { foreach (var rel in this._Relations) rel.PreMove(p, mo); }
        public override void PreMove(line l, MovingOpts mo) { foreach (var rel in this._Relations) rel.PreMove(l, mo);  }
        public override void PreMove(circle c, MovingOpts mo) { foreach (var rel in this._Relations) rel.PreMove(c, mo);  }
        public override void PreMove(vertex v, MovingOpts mo) { foreach (var rel in this._Relations) rel.PreMove(v, mo);  }
        public override void PostMove(poly p, MovingOpts mo) { foreach (var rel in this._Relations) rel.PostMove(p, mo); }
        public override void PostMove(line l, MovingOpts mo) { foreach (var rel in this._Relations) rel.PostMove(l, mo); }
        public override void PostMove(circle c, MovingOpts mo) { foreach (var rel in this._Relations) rel.PostMove(c, mo); }
        public override void PostMove(vertex v, MovingOpts mo) { foreach (var rel in this._Relations) rel.PostMove(v, mo); }

        #endregion

        public void Delete(drawable d)
        {
            this._Relations.RemoveAll((Relation rel) => rel.IsBoundWith(d));

            foreach (var v in d.Vertices)
                this._Relations.RemoveAll((Relation rel) => rel.IsBoundWith(v));

        }

        public bool HasRelations(drawable d)
        {
            foreach (var rel in this._Relations)
                if (rel.IsBoundWith(d)) return true;

            return false;
        }

        #region Prohibit other Relations

        public override bool FixedRadiusEnabled(drawable d)
        {
            foreach (var rel in this._Relations)
                if (!rel.FixedRadiusEnabled(d))
                    return false;

            return true;
        }

        public override bool FixedCenterEnabled(drawable d)
        {
            foreach (var rel in this._Relations)
                if (!rel.FixedCenterEnabled(d))
                    return false;

            return true;
        }

        public override bool AdjacentEnabled(drawable d)
        {
            foreach (var rel in this._Relations)
                if (!rel.AdjacentEnabled(d))
                    return false;

            return true;
        }

        public override bool ParallelEnabled(drawable d)
        {
            foreach (var rel in this._Relations)
                if (!rel.ParallelEnabled(d))
                    return false;

            return true;
        }

        public override bool EqualEnabled(drawable d)
        {
            foreach (var rel in this._Relations)
                if (!rel.EqualEnabled(d))
                    return false;

            return true;
        }

        public override bool FixedLengthEnabled(drawable d)
        {
            foreach (var rel in this._Relations)
                if (!rel.FixedLengthEnabled(d))
                    return false;

            return true;
        }

        #endregion
    }
}
