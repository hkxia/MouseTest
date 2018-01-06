using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace MouseTest
{
    public class myCircle : myShape
    {
        private Point c = new Point(0,0);
        private int r = 0;
        private float perimeter = 0;
        private float area = 0;
        private const float PI = 3.1415926f;
        private Rectangle rect;

        /// <summary>
        /// 初始化圆
        /// </summary>
        /// <param name="_center"></param>
        /// <param name="_r"></param>
        public myCircle(string _name, Point _center, int _r)
        {
            c = _center;
            r = _r;
            perimeter = PI*2*r;
            area = PI*r*r;
            rect = new Rectangle(c.X - r, c.Y - r, r*2, r*2);
            name = _name;
        }

        /// <summary>
        /// 画圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen_color"></param>
        /// <param name="brushColor"></param>
        /// <param name="pen_width"></param>
        /// <param name="en_solid_draw"></param>
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

            Point s = new Point(c.X - r, c.Y - r);
            Rectangle rec = new Rectangle(s, new Size(r * 2, r * 2));

            if (enSolidDraw)
            {
                b = new SolidBrush(brushColor);
                g.FillEllipse(b,rec );
            }

            g.DrawEllipse(pen, rec);

            pen.Dispose();
            if (b != null) b.Dispose();
        }

        /// <summary>
        /// 获取圆的外切矩形
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            return rect;
        }

        /// <summary>
        /// 获取圆上的所有点
        /// </summary>
        /// <param name="splitAngle"></param>
        /// <returns></returns>
        public Point[] GetCirclePoint(int splitAngle)
        {
            if (splitAngle <= 0) return null;
            if (c.X == 0 && c.Y == 0) return null;
            if (r <= 0) return null;
            
            Point[] points = new Point[splitAngle];
            for (int i = 0; i < splitAngle; i++)
            {
                int xi = Convert.ToInt16(c.X + r * System.Math.Cos(i*3.14/180));
                int yi = Convert.ToInt16(c.Y + r * System.Math.Sin(i*3.14/180));
                points[i] = new Point(xi,yi);
            }

            return points;
        }
        public Point Center()
        {
            return c;
        }
        public int R()
        {
            return r;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
