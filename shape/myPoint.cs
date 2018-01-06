using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace MouseTest
{
    public class myPoint : myShape
    {
        private int x = 0;
        private int y = 0;
        private Point p;

        public myPoint(string _name, int _x, int _y)
        {
            x = _x;
            y = _y;
            p.X = x;
            p.Y = y;
            name = _name;
        }
        public myPoint(string _name, Point _p)
        {
            x = _p.X;
            y = _p.Y;
            p = _p;
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

            Point[] ps = new Point[]{p,p};
            b = null;
            pen = new Pen(penColor, penW);

            if (enSolidDraw)
            {
                b = new SolidBrush(brushColor);
                g.FillEllipse(b, p.X, p.Y, 2,2);
            }
            try
            {
                g.DrawEllipse(pen, p.X, p.Y, penW, penW);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }

            pen.Dispose();
            if (b != null) b.Dispose();
        }

        public Point GetPoint()
        {
            return p;
        }

        public int X()
        {
            return x;
        }
        public int Y()
        {
            return y;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
