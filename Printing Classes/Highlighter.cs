﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    public class Highlighter
    {
        public List<(drawable, Brush)> Highlights
        {
            get
            {
                List<(drawable, Brush)> temp = new List<(drawable, Brush)>();
                temp.AddRange(designer.RelationSanitizer.GetHighlights());
                temp.AddRange(designer.Tracked.ConvertAll((drawable d) => (d, Embellisher.SelectedColor)));

                return temp;
            }
        }

        public List<((string, Point), Brush)> Strings
        {
            get => designer.RelationSanitizer.GetStrings();
        }
    }
}
