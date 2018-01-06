using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MouseTest
{
    public class myRectangle: myShape
    {
        private Point s;
        private int w = 0;
        private int h = 0;
        private int perimeter = 0;

        public myRectangle(string _name, Point _start, int _w, int _h)
        {
            s = _start;
            w = _w;
            h = _h;
            perimeter = h*2 + w*2;
            name = _name;
        }

        public myRectangle(string _name, Point _center, int _w, int _h, int angle)
        {
            s = new Point(_center.X - _w/2, _center.Y - _h/2);
            w = _w;
            h = _h;
            perimeter = h * 2 + w * 2;
            name = _name;
        }

        public override void Draw(Graphics _g, Color _penColor, Color _brushColor, bool _enSolidDraw = false, float _penW = 1)
        {
            if (_g == null) return;
            if (_g.IsClipEmpty) return;

            g = _g;
            penColor = _penColor;
            brushColor = _brushColor;
            enSolidDraw = _enSolidDraw;
            penW = _penW;

            b = null;
            pen = new Pen(penColor, penW);
            Rectangle rec = new Rectangle(s, new Size(w, h));

            if (enSolidDraw)
            {
                b = new SolidBrush(brushColor);
                g.FillRectangle(b, rec);
            }

            g.DrawRectangle(pen, rec);

            pen.Dispose();
            if (b != null) b.Dispose();
        }

        public Point GetStartPoint()
        {
            return s;
        }

        public int W()
        {
            return w;
        }
        public int H()
        {
            return h;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
