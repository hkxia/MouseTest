using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MouseTest
{
    public class DirectXWheel
    {
        #region  定义
        private Mesh mesh = null;
        private bool pause = false;
        private Device device = null;
        private Material meshmaterials;
        private Texture[] meshtexture = null;
        private Microsoft.DirectX.Direct3D.Font fonts = null;
        private Microsoft.DirectX.Direct3D.Material[] meshmaterials1;
        private String meshFilePath = @"MouseWheel.x";
        private String textureFilePath = @"p.jpg";

        private float angle = 0f;
        private float viewz = -100.0f;
        private int position_x = 0;  
        private int position_y = 0;
        private float pitch = 0f;
        private float yaw = 0f; 
        private float roll = 0f;
        private float carema_x = 0f;
        private float carema_y = 0f;
        #endregion
        #region properties

        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }
        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }
        public float Roll
        {
            get { return roll; }
            set { roll = value; }
        }
        public float carema_X
        {
            get { return carema_x; }
            set { carema_x = value; }
        }
        public float carema_Y
        {
            get { return carema_y; }
            set { carema_y = value; }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        public float viewZ
        {
            get { return viewz; }
            set { viewz = value; }
        }
        public int position_X
        {
            get { return position_x; }
            set { position_x = value; }
        }
        public int position_Y
        {
            get { return position_y; }
            set { position_y = value; }
        }

        #endregion

        public void LoadFile(String MeshFilePath, String TextureFilePath)
        {
            meshFilePath = MeshFilePath;
            textureFilePath = TextureFilePath;
        }
        public bool InitializeGraphics(System.Windows.Forms.Control mainForm, System.Windows.Forms.Control ctrl)
        {
            try
            {
                PresentParameters presentparams = new PresentParameters();
                presentparams.Windowed = true;
                presentparams.DeviceWindow = ctrl;
                presentparams.SwapEffect = SwapEffect.Discard;
                presentparams.EnableAutoDepthStencil = true;
                presentparams.AutoDepthStencilFormat = DepthFormat.D16;
                device = new Device(0, DeviceType.Hardware, mainForm.Handle, CreateFlags.SoftwareVertexProcessing, presentparams);
                // fonts = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font
                //("Arial", 14.0f, FontStyle.Bold | FontStyle.Italic));
                device.DeviceReset += new System.EventHandler(this.OnResetDevice);
                this.OnCreateDevice(device, null);
                this.OnResetDevice(device, null);
            }
            catch (DirectXException) { return false; }
            return true;
        }
        public void SetDefaultValue(float _yaw = 36, int _positionx = 10, int _positiony = -4, float _viewz = -99, float _pitch = 0)
        {
            yaw = _yaw * 3.14f / 60;
            position_X = _positionx;
            position_Y = _positiony;
            viewZ = _viewz;
            Pitch = _pitch;
        }
        public void OnCreateDevice(object sender, EventArgs e)
        {
            meshmaterials = new Material();
            //meshmaterials.Ambient = System.Drawing.Color.White;
            //meshmaterials.Diffuse = System.Drawing.Color.White;
            //ColorValue cv=new ColorValue(255,1,1);
            //meshmaterials.AmbientColor = cv;
            //meshmaterials.DiffuseColor = cv;
            ExtendedMaterial[] Materials = null;
            mesh = Mesh.FromFile(meshFilePath, MeshFlags.RtPatches, device, out Materials);//要载入的.x文件
            //mesh = Mesh.FromFile(@"..\..\car.x", MeshFlags.RtPatches, device, out Materials);//要载入的.x文件

            if (meshtexture == null)//没有网格纹理的话，载入？？
            {
                meshtexture = new Texture[Materials.Length];
                meshmaterials1 = new Microsoft.DirectX.Direct3D.Material[Materials.Length];
                for (int i = 0; i < Materials.Length; i++)
                {
                    meshmaterials1[i] = Materials[i].Material3D;
                    meshmaterials1[i].DiffuseColor = new ColorValue(224, 255, 255);
                    meshmaterials1[i].Ambient = meshmaterials1[i].Diffuse;
                    //meshtexture[i] = TextureLoader.FromFile(device, textureFilePath);//DDS files or jpg is OK.  这里的meshtexture 文件必须有，不然模型全黑乎乎的
                }
            }
        }
        public void OnResetDevice(object sender, EventArgs e)
        {
            Device dev = (Device)sender;
            //dev.RenderState.Ambient = Color.Blue;
            dev.RenderState.CullMode = Cull.None;
            dev.RenderState.Lighting = true;
            dev.RenderState.BlendFactor = Color.White;
            dev.RenderState.ColorVertex = true;
            dev.RenderState.FogColor = Color.Blue;

            dev.Lights[0].Type = LightType.Directional;
            dev.Lights[0].Diffuse = Color.SkyBlue;
            dev.Lights[0].Direction = new Vector3(0, -1, 1);//设置投光位置
            dev.Lights[0].Update();
            dev.Lights[0].Enabled = true;
            
            dev.Material = meshmaterials;
        }
        public void Render()
        {
            if (device == null) return;
            if (pause) return;
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, SystemColors.ControlLight, 10.0f, 0);
            SetupMatrices();
            device.BeginScene();
            for (int i = 0; i < meshmaterials1.Length; i++)
            {
                device.Material = meshmaterials1[i];
                device.SetTexture(0, meshtexture[i]);
                mesh.DrawSubset(i);
            }

            device.EndScene();
            device.Present();
        }
        public void SetForeward(int value)
        {
            Roll = value  * 3.14f / 60;
            Render();
        }
        public void SetBackward(int value)
        {
            Roll = (-1) * value * 3.14f / 60;
            Render();
        }

        private void SetupMatrices()
        {
            //float itime = System.Environment.TickCount%1000.0f;
            //angle = (float)(2 * Math.PI) * itime / 1000.0f;
            //device.Transform.World = Matrix.RotationY(angle);
            device.Transform.World = Matrix.RotationYawPitchRoll(yaw, pitch, roll);

            //一个摄像机矩阵可有由三个部分组成：摄像机位置、目标位置以及摄像机上下方
            device.Transform.View = Matrix.LookAtLH(new Vector3(carema_x, carema_y, viewz), new Vector3(-position_x * viewz / 200, -position_y * viewz / 200, 0), new Vector3(0, 5, 0));

            //下面的方法参考 http://www.cnblogs.com/markuya/articles/1517348.html
            /*函数名称可以翻译为投影变换，类似透视图
             * 其中参数fovy为y轴上的视角，Aspect为高宽比，zn为近裁面，zf为远裁面。
             * 远近裁面跟viewz相关，就是要处于这两个面之间才能正确显示
             * Y轴的视角：在DirectX的帮助文档中描述fovy为filed of view in y direction。
             * 
             * BV:其实fovy可以理解为焦距哇哈哈，望远镜倍数……
             * */
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 10, 1.0f, 1.0f, 5000.0f);//设置透视，最后面那个参数好像是最远能看到的距离

        }
    }
}
