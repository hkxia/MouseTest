using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace MouseTest
{
    public class myTriangle : myShape
    {
        private PointF A;
        private PointF B;
        private PointF C;

        public myTriangle(string _name, PointF a,PointF b,PointF c)
        {
            A = a;
            B = b;
            C = c;
            name = _name;
        }

        /// <summary>
        /// 绘制三角形
        /// </summary>
        /// <param name="g"></param>
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
            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddLine(A, B);
            graphPath.AddLine(B, C);
            graphPath.AddLine(C, A);

            if (enSolidDraw)
            {
                b = new SolidBrush(brushColor);
                g.FillPath(b, graphPath);
            }

            g.DrawLine(pen, A, B);
            g.DrawLine(pen, B, C);
            g.DrawLine(pen, C, A);

            if (b != null) b.Dispose();
            pen.Dispose();
            graphPath.Dispose();
        }

        /// <summary>
        /// 三角形旋转
        /// </summary>
        /// <param name="degrees"></param>
        public void Rotate(int degrees)
        {
            float angle = (float)(degrees / 360f * Math.PI);
            float newX = (float)(A.X * Math.Cos(angle) - A.Y * Math.Sin(angle));
            float newY = (float)(A.X * Math.Sin(angle) + A.Y * Math.Cos(angle));

            A.X = newX;
            A.Y = newY;

            newX = (float)(B.X * Math.Cos(angle) - B.Y * Math.Sin(angle));
            newY = (float)(B.X * Math.Sin(angle) + B.Y * Math.Cos(angle));

            B.X = newX;
            B.Y = newY;

            newX = (float)(C.X * Math.Cos(angle) - C.Y * Math.Sin(angle));
            newY = (float)(C.X * Math.Sin(angle) + C.Y * Math.Cos(angle));

            C.X = newX;
            C.Y = newY;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
