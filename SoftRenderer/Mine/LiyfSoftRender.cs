using SoftRenderer.Mine.Element;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftRenderer.Mine
{
    public enum RenderMode
    {
        /// <summary>
        /// 线框模式
        /// </summary>
        WireFrame,
        /// <summary>
        /// 纹理模式
        /// </summary>
        Textured,

        /// <summary>
        /// 顶点着色
        /// </summary>
        VertexColor
    }

    /// <summary>
    /// 纹理过滤模式
    /// </summary>
    public enum TextureFilterMode
    {
        /// <summary>
        /// 点采样
        /// </summary>
        point,
        /// <summary>
        /// 双线性采样
        /// </summary>
        Bilinear
    }

    public partial class LiyfSoftRender : Form
    {

        // ----------------------------------

        static Vector3D[] pointList = {
                                            new Vector3D(-1,  1, -1),
                                            new Vector3D(-1, -1, -1),
                                            new Vector3D(1, -1, -1),
                                            new Vector3D(1, 1, -1),

                                            new Vector3D( -1,  1, 1),
                                            new Vector3D(-1, -1, 1),
                                            new Vector3D(1, -1, 1),
                                            new Vector3D(1, 1, 1)
                                        };
        //三角形顶点索引 12个面
         static int[] indexs = {   0,1,2,
                                   0,2,3,
                                   //
                                   7,6,5,
                                   7,5,4,
                                   //
                                   0,4,5,
                                   0,5,1,
                                   //
                                   1,5,6,
                                   1,6,2,
                                   //
                                   2,6,7,
                                   2,7,3,
                                   //
                                   3,7,4,
                                   3,4,0
                               };

        //uv坐标
        public static Point2D[] uvs ={
                                  new Point2D(0, 0),new Point2D( 0, 1),new Point2D(1, 1),
                                   new Point2D(0, 0),new Point2D(1, 1),new Point2D(1, 0),
                                   //
                                    new Point2D(0, 0),new Point2D( 0, 1),new Point2D(1, 1),
                                   new Point2D(0, 0),new Point2D(1, 1),new Point2D(1, 0),
                                   //
                                    new Point2D(0, 0),new Point2D( 0, 1),new Point2D(1, 1),
                                   new Point2D(0, 0),new Point2D(1, 1),new Point2D(1, 0),
                                   //
                                    new Point2D(0, 0),new Point2D( 0, 1),new Point2D(1, 1),
                                   new Point2D(0, 0),new Point2D(1, 1),new Point2D(1, 0),
                                   //
                                     new Point2D(0, 0),new Point2D( 0, 1),new Point2D(1, 1),
                                   new Point2D(0, 0),new Point2D(1, 1),new Point2D(1, 0),
                                   ///
                                     new Point2D(0, 0),new Point2D( 0, 1),new Point2D(1, 1),
                                   new Point2D(0, 0),new Point2D(1, 1),new Point2D(1, 0)
                              };

        //顶点色
        public static Vector3D[] vertColors = {
                                             new Vector3D( 0, 1, 0), new Vector3D( 0, 0, 1), new Vector3D( 1, 0, 0),
                                               new Vector3D( 0, 1, 0), new Vector3D( 1, 0, 0), new Vector3D( 0, 0, 1),
                                               //
                                                new Vector3D( 0, 1, 0), new Vector3D( 0, 0, 1), new Vector3D( 1, 0, 0),
                                               new Vector3D( 0, 1, 0), new Vector3D( 1, 0, 0), new Vector3D( 0, 0, 1),
                                               //
                                                new Vector3D( 0, 1, 0), new Vector3D( 0, 0, 1), new Vector3D( 1, 0, 0),
                                               new Vector3D( 0, 1, 0), new Vector3D( 1, 0, 0), new Vector3D( 0, 0, 1),
                                               //
                                                new Vector3D( 0, 1, 0), new Vector3D( 0, 0, 1), new Vector3D( 1, 0, 0),
                                               new Vector3D( 0, 1, 0), new Vector3D( 1, 0, 0), new Vector3D( 0, 0, 1),
                                               //
                                                new Vector3D( 0, 1, 0), new Vector3D( 0, 0, 1), new Vector3D( 1, 0, 0),
                                               new Vector3D( 0, 1, 0), new Vector3D( 1, 0, 0), new Vector3D( 0, 0, 1),
                                               //
                                                new Vector3D( 0, 1, 0), new Vector3D( 0, 0, 1), new Vector3D( 1, 0, 0),
                                               new Vector3D( 0, 1, 0), new Vector3D( 1, 0, 0), new Vector3D( 0, 0, 1)
                                         };
        //-----------------------------------



        Bitmap mFrameBuff; //帧缓冲
        Graphics mFrameGraphics;
        Camera mCamera; //摄像机

        Bitmap mTexture; //纹理

        float[,] mZBuff;//z缓冲，用来做深度测试

        RenderMode mRenderMode = RenderMode.WireFrame;

        TextureFilterMode mFilterMode = TextureFilterMode.point;

        Mesh mMesh;
        public LiyfSoftRender()
        {
            InitializeComponent();

            mFrameBuff = new Bitmap(this.MaximumSize.Width, this.MaximumSize.Height);
            mFrameGraphics = Graphics.FromImage(mFrameBuff);
            mZBuff = new float[this.MaximumSize.Width, this.MaximumSize.Height];
            mCamera = new Camera(new Vector3D(0, 0, 0, 1),
                new Vector3D(0, 0, 1, 1),
                new Vector3D(0, 1, 0, 1),
                (float)(System.Math.PI / 4f),(this.MaximumSize.Width / (float)this.MaximumSize.Height),
                1f,
                500f
                );

            Image imge = Image.FromFile("../../Texture/timg.jpg");
            mTexture = new Bitmap(imge, 256, 256);

            mFilterMode = TextureFilterMode.Bilinear;

            mMesh = new Mesh(pointList, indexs,vertColors, uvs);
            System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 30f);

            mainTimer.Elapsed += new System.Timers.ElapsedEventHandler(Tick);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();
        }

        float rot = 0;
        bool flag = false;

        void Tick(object sender, EventArgs e)
        {
            
            lock(mFrameBuff){


                Array.Clear(mZBuff, 0, mZBuff.Length);
                mFrameGraphics.Clear(System.Drawing.Color.Black);
                rot += 0.05f;
                Matrix4x4 m = MathUtil.GetRotateY(rot) * MathUtil.GetTranslate(0, 0, 10);
                Matrix4x4 v =  MathUtil.GetView(mCamera.pos, mCamera.lookAt, mCamera.up);
                Matrix4x4 p = MathUtil.GetProjection(mCamera.fov, mCamera.aspect, mCamera.nearClip, mCamera.farClip);

                mRenderMode = RenderMode.Textured;

                Draw(m, v, p);
                flag = true;
            }
            
        }


        
        Graphics g = null;
        void Draw(Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            //Vertex v1 = new Vertex(new Vector3D(0, 0, 0, 1));
            //Vertex v2 = new Vertex(new Vector3D(1, 0, 0, 1));
            //Vertex v3 = new Vertex(new Vector3D(0, 1, 0, 1));

            for (int i = 0; i + 2 < mMesh.verts.Length; i+=3)
            {
                DrawTriangle(mMesh.verts[i], mMesh.verts[i+1], mMesh.verts[i+2], m, v, p);
            }

        //    DrawTriangle(v1, v2, v3, m, v, p);

            if(null == g)
            {
                g = this.CreateGraphics();
            }
            g.Clear(System.Drawing.Color.Black);
            g.DrawImage(mFrameBuff, 0, 0);

        }


        void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {

            //变换到相机空间
            SetMVTransform(m, v, ref v1);
            SetMVTransform(m, v, ref v2);
            SetMVTransform(m, v, ref v3);

            if (BackFaceCulling(v1, v2, v3))
            {
                return;
            }

            SetProjectionTransform(p, ref v1);
            SetProjectionTransform(p, ref v2);
            SetProjectionTransform(p, ref v3);

            if (Clip(v1) == false || Clip(v2) == false || Clip(v3) == false)
            {
                return;
            }

            TransformToScreen(ref v1);
            TransformToScreen(ref v2);
            TransformToScreen(ref v3);

            if (mRenderMode == RenderMode.WireFrame){

                BresenhamDrawLine(v1.point, v2.point);
                BresenhamDrawLine(v2.point, v3.point);
                BresenhamDrawLine(v3.point, v1.point);
            }
            else
            {
                TriangleRasterization(v1, v2, v3);
            }
            
        }

        /// <summary>
        /// 
        /// https://www.cnblogs.com/pheye/archive/2010/08/14/1799803.html
        /// 这里有详细讲解布兰森 画线的算法
        /// 核心思想就是 
        /// 假如 dx 的长度大于 dy
        /// 该线的斜率就是 m = dy / dx
        /// 当x加1的时候，判断y值是多少，当x + 1，y值应该加上m， 但是y + m不大可能一定是整数
        /// 当 y+m 小于 y+0.5的时候，就取y值，当y +m > y +0.5的时候，就取y +1，就这样以此类推，单是也要
        /// 保留每次计算的误差，让误差累积，超过y+0.5之后，就会取y+1,然后再对误差进行-1的操作，但是因为这浮点类型的
        /// 计算又很耗费性能，一个整数的运算一个时钟周期就可以完成，但是一个浮点类型的运算，就可能需要上百个
        /// 运行周期，而且这个条件只是个判断条件，所以就想办法，这些条件里面的运算参数取整，全部再之前的误差
        /// 基础上乘上 dx,就可以满足了条件判断了
        
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        void BresenhamDrawLine(Vector3D p1, Vector3D p2)
        {

            int x = (int)(System.Math.Round(p1.x, MidpointRounding.AwayFromZero));
            int y = (int)(System.Math.Round(p1.y, MidpointRounding.AwayFromZero));

            int dx = (int)(System.Math.Round(p2.x - p1.x, MidpointRounding.AwayFromZero));
            int dy = (int)(System.Math.Round(p2.y - p1.y, MidpointRounding.AwayFromZero));

            int stepx = dx > 0 ? 1 : -1;
            int stepy = dy > 0 ? 1 : -1;
            dx = System.Math.Abs(dx);
            dy = System.Math.Abs(dy);

            int error = 0;
            if (dx > dy)
            {
                for(int i = 0; i <= dx; i++)
                {
                    mFrameBuff.SetPixel(x + i* stepx, y, System.Drawing.Color.White);
                    error = error + dy;
                    if(2 * error > dx)
                    {
                        y += stepy;
                        error -= dx;
                    }
                }

            }
            else
            {
                for(int i = 0; i <= dy; i++)
                {
                    mFrameBuff.SetPixel(x, y + i * stepy, System.Drawing.Color.White);
                    error = error + dx;
                    if(2 * error > dy)
                    {
                        x += stepx;
                        error -= dy;
                    }
                }
            }
        }


        void ScanLineFill(Vertex left, Vertex right, int y)
        {
            if(left.onePerZ == 0 || right.onePerZ == 0)
            {
                int a = 01;
            }

            float dx = right.point.x - left.point.x;


            for(float x = left.point.x; x <= right.point.x; x += 0.5f)
            {
                int xPos = (int)(x + 0.5f);
                float lerpFactor = 0;
                if(dx != 0)
                {
                    lerpFactor = (x - left.point.x) / dx;
                }

                float onPreZ = MathUtil.Lerp(left.onePerZ, right.onePerZ, lerpFactor);
                //进行深度测试
                //看来深度值都是
                if(onPreZ >= mZBuff[xPos, y])
                {
                    float w = 1 / onPreZ;
                    mZBuff[xPos, y] = onPreZ;

                    float u = MathUtil.Lerp(left.u, right.u, lerpFactor) * w * (mTexture.Width - 1);
                    float v = MathUtil.Lerp(left.v, right.v, lerpFactor) * w * (mTexture.Height - 1);

                    Element.Color vertColor = MathUtil.Lerp(left.vColor, right.vColor, lerpFactor) * w;

                    if(mRenderMode == RenderMode.VertexColor)
                    {
                        mFrameBuff.SetPixel(xPos, y, vertColor.TransFormToSystemColor());
                    }
                    else if(mRenderMode == RenderMode.Textured)
                    {

                        if(mFilterMode == TextureFilterMode.point)
                        {
                            //点采样
                            mFrameBuff.SetPixel(xPos, y, ReadTexture((int)u, (int)v));
                        }
                        else if(mFilterMode == TextureFilterMode.Bilinear)
                        {
                           float uIndex = (float)System.Math.Floor(u);
                           float vIndex = (float)System.Math.Floor(v);

                            float du = u - uIndex;
                            float dv = v - vIndex;

                            //至于为啥 是 uIndex是 1-du的权重， uIndex + 1是du的权重，这个东西可以
                            //互换,看哪个效果比较好
                            Element.Color color1 =
                                new Element.Color(ReadTexture((int)uIndex, (int)vIndex)) *
                                (1 - du) * (1 - dv);

                            Element.Color color2 =
                                new Element.Color(ReadTexture((int)uIndex + 1, (int)vIndex)) *
                                (du) * (1 - dv);
                            Element.Color color3 =
    new Element.Color(ReadTexture((int)uIndex + 1, (int)vIndex + 1)) *
    (du) * (dv);
                            Element.Color color4 =
new Element.Color(ReadTexture((int)uIndex, (int)vIndex + 1)) *
(1 - du) * (dv);

                            Element.Color color = color1 + color2 + color3 + color4;
                            mFrameBuff.SetPixel(xPos, y, color.TransFormToSystemColor());

                        }

                    }
                }
            }
        }

        System.Drawing.Color ReadTexture(int u, int v)
        {
            u = MathUtil.Range(u, 0, mTexture.Width - 1);
            v = MathUtil.Range(v, 0, mTexture.Height - 1);

            return mTexture.GetPixel(u, v);
        }


        /// <summary>
        /// 画平底三角形
        ///     p3
        ///     
        /// 
        /// p1       p2
        /// 
        /// dx 是左上角为0 点
        /// 此时计算 必须还是用float，如果用int值的话，就会错误
        /// </summary>
        void DrawTriangleBottom(Vertex v1, Vertex v2, Vertex v3 )
        {
            Vector3D p1 = v1.point;
            Vector3D p2 = v2.point;
            Vector3D p3 = v3.point;

            float y3 = p3.y;
            float y1  =p1.y;
            for(float y = y1; y <= y3; y+= 0.5f)
            {
                //左边的点
                float xl = ((y - y1) * (p3.x - p1.x) / (y3 - y1) + p1.x);
                //右边的点
                float xr = ((y - y1) * (p3.x - p2.x) / (y3 - y1) + p2.x);

                Vector3D vl = new Vector3D(xl, y, 0);
                Vector3D vr = new Vector3D(xr, y, 0);


                float dy = y - y1;
                float t = dy / (y3 - y1);

                Vertex vertLeft = new Vertex();
                vertLeft.point = vl;
                MathUtil.LerpVertex(ref vertLeft, v1, v3, t);

                Vertex vertRight = new Vertex();
                vertRight.point = vr;
                MathUtil.LerpVertex(ref vertRight, v2, v3, t);


                int yIndex = (int)(System.Math.Round(y, MidpointRounding.AwayFromZero));

                if (vertLeft.point.x < vertRight.point.x)
                {
                    ScanLineFill(vertLeft, vertRight, yIndex);
                }
                else
                {
                    ScanLineFill(vertRight, vertLeft, yIndex);
                }

            }
        }

        /// <summary>
        /// 画平顶三角形
        /// 
        /// p1     p2
        /// 
        /// 
        ///     p3
        /// 
        ///因为Dx 的坐标系是左上角的为0点，所以看着平顶 就变成平顶了
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        void DrawTriangleTop(Vertex v1, Vertex v2, Vertex v3)
        {
            Vector3D p1 = v1.point;
            Vector3D p2 = v2.point;
            Vector3D p3 = v3.point;

            float y3 = p3.y;
            float y1 = p1.y;
            for (float y = y3; y <= y1; y+=0.5f)
            {
                //左边的点
                float xl = ((y - y1) * (p3.x - p1.x) / (y3 - y1) + p1.x);
                //右边的点
                float xr = ((y - y1) * (p3.x - p2.x) / (y3 - y1) + p2.x);

                Vector3D vl = new Vector3D(xl, y, 0);
                Vector3D vr = new Vector3D(xr, y, 0);

                float dy = y - y3;
                float t = dy /(y1 - y3);

                Vertex vertLeft = new Vertex();
                vertLeft.point = vl;
                MathUtil.LerpVertex(ref vertLeft, v3, v1, t);

                Vertex vertRight = new Vertex();
                vertRight.point = vr;
                MathUtil.LerpVertex(ref vertRight, v3, v2, t);

                int yIndex = (int)(System.Math.Round(y, MidpointRounding.AwayFromZero));
                if (vertLeft.point.x < vertRight.point.x)
                {
                    ScanLineFill(vertLeft, vertRight, yIndex);
                }
                else
                {
                    ScanLineFill(vertRight, vertLeft, yIndex);
                }
            }

        }


        void TriangleRasterization(Vertex v1, Vertex v2, Vertex v3)
        {


            if (v1.point.y == v2.point.y)
            {
                if (v1.point.y < v3.point.y)
                {
                    //平底
                    DrawTriangleBottom(v1, v2, v3);
                }
                else
                {  
                    //平顶
                    DrawTriangleTop(v1, v2, v3);
                }
            }
            else if (v1.point.y == v3.point.y)
            {
                if (v1.point.y < v2.point.y)
                {
                    //平底
                    DrawTriangleBottom(v1, v3, v2);
                    
                }
                else
                {
                    //平顶
                    DrawTriangleTop(v1, v3, v2);
                }
            }
            else if (v2.point.y == v3.point.y)
            {
                if (v2.point.y < v1.point.y)
                {
                    //平底
                    DrawTriangleBottom(v2, v3, v1);
                }
                else
                {
                    //平顶
                    DrawTriangleTop(v2, v3, v1);
                }
            }
            else
            {

                //分割三角形
                    Vertex top;

                    Vertex bottom;
                    Vertex middle;
                    if (v1.point.y > v2.point.y && v2.point.y > v3.point.y)
                    {
                        top = v1;
                        middle = v2;
                        bottom = v3;
                    }
                    else if (v3.point.y > v2.point.y && v2.point.y > v1.point.y)
                    {
                        top = v3;
                        middle = v2;
                        bottom = v1;
                    }
                    else if (v2.point.y > v1.point.y && v1.point.y > v3.point.y)
                    {
                        top = v2;
                        middle = v1;
                        bottom = v3;
                    }
                    else if (v3.point.y > v1.point.y && v1.point.y > v2.point.y)
                    {
                        top = v3;
                        middle = v1;
                        bottom = v2;
                    }
                    else if (v1.point.y > v3.point.y && v3.point.y > v2.point.y)
                    {
                        top = v1;
                        middle = v3;
                        bottom = v2;
                    }
                    else if (v2.point.y > v3.point.y && v3.point.y > v1.point.y)
                    {
                        top = v2;
                        middle = v3;
                        bottom = v1;
                    }
                    else
                    {
                        //三点共线
                        return;
                    }

                //插值求中间点x
                float middlex = (middle.point.y - top.point.y) * (bottom.point.x - top.point.x) / (bottom.point.y - top.point.y) + top.point.x;

                Vector3D middleVec = new Vector3D(middlex, middle.point.y, middle.point.z, 1);
                Vertex middleVert = new Vertex();
                middleVert.point = middleVec;

                float dy = middle.point.y - bottom.point.y;
                float t = dy / (top.point.y - bottom.point.y);
                MathUtil.LerpVertex(ref middleVert, bottom, top, t);

                //平底
                DrawTriangleBottom(middleVert, middle, top);


                //平顶
                DrawTriangleTop(middleVert, middle, bottom);


            }


        }



        /// <summary>
        /// 将顶点变换到相机空间
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v"></param>
        /// <param name="vertex"></param>
        void SetMVTransform(Matrix4x4 m, Matrix4x4 v, ref Vertex vertex)
        {
            Matrix4x4 tmp = m * v;
            vertex.point = vertex.point * m * v;


        }
        void SetProjectionTransform(Matrix4x4 p, ref Vertex vert)
        {
            vert.point = vert.point * p;
            //https://blog.csdn.net/puppet_master/article/details/80317178
            //至于为什么要存储这个z值，是因为将来对顶点颜色与uv差值的时候，都需要根据深度进行差值，如果
            //如果不使用深度的话，会导致图像变形，除非三个顶点在同一个深度才会没有问题，至于为啥，有
            //现成的数学定理证明, w的值保存的是 顶点原生的深度值
            vert.onePerZ = 1 / vert.point.w;
           // vert.onePerZ = 1 ;
            vert.u *= vert.onePerZ;
            vert.v *= vert.onePerZ;
            vert.vColor *= vert.onePerZ;

            //如果有光照颜色的话，也需要进行z值的处理
        }

        /// <summary>
        /// NDC 裁剪
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        bool Clip(Vertex v)
        {
            //坐标的三维分量 必须在 -w 与 w之间
            if (v.point.x >= -v.point.w && v.point.x <= v.point.w &&
                v.point.y >= -v.point.w && v.point.y <= v.point.w &&
                v.point.z >= -v.point.w && v.point.z <= v.point.w)
            {
                return true;
            }

            return false;
        }

        void TransformToScreen(ref Vertex v)
        {
            if (v.point.w != 0)
            {
                //进行透视除法
                v.point.x /= v.point.w;
                v.point.y /= v.point.w;
                v.point.z /= v.point.w;
                v.point.w = 1;

                //转换到屏幕坐标系
                v.point.x = (v.point.x + 1) * 0.5f * this.MaximumSize.Width;
                v.point.y = (1 - v.point.y) * 0.5f * this.MaximumSize.Height;
            }
        }


        /// <summary>
        /// 背面剔除
        /// TODO 顶点的方向???
        /// 向量叉乘的交换律???
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        bool BackFaceCulling(Vertex v1, Vertex v2, Vertex v3)
        {
            if(mRenderMode == RenderMode.WireFrame)
            {
                return false;
            }

            Vector3D vec1 = v2.point - v1.point;
            Vector3D vec2 = v3.point - v2.point;

            Vector3D normal = Vector3D.Cross(vec1, vec2);

            //在相机空间中，相机的坐标点就是 （0 0 0）
            Vector3D viewDir = v1.point - new Vector3D(0, 0, 0);
            if (Vector3D.Dot(normal, viewDir) < 0)
            {
                return true;
            }

            return false;

        }

    }
}
