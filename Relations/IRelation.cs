using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    public interface IRelation
    {
        public void Sanitize(drawable d, ref Point distance, bool opts = false);
    }
}
