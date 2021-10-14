using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class Canvas
    {
        private Dictionary<Point, List<drawable>> _DrawableMap = new Dictionary<Point, List<drawable>>();
        private List<drawable> _DrawablesHeap = new List<drawable>();

        private Dictionary<Point, List<drawable>> _VertexMap = new Dictionary<Point, List<drawable>>();
        private List<vertex> _VerticesHeap = new List<vertex>();

        private Bitmap _MainBitmap = new Bitmap(printer.Width, printer.Height);
        private Bitmap _Preview = new Bitmap(printer.Width, printer.Height);

        public Bitmap MainBitmap { get => this._MainBitmap; }
        public Bitmap Preview { get => this._Preview; }

        private Graphics _MainGraphics;
        private Graphics _PreviewGraphics;

        public Graphics MainGraphics { get => this._MainGraphics; }
        public Graphics PreviewGraphics { get => this._PreviewGraphics; }

        public Canvas()
        {
            this._MainGraphics = Graphics.FromImage(this._MainBitmap);
            this._PreviewGraphics = Graphics.FromImage(this._Preview);

            this._MainGraphics.Clear(Color.White);
            this._PreviewGraphics.Clear(Color.Transparent);
        }

        private void InitializePixel(Point p, Dictionary<Point, List<drawable>> map, drawable d)
        {
            if (!map.ContainsKey(p))
                map.Add(p, new List<drawable>());

            map[p].Add(d);
        }

        private void InitializePixels(List<Point> pixels, Dictionary<Point, List<drawable>> map, drawable d)
        {
            foreach (var pixel in d.Pixels)
                this.InitializePixel(pixel, map, d);
        }

        public void RegisterVertex(vertex v)
        {
            this.InitializePixels(v.Pixels, this._VertexMap, v);
            this._VerticesHeap.Add(v);
        }

        public void RegisterDrawable(drawable d)
        {
            this.InitializePixels(d.Pixels, this._DrawableMap, d);
            this._DrawablesHeap.Add(d);
        }

        public void Reprint()
        {
            this.PrintToMain(() =>
            {
                List<Point> temp = new List<Point>();

                foreach (var d in this._DrawablesHeap)
                    printer.PutPixels(d.Pixels, d.Brush);

                foreach (var v in this._VerticesHeap)
                    this.PrintVertex(v);
            });
        }

        public void PrintToMain(Action a)
        {
            printer.SetGraphics(this._MainGraphics);
            a.Invoke();
            printer.SetGraphics(this._PreviewGraphics);
        }

        public void PrintDrawable(drawable d) => this.PrintToMain(() => printer.PutPixels(d.Pixels, d.Brush));

        public void PrintVertex(vertex v) => this.PrintToMain(() => printer.PutVertex(v.Pixels[0], v.Brush));

        public void ErasePreview()
        {
            this._PreviewGraphics.Clear(Color.Transparent);
        }

        public void EraseDrawable(drawable d)
        {
            foreach (var pixel in d.Pixels)
            {
                this._DrawableMap[pixel].Remove(d);
                this._DrawablesHeap.Remove(d);
            }

            this.Reprint();
            printer.Erase();
        }

        public void EraseVertex(vertex v)
        {
            this._VertexMap[v.Pixels[0]].Remove(v);
            this._VerticesHeap.Remove(v);

            this.Reprint();
            printer.Erase();
        }

        public void DeregisterDrawable(drawable d)
        {
            foreach (var pixel in d.Pixels)
                if (this._DrawableMap.ContainsKey(pixel)) this._DrawableMap[pixel].Remove(d);

            this._DrawablesHeap.Remove(d);
        }

        public void DeregisterDrawables(List<drawable> drawables)
        {
            foreach (var d in drawables)
                this.DeregisterDrawable(d);
        }
    }
}
