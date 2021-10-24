using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace lab01
{
    public class poly : drawable
    {
        private List<drawable> _Lines;

        public List<drawable> Lines { get => this._Lines; }
        public bool MovingSimultaneously = false;

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

        public void AddVertex(vertex pre, vertex v, vertex post)
        {
            int pre_idx = this.Vertices.FindLastIndex((vertex v) => v == pre);
            int post_idx = this.Vertices.FindLastIndex((vertex v) => v == post);

            if ((pre_idx == this.Vertices.Count && post_idx == 0) || (post_idx == this.Vertices.Count && pre_idx == 0))
                this.Vertices.Add(v);
            else
            {
                int bigger = pre_idx > post_idx ? pre_idx : post_idx;
                this.Vertices.Insert(bigger, v);
            }
        }

        #region Reprints and Highlights

        public override void Print(Action<Action> how, Brush brush)
        {
            foreach (var line in this._Lines)
                line.Print(how, brush);

            foreach (var vertex in this._Vertices)
                vertex.Print(how, Embellisher.VertexBrush);
        }

        public override void Reprint(int nx = 0, int ny = 0)
        {
            foreach (var line in this._Lines)
                line.Print(designer.Canvas.PrintToPreview, Embellisher.DrawColor);

            foreach (var vertex in this._Vertices)
                vertex.Print(designer.Canvas.PrintToPreview, Embellisher.VertexBrush);
        }

        public override void Highlight(Action<Action> how, Brush brush)
        {
            how(() => printer.PutFigure(this, brush));
        }

        #endregion

        #region Moving

        public override void Move(MouseEventArgs e, Point distance, Relation sanitizer, MovingOpts mo)
        {
            sanitizer.Sanitize(this, ref distance);
            mo.Solo = false;

            foreach (var vertex in this._Vertices)
                vertex.Move(e, distance, sanitizer, mo);

            foreach (var line in this._Lines)
                line.Move(e, distance, sanitizer, mo);

            this.Print(designer.Canvas.PrintToPreview, Embellisher.DrawColor);
        }

        public override void PostMove(MovingOpts mo)
        {
            designer.RelationSanitizer.PostMove(this, mo);
            this.MovingSimultaneously = false;
            base.PostMove(mo);
        }

        public override void PreMove(MovingOpts mo)
        {
            designer.RelationSanitizer.PreMove(this, mo);
            this.MovingSimultaneously = true;
            base.PreMove(mo);
        }

        #endregion

        #region Canvas Register and Deregister

        public override void Register()
        {
            foreach (var l in this._Lines)
                l.Register();

            foreach (var v in this._Vertices)
                v.Register();
        }

        public override void DeregisterDrawable()
        {
            foreach (var line in this._Lines)
                line.DeregisterDrawable();

            foreach (var vertex in this._Vertices)
                vertex.DeregisterDrawable();
        }

        #endregion

        public override void RespondToRelation(Relation rel, int nx = 0, int ny = 0)
        {
            rel.SanitizePoly(this);
        }

        public override void Delete()
        {
            designer.Canvas.EraseVertices(this.Vertices);

            foreach (var line in this._Lines)
            {
                designer.Canvas.EraseDrawable(line);
                designer.RelationSanitizer.Delete(line);
            }

            designer.RelationSanitizer.Delete(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }
    }
}