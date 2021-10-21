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

        public void AddVertex(vertex pre, vertex v)
        {
            int pre_idx = this.Vertices.FindLastIndex((vertex v) => v == pre);
            this.Vertices.Insert(pre_idx, v);
        }

        public override void DeregisterDrawable()
        {
            foreach (var line in this._Lines)
                line.DeregisterDrawable();

            foreach (var vertex in this._Vertices)
                vertex.DeregisterDrawable();
        }

        public override void Print(Action<Action> how, Brush brush)
        {
            foreach (var line in this._Lines)
                line.Print(how, brush);

            foreach (var vertex in this._Vertices)
                vertex.Print(how, brush);
        }

        public override void Highlight(Action<Action> how, Brush brush)
        {
            how(() => printer.PutFigure(this, brush));
            //base.Highlight(how, brush);
        }

        public override void PostMove(MovingOpts mo)
        {
            if (!mo.Stop)
            {
                mo.Stop = true;
                designer.RelationSanitizer.PostMove(this, mo);
            }

            this.MovingSimultaneously = false;
            base.PostMove(mo);
        }

        public override void PreMove(MovingOpts mo)
        {
            if (!mo.Stop)
            {
                mo.Stop = true;
                designer.RelationSanitizer.PreMove(this, mo);
            }

            this.MovingSimultaneously = true;
            base.PreMove(mo);
        }

        public override void Move(MouseEventArgs e, Point distance, Relation sanitizer, MovingOpts mo)
        {
            sanitizer.Sanitize(this, ref distance);
            mo.Solo = false;

            foreach (var vertex in this._Vertices)
                vertex.Move(e, distance, sanitizer, mo);

            foreach (var line in this._Lines)
                line.Move(e, distance, sanitizer, mo);

            this.Print(designer.Canvas.PrintToPreview, embellisher.DrawColor);
        }

        public override void Register()
        {
            foreach (var l in this._Lines)
                l.Register();

            foreach (var v in this._Vertices)
                v.Register();
        }


        public poly(List<drawable> _lines, List<vertex> _vertices) : base(null, _vertices, Brushes.Black)
        {
            this._Lines = _lines;
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

        public override void RespondToRelation(Relation rel)
        {
            rel.SanitizePoly(this);
        }
    }
}