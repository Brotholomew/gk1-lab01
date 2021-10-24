using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    public struct MovingOpts
    {
        public bool Solo;
        public bool Stop;
        public bool CircleOpts;
        public bool CircleVMoving;

        public MovingOpts(bool _solo = true, bool _stop = false, bool _circleOpts = false, bool _circleVMoving = false)
        {
            this.Solo = _solo;
            this.Stop = _stop;
            this.CircleOpts = _circleOpts;
            this.CircleVMoving = _circleVMoving;
        }
    }
}
