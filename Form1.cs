using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Data;
using GDI2D;
using X3D;
using Algorithm;

namespace MouseTest
{
    public partial class frmMain : Form
    {
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);
        [DllImport("User32")]
        public extern static bool GetCursorPos(ref Point lpPoint);

        public frmMain()
        {
            InitializeComponent();
            Init();
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {

        }
        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            data.wheel3d_left.Dispose();
            data.wheel3d_right.Dispose();
        }
        private void FormPaint(object sender, PaintEventArgs e)
        {

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            if (object.ReferenceEquals(sender,btnStart))
            {
                StringBuilder vidpid = new StringBuilder("");
                //UsbMonitorApi.MonitorLtdusbDevices(vidpid, 0x413C, 0x301A, txtUSBCodeLeft.Handle, txtUSBCodeRight.Handle);
                //UsbMonitorApi.MonitorLtdusbDevices(vidpid, 0x04D9, 0xA156, txtUSBCodeLeft.Handle, txtUSBCodeRight.Handle);
                UsbMonitorApi.MonitorLtdusbDevices(vidpid, 0x1BCF, 0x0053, txtUSBCodeLeft.Handle, txtUSBCodeRight.Handle);
            }
            else if (object.ReferenceEquals(sender, btnWheelShow))
            {
                data.wheel3d_left.InitializeGraphics(this, pnLeftWheel);
                data.wheel3d_left.SetDefaultValue();
                data.wheel3d_left.Render();

                data.wheel3d_right.InitializeGraphics(this, pnRightWheel);
                data.wheel3d_right.SetDefaultValue();
                data.wheel3d_right.Render();
            }
        }
        private void TextChange(object sender, EventArgs e)
        {
            if (object.ReferenceEquals(sender, txtUSBCodeLeft))
            {
                data.mpL.SetCode(txtUSBCodeLeft.Text);
                data.mpL.X_BYTE_IDX = 2;
                data.mpL.Y_BYTE_IDX = 6;
                data.mpL.WHEEL_BYTE_IDX = 10;
                DataStruct.MouseTestResultData mtrd = data.LMouseProc.Process(data.mpL);
                myNotify.myNotify.OnNotify(mtrd);
            }
            else if (object.ReferenceEquals(sender, txtUSBCodeRight))
            {
                //data.mpR.SetCode(txtUSBCodeRight.Text);
                //DataStruct.MouseTestResultData mtrd = data.RMouseProc.Process(data.mpR);
                //myNotify.myNotify.OnNotify2(mtrd);
                OutputInfo(txtInfo, txtUSBCodeRight.Text);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (data.gPicL == null) { data.gPicL = picLeft.CreateGraphics(); }
            if (data.gDrawL == null) { data.gDrawL = labLeft.CreateGraphics(); }
            if (data.gPicR == null) { data.gPicR = picRight.CreateGraphics(); }
            if (data.gDrawR == null) { data.gDrawR = labRight.CreateGraphics(); }

            Result.Init2DShowResult(data.gPicL, data.gDrawL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestResult.STDBY);
            Result.Init2DShowResult(data.gPicR, data.gDrawR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestResult.STDBY);

            Result.InitShowResult(tlpLMouseResult1.Controls, DataStruct.myColor.STDBY);
            Result.InitShowResult(tlpLMouseResult2.Controls, DataStruct.myColor.STDBY);
            Result.InitShowResult(tlpRMouseResult1.Controls, DataStruct.myColor.STDBY);
            Result.InitShowResult(tlpRMouseResult2.Controls, DataStruct.myColor.STDBY);

        }
        private new void MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void Init()
        {
            myNotify.myNotify.NotifyMouseData += new EventHandler<DataStruct.MouseDataEventArgs>(NotifyMouseStatus);
            myNotify.myNotify.NotifyMouseData2 += new EventHandler<DataStruct.MouseDataEventArgs>(NotifyMouseStatus2);
            this.MaximizeBox = true;
            this.WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);     // 禁止擦除背景.
            //SetStyle(ControlStyles.DoubleBuffer, true);           // 双缓冲
            data.wheel3d_left.LoadFile(@"MouseWheel.x", @"p.jpg");
            data.wheel3d_right.LoadFile(@"MouseWheel.x", @"p.jpg");
            foreach (data.TestItem item in Enum.GetValues(typeof(data.TestItem)))
            {
                data.EnTestItem.Add(item, false);
            }
            data.InitAllShapes();
            bgwModuleL.RunWorkerAsync();
            bgwModuleR.RunWorkerAsync();
        }
        private void NotifyMouseStatus(object sender, DataStruct.MouseDataEventArgs e)
        {
            if (data.gPicL == null) { data.gPicL = picLeft.CreateGraphics(); }
            if (data.gDrawL == null){data.gDrawL = labLeft.CreateGraphics();}

            DataProc.BtnStatus(e.data, data.EnTestItem, ref data.LMouseBtnResults);
            DataProc.WheelStatus(e.data, data.EnTestItem, ref data.wheel_count_L);
            DataProc.WheelStatus3DStep(e.data, data.EnTestItem, data.wheel_count_L, data.wheel3d_left);
            DataProc.CircleTest(e.data, data.EnTestItem, ref data.startLCirX, ref data.startLCirY, data.gDrawL, DataStruct.ProductIndex.PRODUCT_1);
            DataProc.TriTest(e.data, data.EnTestItem, ref data.startLTriX, ref data.startLTriY, data.gDrawL, DataStruct.ProductIndex.PRODUCT_1);

            data.waithandleL.Set();
        }
        private void NotifyMouseStatus2(object sender, DataStruct.MouseDataEventArgs e)
        {
            if (data.gPicR == null) { data.gPicR = picRight.CreateGraphics(); }
            if (data.gDrawR == null) { data.gDrawR = labRight.CreateGraphics(); }

            DataProc.BtnStatus(e.data, data.EnTestItem, ref data.RMouseBtnResults);
            DataProc.WheelStatus(e.data, data.EnTestItem, ref data.wheel_count_R);
            DataProc.WheelStatus3DStep(e.data, data.EnTestItem, data.wheel_count_R, data.wheel3d_right);
            DataProc.CircleTest(e.data, data.EnTestItem, ref data.startRCirX, ref data.startRCirY, data.gDrawR, DataStruct.ProductIndex.PRODUCT_2);
            DataProc.TriTest(e.data, data.EnTestItem, ref data.startRTriX, ref data.startRTriY, data.gDrawR, DataStruct.ProductIndex.PRODUCT_2);

            data.waithandleR.Set();
        }
        private void DrawShape(object sender, DataStruct.DrawShapeEventsArgs e)
        {
            if (!labLeft.InvokeRequired)
            {
                bool cirRes = false;
                bool triRes = false;
                Type t = e.exShape.GetType();
                if (t.ToString() == "GDI2D.myCircle")
                {
                    cirRes = PointAlgorithm.IsPointInTwoCircle(e.currPoint, (myCircle)e.exShape, (myCircle)e.innerShape);
                }
                else if (t.ToString() == "GDI2D.myTriangle")
                {
                    triRes = PointAlgorithm.IsPointInTwoTriangle(e.currPoint.GetPoint(), data.exA, data.exB, data.exC, data.innerA, data.innerB, data.innerC);
                }

                foreach (var p in data.mouseDataL)
                {
                    if (cirRes || (triRes))
                    {
                        p.Draw(e.graphics, DataStruct.myColor.PASS, DataStruct.myColor.PASS, true, 1);
                    }
                    else
                    {
                        p.Draw(e.graphics, DataStruct.myColor.FAIL, DataStruct.myColor.FAIL, true, 1);
                    }
                }
            }
            else
            {
                Action<object, DataStruct.DrawShapeEventsArgs> del = DrawShape;
                labLeft.Invoke(del, sender, e);
            }
        }
        private void ShowProgress()
        {

        }
        private static Point GetClientOnParent(Control c)
        {
            var retval = new Point(0, 0);
            for (; c.Parent != null; c = c.Parent)
            {
                retval.Offset(c.Location);
            }
            return retval;
        }
        private void bgwModuleLDoWork1(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                data.waithandleL.WaitOne();
                //左按键
                if (data.LMouseBtnResults.L_Btn.key_up && data.LMouseBtnResults.L_Btn.key_down)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.L_BTN,DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLLBtnResult, DataStruct.myColor.PASS);
                }
                //右按键
                if (data.LMouseBtnResults.R_Btn.key_up && data.LMouseBtnResults.R_Btn.key_down)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.R_BTN,DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLRBtnResult, DataStruct.myColor.PASS);
                }
                //滚轮按键
                if (data.LMouseBtnResults.W_Btn.key_up && data.LMouseBtnResults.W_Btn.key_down)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLWBtnResult, DataStruct.myColor.PASS); 
                }
                //侧前按键
                if (data.LMouseBtnResults.SF_Btn.key_up && data.LMouseBtnResults.SF_Btn.key_down)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.SIDE_FRONT_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLSFBtnResult, DataStruct.myColor.PASS); 
                }
                //侧后按键
                if (data.LMouseBtnResults.SB_Btn.key_up && data.LMouseBtnResults.SB_Btn.key_down)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.SIDE_BACK_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLSBBtnResult, DataStruct.myColor.PASS);
                }
                //滚轮前滚动
                if (data.wheel_count_L.ForwardCount >= data.WheelStdStepPerCircle)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_FORWARD, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLWForwardRollResult, DataStruct.myColor.PASS);
                }
                //滚轮后滚动
                if (data.wheel_count_L.BackwardCount >= data.WheelStdStepPerCircle)
                {
                    Result.ShowResult(data.gPicL, DataStruct.ProductIndex.PRODUCT_1, DataStruct.TestType.WHEEL_BACKWARD, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtLWBackwardRollResult, DataStruct.myColor.PASS);
                }
            }
        }
        private void bgwModuleLProgressChanged1(object sender, ProgressChangedEventArgs e)
        {
            this.labLeft.Update();
        }
        private void bgwRunWorkerCompleted1(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        private void bgwModuleLDoWork2(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                data.waithandleR.WaitOne();
                //左按键
                if (data.RMouseBtnResults.L_Btn.key_up && data.RMouseBtnResults.L_Btn.key_down)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.L_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRLBtnResult, DataStruct.myColor.PASS);
                }
                //右按键
                if (data.RMouseBtnResults.R_Btn.key_up && data.RMouseBtnResults.R_Btn.key_down)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.R_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRRBtnResult, DataStruct.myColor.PASS);
                }
                //滚轮按键
                if (data.RMouseBtnResults.W_Btn.key_up && data.RMouseBtnResults.W_Btn.key_down)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRWBtnResult, DataStruct.myColor.PASS);
                }
                //侧前按键
                if (data.RMouseBtnResults.SF_Btn.key_up && data.RMouseBtnResults.SF_Btn.key_down)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.SIDE_FRONT_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRSFBtnResult, DataStruct.myColor.PASS);
                }
                //侧后按键
                if (data.RMouseBtnResults.SB_Btn.key_up && data.RMouseBtnResults.SB_Btn.key_down)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.SIDE_BACK_BTN, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRSBBtnResult, DataStruct.myColor.PASS);
                }
                //滚轮前滚动
                if (data.wheel_count_R.ForwardCount >= data.WheelStdStepPerCircle)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_FORWARD, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRWForwardRollResult, DataStruct.myColor.PASS);
                }
                //滚轮后滚动
                if (data.wheel_count_R.BackwardCount >= data.WheelStdStepPerCircle)
                {
                    Result.ShowResult(data.gPicR, DataStruct.ProductIndex.PRODUCT_2, DataStruct.TestType.WHEEL_BACKWARD, DataStruct.TestResult.PASSED);
                    SetControlProperty(txtRWBackwardRollResult, DataStruct.myColor.PASS);
                }
            }
        }
        private void bgwModuleLProgressChanged2(object sender, ProgressChangedEventArgs e)
        {
            this.labRight.Update();
        }
        private void bgwRunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void OutputInfo(string info)
        {
            OutputInfo(txtUSBCodeRight, info);
        }
        private void OutputInfo(TextBox txtInfo, string info)
        {
            if (string.IsNullOrEmpty(info)) return;

            if (txtInfo.InvokeRequired)
            {
                Action<TextBox, string> d = OutputInfo;
                txtInfo.BeginInvoke(d, new object[] { txtInfo, info });
            }
            else
            {
                if (txtInfo.MaxLength < 32767 && !txtInfo.Visible) return;
                txtInfo.AppendText(info);
                txtInfo.AppendText("\r\n");
                //txtInfo.Text += info;
                //txtInfo.Text += "\r\n";
            }
        }
        private void SetControlProperty(TextBox txtCtrl, Color c)
        {
            if (txtCtrl.InvokeRequired)
            {
                Action<TextBox, Color> d = SetControlProperty;
                txtCtrl.BeginInvoke(d, new object[] {txtCtrl, c});
            }
            else
            {
                txtCtrl.BackColor = c;
            }
        }
        private void CheckedChanged(object sender, EventArgs e)
        {
            data.SetBtnItem(data.TestItem.EN_L_DOWN, chkLBtnDown.Checked);
            data.SetBtnItem(data.TestItem.EN_L_UP, chkLBtnUP.Checked);
            data.SetBtnItem(data.TestItem.EN_R_UP, chkRBtnUP.Checked);
            data.SetBtnItem(data.TestItem.EN_R_DOWN, chkRBtnDown.Checked);

            data.SetBtnItem(data.TestItem.EN_W_UP, chkWBtnUp.Checked);
            data.SetBtnItem(data.TestItem.EN_W_DOWN, chkWBtnDown.Checked);

            data.SetBtnItem(data.TestItem.EN_WHEEL_FORWARD, chkWForward.Checked);
            data.SetBtnItem(data.TestItem.EN_WHEEL_BACKWARD, chkWBackward.Checked);

            data.SetBtnItem(data.TestItem.EN_CIRCLE, chkEnCirTest.Checked);
            data.SetBtnItem(data.TestItem.EN_TRIANGLE, chkEnTriTest.Checked);

            data.SetBtnItem(data.TestItem.EN_SF_UP, chkSF_UP.Checked);
            data.SetBtnItem(data.TestItem.EN_SF_DOWN, chkSF_DOWN.Checked);

            data.SetBtnItem(data.TestItem.EN_SB_UP, chkSB_UP.Checked);
            data.SetBtnItem(data.TestItem.EN_SB_DOWN, chkSB_DOWN.Checked);
            txtInfo.Clear();
            if (chkClear.Checked)
            {
                data.ClearBtnResult(ref data.LMouseBtnResults);
                
            }
             
        }
    }
}
