using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data;
using GDI2D;
using System.Drawing;


namespace MouseTest
{
    public class Result
    {
        public static void ShowResult(Graphics g, DataStruct.ProductIndex index, DataStruct.TestType ttype, DataStruct.TestResult result)
        {
            if (data.AllShapes == null) return;

            foreach (DataStruct.ShowShapes r in data.AllShapes)
            {
                if (data.GenShapeName(index, ttype) == r.shape.Name())
                {
                    if (result == DataStruct.TestResult.PASSED)
                    {
                        r.shape.Draw(g, DataStruct.myColor.PASS, DataStruct.myColor.PASS, r.solidFill, 1);
                    }
                    else if(result == DataStruct.TestResult.FAILED)
                    {
                        r.shape.Draw(g, DataStruct.myColor.FAIL, DataStruct.myColor.FAIL, r.solidFill, 1);
                    }
                    else if (result == DataStruct.TestResult.STDBY)
                    {
                        r.shape.Draw(g, DataStruct.myColor.STDBY, DataStruct.myColor.STDBY, r.solidFill, 1);
                    }
                    else
                    {
                        r.shape.Draw(g, DataStruct.myColor.STDBY2, DataStruct.myColor.STDBY2, r.solidFill, 1);
                    }
                }
            }
        }
        public static DataStruct.ShowShapes GetShape(DataStruct.ProductIndex index, DataStruct.TestType ttype)
        {
            if (data.AllShapes == null) return null;

            foreach (DataStruct.ShowShapes r in data.AllShapes)
            {
                if (data.GenShapeName(index, ttype) == r.shape.Name())
                {
                    return r;
                }
            }

            return null;
        }

        public static void InitShowResult(TableLayoutControlCollection ctrls, Color bc)
        {
            if(ctrls == null) return;
            foreach (Control c in ctrls)
            {
                if (c is TextBox)
                {
                    c.BackColor = bc;
                }
            }
        }
        public static void Init2DShowResult(Graphics btnG, Graphics drawG, DataStruct.ProductIndex idx, DataStruct.TestResult tresult)
        {
            foreach (DataStruct.TestType t in Enum.GetValues(typeof(DataStruct.TestType)))
            {
                if (t.ToString().Contains("DRAW"))
                {
                    ShowResult(drawG, idx, t, tresult);
                }
                else
                {
                    ShowResult(btnG, idx, t, tresult);
                }
            }
        }
    }
}
