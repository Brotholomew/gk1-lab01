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
    public enum DesignModes { Off, Poly, Circle, DeleteFigure, DeleteVertex, DeleteEdge, Moving };

    public partial class mainForm : Form
    {
        private DesignModes _DesignMode = DesignModes.Off;
        public DesignModes DM { get => this._DesignMode;
            set
            {
                if (value == DesignModes.Off)
                    this.Cursor = embellisher.NormalCursor;

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
        }

        private void clickButtonLineDraw(object sender, EventArgs e)
        {
            this.DM = this._DesignMode == DesignModes.Poly ? DesignModes.Off : DesignModes.Poly;

            if (this._DesignMode == DesignModes.Poly)
                this.Cursor = embellisher.CircleDrawCursor;
        }

        private void MouseClickButtonCircleDraw(object sender, MouseEventArgs e)
        {
            this.DM = this._DesignMode == DesignModes.Circle ? DesignModes.Off : DesignModes.Circle;

            if (this._DesignMode == DesignModes.Circle)
                this.Cursor = embellisher.CircleDrawCursor;
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e, this); 
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            if (this._DesignMode == DesignModes.Poly)
            {
                designer.DrawPoly(e, this);
            }
            else if (this._DesignMode == DesignModes.Circle)
            {
                designer.DrawCircle(e.Location, this, designer.State == PrintingStates.FollowMouse);
            }
            else if (this.DM == DesignModes.DeleteFigure)
            {
                //designer.Canvas.Remove
            }
            else if (this.DM == DesignModes.DeleteEdge)
            {

            }
            else if (this.DM == DesignModes.DeleteVertex)
            {

            }

            if (designer.LastTracked != null)
            {
                drawable d = designer.LastTracked;


                // moving
                this._DesignMode = DesignModes.Moving;
            }

            if (e.Button == MouseButtons.Right)
            {
                designer.Tracked.Clear();
                designer.Canvas.Highlights.Clear();
                designer.Canvas.Reprint();
                printer.Erase();
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(designer.Canvas.MainBitmap, 0, 0);
            e.Graphics.DrawImage(designer.Canvas.Preview, 0, 0);
        }

        private void MouseUpCanvas(object sender, MouseEventArgs e)
        {
            if (designer.LastTracked != null)
            {
                this._DesignMode = DesignModes.Off;
                drawable d = designer.LastTracked;
                
                if (designer.Tracked.Contains(d))
                {
                    designer.Canvas.Highlights.Remove(d);
                    designer.Tracked.Remove(d);
                }
                else
                {
                    designer.Tracked.Add(d);
                    designer.Canvas.Highlights.Add(d, embellisher.SelectedColor);
                }

                designer.LastTracked = null;
                designer.Canvas.Reprint();
                printer.Erase();
            }

            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (designer.Tracked.Count == 1)
            {
                DeleteButton.Enabled = true;

                if (designer.Tracked[0] is line)
                    AddVertexButton.Enabled = true;
            } 
            else
            {
                DeleteButton.Enabled = false;
                AddVertexButton.Enabled = false;
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

        private void MouseClickMoveButton(object sender, MouseEventArgs e)
        {

        }

        private void MouseClickAddVertexButton(object sender, MouseEventArgs e)
        {
            designer.Canvas.Highlights.Remove(designer.Tracked[0]);
            ((line)designer.Tracked[0]).AddVertex();
            designer.Tracked.Clear();
            this.UpdateButtons();
        }
    }
}
