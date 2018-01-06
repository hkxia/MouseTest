using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MouseTest
{
    public class myArrow : myShape
    {
        private Rectangle rect;
        private int arrowW = 0;
        private int arrowH = 0;
        private ARROW_ORI ori;
        private myTriangle tri1;
        private myTriangle tri2;

        public enum ARROW_ORI
        {
            END_ARROW_TOP = 0,
            END_ARROW_BOM = 1,
            END_ARROW_BOTH_TB = 3,
            END_ARROW_LEFT = 4,
            END_ARROW_RIGHT = 5,
            END_ARROW_BOTH_LR = 6
        }
        public myArrow(string _name, myRectangle _rect, int _arrowW, int _arrowH, ARROW_ORI _ori)
        {
            rect = new Rectangle(_rect.GetStartPoint().X, _rect.GetStartPoint().Y, _rect.W(), _rect.H());
            arrowW = _arrowW;
            arrowH = _arrowH;
            ori = _ori;
            name = _name;
        }

        public myArrow(string _name, Point _startPoint, int _rectW, int _rectH, int _arrowW, int _arrowH, ARROW_ORI _ori)
        {
            rect = new Rectangle(_startPoint.X, _startPoint.Y, _rectW, _rectH);
            arrowW = _arrowW;
            arrowH = _arrowH;
            ori = _ori;
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

            if (enSolidDraw)
            {
                b = new SolidBrush(brushColor);
                g.FillRectangle(b, rect);
            }
            g.DrawRectangle(pen, rect);

            myTriangle[] ts = GenTriangle(ori, arrowW, arrowH);
            if (ts != null)
            {
                foreach (var t in ts)
                {
                    t.Draw(g, _penColor, brushColor, _enSolidDraw, _penW);
                }
            }

            pen.Dispose();
            if (b != null) b.Dispose();
        }
        /// <summary>
        /// 生成箭头三角形，或1个，或2个箭头
        /// </summary>
        /// <param name="_ori"></param>
        /// <param name="_arrowW"></param>
        /// <param name="_arrowH"></param>
        /// <returns></returns>
        private myTriangle[] GenTriangle(ARROW_ORI _ori, int _arrowW, int _arrowH)
        {
            myTriangle[] tris = null;
            string tname = string.Format("ori{0},sx{1},sy{2},w{3},h{4}", ori.ToString(), rect.X, rect.Y, _arrowW, _arrowH);
            if (_ori == ARROW_ORI.END_ARROW_TOP)
            {
                PointF A = new PointF(rect.X - _arrowW, rect.Y);
                PointF B = new PointF(rect.X + rect.Width + _arrowW, rect.Y);
                PointF C = new PointF(A.X + (B.X - A.X) / 2, A.Y - _arrowH);
                tri1 = new myTriangle(tname, A, B, C);
                tris = new myTriangle[1]{tri1};
            }
            else if (_ori == ARROW_ORI.END_ARROW_BOM)
            {
                PointF A = new PointF(rect.X - _arrowW, rect.Y + rect.Height);
                PointF B = new PointF(rect.X + rect.Width + _arrowW, A.Y);
                PointF C = new PointF(A.X + (B.X - A.X) / 2, A.Y + _arrowH);
                tri1 = new myTriangle(tname, A, B, C);
                tris = new myTriangle[1] { tri1 };
            }
            else if (_ori == ARROW_ORI.END_ARROW_BOTH_TB)
            {
                PointF A = new PointF(rect.X - _arrowW, rect.Y);
                PointF B = new PointF(rect.X + rect.Width + _arrowW, rect.Y);
                PointF C = new PointF(A.X + (B.X - A.X) / 2, A.Y - _arrowH);
                tri1 = new myTriangle(tname, A, B, C);

                PointF A2 = new PointF(rect.X - _arrowW, rect.Y + rect.Height);
                PointF B2 = new PointF(rect.X + rect.Width + _arrowW, A2.Y);
                PointF C2 = new PointF(A2.X + (B2.X - A2.X) / 2, A2.Y + _arrowH);
                tri2 = new myTriangle(tname, A2, B2, C2);

                tris = new myTriangle[2] { tri1, tri2 };
            }
            else if (_ori == ARROW_ORI.END_ARROW_LEFT)
            {
                PointF A = new PointF(rect.X, rect.Y - _arrowW);
                PointF B = new PointF(A.X, rect.Y + rect.Height + _arrowW);
                PointF C = new PointF(A.X - arrowH, A.Y + (B.Y - A.Y) / 2);
                tri1 = new myTriangle(tname, A, B, C);
                tris = new myTriangle[1] { tri1 };
            }
            else if (_ori == ARROW_ORI.END_ARROW_RIGHT)
            {
                PointF A = new PointF(rect.X + rect.Width, rect.Y - _arrowW);
                PointF B = new PointF(A.X, rect.Y + rect.Height + _arrowW);
                PointF C = new PointF(A.X + _arrowH, A.Y + (B.Y - A.Y) / 2);
                tri1 = new myTriangle(tname, A, B, C);
                tris = new myTriangle[1] { tri1 };
            }
            else if (_ori == ARROW_ORI.END_ARROW_BOTH_LR)
            {
                PointF A = new PointF(rect.X, rect.Y - _arrowW);
                PointF B = new PointF(A.X, rect.Y + rect.Height + _arrowW);
                PointF C = new PointF(A.X - arrowH, A.Y + (B.Y - A.Y) / 2);
                tri1 = new myTriangle(tname, A, B, C);

                PointF A2 = new PointF(rect.X + rect.Width, rect.Y - _arrowW);
                PointF B2 = new PointF(A2.X, rect.Y + rect.Height + _arrowW);
                PointF C2 = new PointF(A2.X + _arrowH, A2.Y + (B2.Y - A2.Y) / 2);
                tri2 = new myTriangle(tname, A2, B2, C2);

                tris = new myTriangle[2] { tri1, tri2 };
            }
            else
            {
                
            }

            return tris;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
