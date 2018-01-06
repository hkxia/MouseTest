using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Algorithm;
using Data;
using GDI2D;
using X3D;

namespace MouseTest
{
    public class DataProc
    {
        /// <summary>
        /// 按键测试
        /// </summary>
        /// <param name="tresult"></param>
        /// <param name="enTestItem"></param>
        /// <param name="status"></param>
        public static void BtnStatus(DataStruct.MouseTestResultData tresult, Dictionary<data.TestItem, bool> enTestItem, ref DataStruct.MouseBtnResult status)
        {
            if(tresult == null || enTestItem == null) return;

            //左按键
            if (tresult.btnL && enTestItem[data.TestItem.EN_L_DOWN])
            {
                status.L_Btn.key_down = true; return;
            }
            if (!tresult.btnL && enTestItem[data.TestItem.EN_L_UP])
            {
                status.L_Btn.key_up = true; return;
            }
            //右按键
            if (tresult.btnR && enTestItem[data.TestItem.EN_R_DOWN])
            {
                status.R_Btn.key_down = true; return;
            }
            if (!tresult.btnR && enTestItem[data.TestItem.EN_R_UP])
            {
                status.R_Btn.key_up = true; return;
            }
            //滚轮按键
            if (tresult.btnWheel && enTestItem[data.TestItem.EN_W_DOWN])
            {
                status.W_Btn.key_down = true; return;
            }
            if (!tresult.btnWheel && enTestItem[data.TestItem.EN_W_UP])
            {
                status.W_Btn.key_up = true; return;
            }
            //侧前按键
            if (tresult.btnSideFront && enTestItem[data.TestItem.EN_SF_DOWN])
            {
                status.SF_Btn.key_down = true; return;
            }
            if (!tresult.btnSideFront && enTestItem[data.TestItem.EN_SF_UP])
            {
                status.SF_Btn.key_up = true; return;
            }
            //侧后按键
            if (tresult.btnSideBack && enTestItem[data.TestItem.EN_SB_DOWN])
            {
                status.SB_Btn.key_down = true; return;
            }
            if (!tresult.btnSideBack && enTestItem[data.TestItem.EN_SB_UP])
            {
                status.SB_Btn.key_up = true; return;
            }

            return;
        }
        /// <summary>
        /// 滚轮转动测试
        /// </summary>
        /// <param name="tresult"></param>
        /// <param name="enTestItem"></param>
        /// <param name="wheelCount"></param>
        public static void WheelStatus(DataStruct.MouseTestResultData tresult, Dictionary<data.TestItem, bool> enTestItem, ref DataStruct.WHEEL_COUNT wheelCount)
        {
            if (tresult == null || enTestItem == null) return;
            if (tresult.curWheelDir == DataStruct.WHEEL_DIR.FORWARD && enTestItem[data.TestItem.EN_WHEEL_FORWARD])
            {                
                wheelCount.SetForwardOnce();
            }
            else if (tresult.curWheelDir == DataStruct.WHEEL_DIR.BACKWARD && enTestItem[data.TestItem.EN_WHEEL_BACKWARD])
            {
                wheelCount.SetBackwardOnce();
            }
        }
        /// <summary>
        /// 滚轮3D滚动显示
        /// </summary>
        /// <param name="tresult"></param>
        /// <param name="enTestItem"></param>
        /// <param name="wheelCount"></param>
        /// <param name="wheel3D"></param>
        public static void WheelStatus3DStep(DataStruct.MouseTestResultData tresult, Dictionary<data.TestItem, bool> enTestItem, DataStruct.WHEEL_COUNT wheelCount, DX3D wheel3D)
        {
            if (tresult == null || enTestItem == null) return;
            if (tresult.curWheelDir == DataStruct.WHEEL_DIR.FORWARD && enTestItem[data.TestItem.EN_WHEEL_FORWARD])
            {
                wheel3D.SetForeward(wheelCount.ForwardCount += 100 / 24);
            }
            else if (tresult.curWheelDir == DataStruct.WHEEL_DIR.BACKWARD && enTestItem[data.TestItem.EN_WHEEL_BACKWARD])
            {
                wheel3D.SetBackward(wheelCount.BackwardCount += 100 / 24);
            }
        }

        public static void CircleTest(DataStruct.MouseTestResultData tresult, Dictionary<data.TestItem, bool> enTestItem, ref int startDrawX, ref int startDrawY, Graphics g, DataStruct.ProductIndex idx)
        {
            if (tresult == null || enTestItem == null || g == null) return;
            if (enTestItem[data.TestItem.EN_CIRCLE])
            {
                startDrawX += tresult.X;
                startDrawY += tresult.Y;

                //当前点
                myPoint p = new myPoint("", startDrawX, startDrawY);
                //外圆
                myCircle exCir = (myCircle) Result.GetShape(idx, DataStruct.TestType.DRAW_CIRCLE_EX).shape;
                //内圆
                myCircle innerCir = (myCircle) Result.GetShape(idx, DataStruct.TestType.DRAW_CIRCLE_INNER).shape;
                //判断当前点是否在制定的内圆和外圆之间
                bool cirRes = PointAlgorithm.IsPointInTwoCircle(p, exCir, innerCir);
                if (cirRes)
                {
                    p.Draw(g, DataStruct.myColor.PASS, DataStruct.myColor.PASS, true, 1);
                }
                else
                {
                    p.Draw(g, DataStruct.myColor.FAIL, DataStruct.myColor.FAIL, true, 1);
                }

                p.Dispose();
                exCir.Dispose();
                innerCir.Dispose();
            }
        }
        public static void TriTest(DataStruct.MouseTestResultData tresult, Dictionary<data.TestItem, bool> enTestItem, ref int startDrawX, ref int startDrawY, Graphics g, DataStruct.ProductIndex idx)
        {
            if (tresult == null || enTestItem == null || g == null) return;
            if (enTestItem[data.TestItem.EN_TRIANGLE])
            {
                startDrawX += tresult.X;
                startDrawY += tresult.Y;

                myPoint p = new myPoint("", startDrawX, startDrawY);
                myTriangle exTri = (myTriangle)Result.GetShape(idx, DataStruct.TestType.DRAW_TRIANGLE_EX).shape;
                myTriangle innerTri = (myTriangle)Result.GetShape(idx, DataStruct.TestType.DRAW_TRIANGLE_INNER).shape;
                bool cirRes = PointAlgorithm.IsPointInTwoTriangle(p.GetPoint(), exTri, innerTri);
                if (cirRes)
                {
                    p.Draw(g, DataStruct.myColor.PASS, DataStruct.myColor.PASS, true, 1);
                }
                else
                {
                    p.Draw(g, DataStruct.myColor.FAIL, DataStruct.myColor.FAIL, true, 1);
                }

                p.Dispose();
                exTri.Dispose();
                innerTri.Dispose();
            }
        }
    }
}
