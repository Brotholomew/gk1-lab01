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
            designer.Tracked.Clear();
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
