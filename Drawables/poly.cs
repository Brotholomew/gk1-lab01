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

        public override void DeregisterDrawable()
        {
            foreach (var line in this._Lines)
                line.DeregisterDrawable();

            foreach (var vertex in this._Vertices)
                vertex.DeregisterDrawable();
        }

        public override void Print(Action<Action> how)
        {
            foreach (var line in this._Lines)
                line.Print(how);

            foreach (var vertex in this._Vertices)
                vertex.Print(how);
        }

        public override void Move(MouseEventArgs e, Point distance, IRelation sanitizer, bool solo = true)
        {
            sanitizer.Sanitize(this, ref distance);

            foreach (var vertex in this._Vertices)
                vertex.Move(e, distance, sanitizer, false);

            foreach (var line in this._Lines)
                line.Move(e, distance, sanitizer, false);

            this.Print(designer.Canvas.PrintToPreview);
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
                designer.Canvas.EraseDrawable(line);

            designer.Canvas.Reprint();
            printer.Erase();
        }
    }
}