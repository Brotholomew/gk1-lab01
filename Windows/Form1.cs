using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab01
{
    public enum DesignModes { Off, Poly, Circle, Moving };

    public partial class mainForm : Form
    {
        private DesignModes _DesignMode = DesignModes.Off;
        public DesignModes DM { get => this._DesignMode;
            set
            {
                if (value == DesignModes.Off)
                    this.Cursor = Embellisher.NormalCursor;

                this._DesignMode = value;
            }
        }

        public mainForm()
        {
            InitializeComponent();
            printer.Initialize(canvas);
            designer.Initialize();
            printer.SetGraphics(designer.Canvas.PreviewGraphics);
            this.UpdateButtons();
            this.PutUpPredefinedScene();
        }

        #region Drawing Handlers

        private void PaintCanvas(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(designer.Canvas.MainBitmap, 0, 0);
            e.Graphics.DrawImage(designer.Canvas.Preview, 0, 0);
        }

        private void MouseMoveCanvas(object sender, MouseEventArgs e) => designer.FollowMouse(e, this);

        private void MouseUpCanvas(object sender, MouseEventArgs e)
        {
            if (designer._Moving != null)
            {
                drawable d = designer._Moving;
                this._DesignMode = DesignModes.Off;
                designer._LastPoint = Point.Empty;
                d.PostMove(new MovingOpts(_stop: false));
            }

            designer.LastTracked = null;
            designer._Moving = null;
            this.UpdateButtons();
        }

        private void MouseDownCanvas(object sender, MouseEventArgs e)
        {
            if (designer.LastTracked != null)
            {
                designer._Moving = designer.LastTracked;
                designer._LastPoint = e.Location;
                designer._Moving.PreMove(new MovingOpts(_stop : false));
                // moving
                this._DesignMode = DesignModes.Moving;
            }
        }

        #endregion

        #region Decorations

        private void UpdateButtons()
        {
            DeleteButton.Enabled = false;
            AddVertexButton.Enabled = false;
            FixedLengthButton.Enabled = false;
            FixedRadiusButton.Enabled = false;
            FixedCenterButton.Enabled = false;
            EqualLengthsButton.Enabled = false;
            ParallelButton.Enabled = false;
            AdjacentCircleButton.Enabled = false;
            DeleteRelationsButton.Enabled = false;

            if (designer.Tracked.Count == 1)
            {
                DeleteButton.Enabled = true;

                if (designer.Tracked[0] is line)
                {
                    AddVertexButton.Enabled = true;

                    if (designer.RelationSanitizer.FixedLengthEnabled(designer.Tracked[0]))
                        FixedLengthButton.Enabled = true;
                }

                if (designer.Tracked[0] is circle)
                {
                    if (designer.RelationSanitizer.FixedRadiusEnabled(designer.Tracked[0])) FixedRadiusButton.Enabled = true;
                    if (designer.RelationSanitizer.FixedCenterEnabled(designer.Tracked[0])) FixedCenterButton.Enabled = true;
                }

                if (designer.RelationSanitizer.HasRelations(designer.Tracked[0]))
                    DeleteRelationsButton.Enabled = true;

            }

            if (designer.Tracked.Count == 2)
            {
                if (designer.Tracked[0] is line && designer.Tracked[1] is line)
                {
                    if (designer.RelationSanitizer.EqualEnabled(designer.Tracked[0]) &&
                        designer.RelationSanitizer.EqualEnabled(designer.Tracked[1]))
                        EqualLengthsButton.Enabled = true;

                    if (designer.RelationSanitizer.ParallelEnabled(designer.Tracked[0]) &&
                        designer.RelationSanitizer.ParallelEnabled(designer.Tracked[1]) &&
                        !((line)designer.Tracked[0]).IsAdjacent((line)designer.Tracked[1]))
                        ParallelButton.Enabled = true;
                }

                if ((designer.Tracked[0] is line && designer.Tracked[1] is circle) ||
                    (designer.Tracked[0] is circle && designer.Tracked[1] is line))
                    if (designer.RelationSanitizer.AdjacentEnabled(designer.Tracked[0]) && designer.Tracked[0] is circle ||
                        designer.RelationSanitizer.AdjacentEnabled(designer.Tracked[1]) && designer.Tracked[1] is circle)
                        AdjacentCircleButton.Enabled = true;
            }
        }

        private void PutUpPredefinedScene()
        {
            // Circle and triangle 1
            Point pc = new Point(450, 100);
            Point pp1 = new Point(450, 300);
            Point pp2 = new Point(500, 400);
            Point pp3 = new Point(300, 450);

            vertex cv = new vertex(pc, new List<Point> { pc }, Embellisher.VertexBrush);
            vertex p1 = new vertex(pp1, new List<Point> { pp1 }, Embellisher.VertexBrush);
            vertex p2 = new vertex(pp2, new List<Point> { pp2 }, Embellisher.VertexBrush);
            vertex p3 = new vertex(pp3, new List<Point> { pp3 }, Embellisher.VertexBrush);

            line l1 = designer.DrawLine(pp1, pp2, Embellisher.DrawColor);
            line l2 = designer.DrawLine(pp2, pp3, Embellisher.DrawColor);
            line l3 = designer.DrawLine(pp3, pp1, Embellisher.DrawColor);

            l1.AddVertex(p1); l1.AddVertex(p2);
            l2.AddVertex(p2); l2.AddVertex(p3);
            l3.AddVertex(p3); l3.AddVertex(p1);

            p1.AddLine(l1); p1.AddLine(l3);
            p2.AddLine(l1); p2.AddLine(l2);
            p3.AddLine(l2); p3.AddLine(l3);

            circle c = designer._DrawCircle(cv, 75, Embellisher.DrawColor);
            poly p = new poly(new List<drawable> { l1, l2, l3 }, new List<vertex> { p1, p2, p3 });

            l1.Poly = p; l2.Poly = p; l3.Poly = p;
            cv.AdjacentLines.Add(c);

            c.Register();
            p.Register();

            designer.RelationSanitizer.AddRelation(new FixedLength(l1, 111));

            // Poly and circle 2
            Point _p1 = new Point(145, 89);
            Point _p2 = new Point(205, 82);
            Point _p3 = new Point(360, 99);
            Point _p4 = new Point(23, 170);
            Point _p5 = new Point(147, 434);
            Point _p6 = new Point(413, 280);
            Point _p7 = new Point(184, 333);
            Point _p8 = new Point(231, 208);

            line _l1 = designer.DrawLine(_p1, _p2, Embellisher.DrawColor);
            line _l2 = designer.DrawLine(_p2, _p3, Embellisher.DrawColor);
            line _l3 = designer.DrawLine(_p3, _p4, Embellisher.DrawColor);
            line _l4 = designer.DrawLine(_p4, _p5, Embellisher.DrawColor);
            line _l5 = designer.DrawLine(_p5, _p6, Embellisher.DrawColor);
            line _l6 = designer.DrawLine(_p6, _p7, Embellisher.DrawColor);
            line _l7 = designer.DrawLine(_p7, _p1, Embellisher.DrawColor);

            vertex _v1 = new vertex(_p1, new List<Point> { _p1 }, Embellisher.VertexBrush);
            vertex _v2 = new vertex(_p2, new List<Point> { _p2 }, Embellisher.VertexBrush);
            vertex _v3 = new vertex(_p3, new List<Point> { _p3 }, Embellisher.VertexBrush);
            vertex _v4 = new vertex(_p4, new List<Point> { _p4 }, Embellisher.VertexBrush);
            vertex _v5 = new vertex(_p5, new List<Point> { _p5 }, Embellisher.VertexBrush);
            vertex _v6 = new vertex(_p6, new List<Point> { _p6 }, Embellisher.VertexBrush);
            vertex _v7 = new vertex(_p7, new List<Point> { _p7 }, Embellisher.VertexBrush);
            vertex _v8 = new vertex(_p8, new List<Point> { _p8 }, Embellisher.VertexBrush);

            _l1.AddVertex(_v1); _l1.AddVertex(_v2);
            _l2.AddVertex(_v2); _l2.AddVertex(_v3);
            _l3.AddVertex(_v3); _l3.AddVertex(_v4);
            _l4.AddVertex(_v4); _l4.AddVertex(_v5);
            _l5.AddVertex(_v5); _l5.AddVertex(_v6);
            _l6.AddVertex(_v6); _l6.AddVertex(_v7);
            _l7.AddVertex(_v7); _l7.AddVertex(_v1);

            _v1.AddLine(_l1); _v1.AddLine(_l7);
            _v2.AddLine(_l1); _v2.AddLine(_l2);
            _v3.AddLine(_l2); _v3.AddLine(_l3);
            _v4.AddLine(_l3); _v4.AddLine(_l4);
            _v5.AddLine(_l4); _v5.AddLine(_l5);
            _v6.AddLine(_l5); _v6.AddLine(_l6);
            _v7.AddLine(_l6); _v7.AddLine(_l7);

            circle ci = designer._DrawCircle(_v8, 61, Embellisher.DrawColor);
            poly po = new poly(new List<drawable> { _l1, _l2, _l3, _l4, _l5, _l6, _l7 }, new List<vertex> { _v1, _v2, _v3, _v4, _v5, _v6, _v7 });

            _l1.Poly = po;
            _l2.Poly = po;
            _l3.Poly = po;
            _l4.Poly = po;
            _l5.Poly = po;
            _l6.Poly = po;
            _l7.Poly = po;
            _v8.AdjacentLines.Add(ci);

            ci.Register(); po.Register();

            designer.RelationSanitizer.AddRelation(new EqualLenghts(new List<line> { _l3, _l5 }));
            designer.RelationSanitizer.AddRelation(new AdjacentCircle(ci, _l7));

            designer.Canvas.Reprint();
        }

        #endregion

        #region Mouse Clicks

        private void MouseClickButtonPolyDraw(object sender, MouseEventArgs e)
        {
            this.DM = this._DesignMode == DesignModes.Poly ? DesignModes.Off : DesignModes.Poly;

            if (this._DesignMode == DesignModes.Poly)
                this.Cursor = Embellisher.CircleDrawCursor;
        }

        private void MouseClickButtonCircleDraw(object sender, MouseEventArgs e)
        {
            this.DM = this._DesignMode == DesignModes.Circle ? DesignModes.Off : DesignModes.Circle;

            if (this._DesignMode == DesignModes.Circle)
                this.Cursor = Embellisher.CircleDrawCursor;
        }

        private void MouseClickCanvas(object sender, MouseEventArgs e)
        {
            if (this._DesignMode == DesignModes.Poly)
            {
                designer.DrawPoly(e, this);
            }
            else if (this._DesignMode == DesignModes.Circle)
            {
                designer.DrawCircle(e.Location, this, designer.State == PrintingStates.FollowMouse);
            }

            if (e.Button == MouseButtons.Right)
            {
                designer.Tracked.Clear();
                designer.Canvas.Highlights.Clear();
                designer.Canvas.Reprint();
                printer.Erase();
            }

            if (designer.LastTracked != null)
            {
                this._DesignMode = DesignModes.Off;
                drawable d = designer.LastTracked;

                if (designer.Tracked.Contains(d))
                {
                    designer.Tracked.Remove(d);
                }
                else
                {
                    designer.Tracked.Add(d);
                }

                designer.Canvas.Reprint();
                printer.Erase();
            }
        }

        private void MouseClickDeleteButton(object sender, MouseEventArgs e)
        {
            drawable d = designer.Tracked[0];
            designer.Tracked.Clear();
            designer.Canvas.Highlights.Remove(d);
            d.Delete();
            this.UpdateButtons();
        }

        private void MouseClickAddVertexButton(object sender, MouseEventArgs e)
        {
            designer.Canvas.Highlights.Remove(designer.Tracked[0]);
            ((line)designer.Tracked[0]).AddVertex();
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickFixedLengthButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new FixedLength((line)designer.Tracked[0]));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickFixedRadiusButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new FixedRadius((circle)designer.Tracked[0]));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickFixedCenterButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new FixedCenter((circle)designer.Tracked[0]));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickEqualLengthsButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new EqualLenghts(designer.Tracked.ConvertAll((drawable d) => (line)d)));
            designer.Tracked = new List<drawable>();
            this.UpdateButtons();
        }

        private void MouseClickParallelEdges(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new ParallelEdges(designer.Tracked.ConvertAll((drawable d) => (line)d)));
            designer.Tracked = new List<drawable>();
            this.UpdateButtons();
        }

        private void MouseClickAdjacentCircleButton(object sender, MouseEventArgs e)
        {
            circle c = null;
            line l = null;
            foreach (var d in designer.Tracked)
            {
                if (d is line) l = (line)d;
                if (d is circle) c = (circle)d;
            }

            designer.RelationSanitizer.AddRelation(new AdjacentCircle(c, l));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickDeleteRelationsButton(object sender, MouseEventArgs e)
        {
            drawable d = designer.Tracked[0];
            designer.RelationSanitizer.Delete(d);
            this.UpdateButtons();
        }

        #endregion
    }
}
