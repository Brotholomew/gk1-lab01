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
            List<Point> temp = new List<Point>();

            foreach (var d in this._DrawablesHeap)
                temp.AddRange(d.Pixels);

            printer.PutPixels(temp, Brushes.Black);

            foreach (var v in this._VerticesHeap)
                this.PrintVertex(v);
        }

        public void PrintDrawable(drawable d) => printer.PutPixels(d.Pixels, d.Brush);

        public void PrintVertex(vertex v) => printer.PutVertex(v.Pixels[0], v.Brush);

        public void ErasePreview()
        {
            printer.Erase();
            this.Reprint();
        }

        public void EraseDrawable(drawable d)
        {
            foreach (var pixel in d.Pixels)
            {
                this._DrawableMap[pixel].Remove(d);
                this._DrawablesHeap.Remove(d);
            }

            this.ErasePreview();
        }

        public void EraseVertex(vertex v)
        {
            this._VertexMap[v.Pixels[0]].Remove(v);
            this._VerticesHeap.Remove(v);

            this.ErasePreview();
        }

        public void DeregisterDrawable(drawable d)
        {
            foreach (var pixel in d.Pixels)
                this._DrawableMap[pixel].Remove(d);

            this._DrawablesHeap.Remove(d);
        }

        public void DeregisterDrawables(List<drawable> drawables)
        {
            foreach (var d in drawables)
                this.DeregisterDrawable(d);
        }
    }
}
