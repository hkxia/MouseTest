using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using Algorithm;
using Data;
using GDI2D;
using X3D;


namespace MouseTest
{
    public class data
    {
        //左边按键坐标和半径
        public static Point LBtnC = new Point(300, 430);
        public static int LBtnCirR = 20;

        //右边按键坐标和半径
        public static Point RBtnC = new Point(120, 430);
        public static int RBtnCirR = 20;

        //DPI按键画图位置和长宽
        public static Point dpiPlus = new Point(195, 250);
        public static Point dpiMinus = new Point(195, 200);
        public static int dpiRecW = 20;
        public static int dpiRecH = 35;

        //Wheel按键画图位置和长宽
        public static Point wheelBtn = new Point(195, 300);
        public static int wheelBtnRecW = 20;
        public static int wheelBtnRecH = 35;

        //侧面按键
        public static Point sideBtnC1 = new Point(365, 150);
        public static int sideRectW1 = 20;
        public static int sideRectH1 = 25;
        public static int sideArrowW1 = 10;
        public static int sideArrowH1 = 20;
        public static Point sideBtnC2 = new Point(380, 250);
        public static int sideRectW2 = 20;
        public static int sideRectH2 = 25;
        public static int sideArrowW2 = 10;
        public static int sideArrowH2 = 20;
        public static myArrow.ARROW_ORI sideBtnOri = myArrow.ARROW_ORI.END_ARROW_LEFT;

        //画向上箭头
        public static Point wheelArrowRectCT = new Point(195, 430);
        public static int wheelRectWT = 20;
        public static int wheelRectHT = 30;
        public static int wheelArrowWT = 10;
        public static int wheelArrowHT = 20;
        //画向下箭头
        public static Point wheelArrowRectCB = new Point(195, 460);
        public static int wheelRectWB = 20;
        public static int wheelRectHB = 30;
        public static int wheelArrowWB = 10;
        public static int wheelArrowHB = 20;
        //滚轮测试箭头方向
        public static myArrow.ARROW_ORI wheelOriUp = myArrow.ARROW_ORI.END_ARROW_TOP;
        public static myArrow.ARROW_ORI wheelOriDown = myArrow.ARROW_ORI.END_ARROW_BOM;

        //画内外圆
        public static Point exCen = new Point(200, 150);
        public static int exR = 270/2;
        public static Point innerCen = new Point(200, 150);
        public static int innerR = 230/2;
        public static myCircle exCircle = new myCircle("exStdCircle", exCen, exR);
        public static myCircle innerCircle = new myCircle("innerStdCircle", innerCen, innerR);

        //画三角形
        public static Point innerA = new Point(300, 650);
        public static Point innerB = new Point(50, 650);
        public static Point innerC = new Point(300, 400);
        public static int gap = 10;
        public static Point exA = new Point(innerA.X + gap , innerA.Y + gap);
        public static Point exB = new Point((int)(innerB.X - gap * 2.5), innerB.Y + gap);
        public static Point exC = new Point(innerC.X + gap, (int)(innerC.Y - gap *2.5F));

        //滚轮滚动一周的标准步数
        public static int WheelStdStepPerCircle = 24;
        //所有的形状
        public static Collection<DataStruct.ShowShapes> AllShapes = new Collection<DataStruct.ShowShapes>();
        
        public static Collection<myPoint> mouseDataL = new Collection<myPoint>();
        public static Collection<myPoint> mouseDataR = new Collection<myPoint>();
        public static EventWaitHandle waithandleL = new AutoResetEvent(false);
        public static EventWaitHandle waithandleR = new AutoResetEvent(false);
        public static Algorithm.MouseDataProcess LMouseProc = new MouseDataProcess();
        public static Algorithm.MouseDataProcess RMouseProc = new MouseDataProcess();
        public static DX3D wheel3d_left = new DX3D();
        public static DX3D wheel3d_right = new DX3D();
        public static DataStruct.WHEEL_COUNT wheel_count_L = new DataStruct.WHEEL_COUNT(0,0);
        public static DataStruct.WHEEL_COUNT wheel_count_R = new DataStruct.WHEEL_COUNT(0,0);
        public static DataStruct.MouseParam mpL = new DataStruct.MouseParam("");
        public static DataStruct.MouseParam mpR = new DataStruct.MouseParam("");
        public static int startLCirX = exCen.X - innerR - (exR - innerR) / 2;
        public static int startLCirY = exCen.Y;
        public static int startLTriX = exA.X + gap/2;
        public static int startLTriY = exA.Y - gap/2;

        public static int startRCirX = exCen.X - innerR - (exR - innerR) / 2;
        public static int startRCirY = exCen.Y;
        public static int startRTriX = exA.X + gap / 2;
        public static int startRTriY = exA.Y - gap / 2;

        public static Graphics gPicL = null;
        public static Graphics gPicR = null;
        public static Graphics gDrawL = null;
        public static Graphics gDrawR = null;

        public static  DataStruct.MouseBtnResult LMouseBtnResults = new DataStruct.MouseBtnResult();
        public static DataStruct.MouseBtnResult RMouseBtnResults = new DataStruct.MouseBtnResult();
        public static Dictionary<TestItem, bool> EnTestItem = new Dictionary<TestItem, bool>(); 
        public enum TestItem
        {
            EN_L_UP=0,
            EN_L_DOWN =1,
            EN_R_UP = 2,
            EN_R_DOWN = 3,
            EN_SF_UP = 4,
            EN_SF_DOWN = 5,
            EN_SB_UP = 6,
            EN_SB_DOWN = 7,
            EN_W_UP = 8,
            EN_W_DOWN = 9,
            EN_WHEEL_FORWARD = 10,
            EN_WHEEL_BACKWARD = 11,
            EN_CIRCLE = 12,
            EN_TRIANGLE = 13
        }

        public static string GenShapeName(DataStruct.ProductIndex product, DataStruct.TestType ttype)
        {
            return product.ToString() + ttype.ToString();
        }
        public static void InitAllShapes()
        {
            if (AllShapes != null)
            {
                foreach (DataStruct.ShowShapes shape in AllShapes)
                {
                    shape.shape.Dispose();
                }
                AllShapes.Clear();
                AllShapes = null;
            }
            if(AllShapes == null) AllShapes = new Collection<DataStruct.ShowShapes>();

            #region product1
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.L_BTN, true, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.L_BTN), LBtnC, LBtnCirR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.R_BTN, true, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.R_BTN), RBtnC, RBtnCirR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_BTN, true, new myRectangle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_BTN), wheelBtn, wheelBtnRecW, wheelBtnRecH)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.SIDE_FRONT_BTN, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.SIDE_FRONT_BTN), sideBtnC1, sideRectW1, sideRectH1, sideArrowW1, sideArrowH1, sideBtnOri)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.SIDE_BACK_BTN, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.SIDE_BACK_BTN), sideBtnC2, sideRectW2, sideRectH2, sideArrowW2, sideArrowH2, sideBtnOri)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_FORWARD, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_FORWARD), wheelArrowRectCT, wheelRectWT, wheelRectHT, wheelArrowWT, wheelArrowHT, wheelOriUp)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_BACKWARD, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_BACKWARD), wheelArrowRectCB, wheelRectWB, wheelRectHB, wheelArrowWB, wheelArrowHB, wheelOriDown)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_CIRCLE_EX, false, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_CIRCLE_EX), exCen, exR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_CIRCLE_INNER, false, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_CIRCLE_INNER), innerCen, innerR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_TRIANGLE_EX, false, new myTriangle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_TRIANGLE_EX), exA, exB, exC)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_TRIANGLE_INNER, false, new myTriangle(GenShapeName(DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.DRAW_TRIANGLE_INNER), innerA, innerB, innerC)));
            #endregion

            #region product2
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.L_BTN, true, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.L_BTN), LBtnC, LBtnCirR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.R_BTN, true, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.R_BTN), RBtnC, RBtnCirR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_BTN, true, new myRectangle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_BTN), wheelBtn, wheelBtnRecW, wheelBtnRecH)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.SIDE_FRONT_BTN, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.SIDE_FRONT_BTN), sideBtnC1, sideRectW1, sideRectH1, sideArrowW1, sideArrowH1, sideBtnOri)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.SIDE_BACK_BTN, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.SIDE_BACK_BTN), sideBtnC2, sideRectW2, sideRectH2, sideArrowW2, sideArrowH2, sideBtnOri)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_FORWARD, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_FORWARD), wheelArrowRectCT, wheelRectWT, wheelRectHT, wheelArrowWT, wheelArrowHT, wheelOriUp)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_BACKWARD, true, new myArrow(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_BACKWARD), wheelArrowRectCB, wheelRectWB, wheelRectHB, wheelArrowWB, wheelArrowHB, wheelOriDown)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_CIRCLE_EX, false, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_CIRCLE_EX), exCen, exR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_CIRCLE_INNER, false, new myCircle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_CIRCLE_INNER), innerCen, innerR)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_TRIANGLE_EX, false, new myTriangle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_TRIANGLE_EX), exA, exB, exC)));
            AllShapes.Add(new DataStruct.ShowShapes(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_TRIANGLE_INNER, false, new myTriangle(GenShapeName(DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.DRAW_TRIANGLE_INNER), innerA, innerB, innerC)));
            #endregion
        }
        public static void SetBtnItem(TestItem key, bool value)
        {
            ModifyDictValue(EnTestItem, key, value);
        }
        public static void ClearBtnItem()
        {
            int c = EnTestItem.Count;
            for (int i = 0; i < c; i++)
            {
                SetBtnItem((TestItem)i, false);
            }
        }
        public static void ClearBtnResult(ref DataStruct.MouseBtnResult result)
        {
            result.L_Btn.key_up = false;
            result.L_Btn.key_down = false;
            result.R_Btn.key_up = false;
            result.R_Btn.key_down = false;
            result.SF_Btn.key_up = false;
            result.SF_Btn.key_down = false;
            result.SB_Btn.key_up = false;
            result.SB_Btn.key_down = false;
            result.W_Btn.key_up = false;
            result.W_Btn.key_down = false;
        }

        public static void ModifyDictValue(Dictionary<TestItem, bool> dic, TestItem key, bool value)
        {
            int dicCount = dic.Keys.Count;
            TestItem[] strKey = new TestItem[dicCount];

            dic.Keys.CopyTo(strKey, 0);//支持.net2.0

            for (int i = 0; i < strKey.Length; i++)
            {
                if (dic.ContainsKey(key) && strKey[i] == key)
                {
                    dic[strKey[i]] = value;
                }
            }
        }
    }
}
