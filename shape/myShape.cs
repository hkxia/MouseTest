using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MouseTest
{
    public class myShape
    {
        public string name = "base_shape";
        public Graphics g = null;
        public Pen pen = null;
        public Brush b = null;
        public Color penColor = Color.Green;
        public Color brushColor = Color.Green;
        public bool enSolidDraw = false;
        public float penW = 1;

        public virtual void Draw(Graphics _g, Color _penColor, Color _brushColor, bool _enSolidDraw = false, float _penW = 1)
        {
            g = _g;
            penColor = _penColor;
            brushColor = _brushColor;
            enSolidDraw = _enSolidDraw;
            penW = _penW;
        }

        public virtual void Dispose()
        {
            if(pen != null) pen.Dispose();
            if(b != null) b.Dispose();
        }
    }
}
