using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Mine.Element
{
    class MathUtil
    {

        /// <summary>
        /// 平移矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix4x4 GetTranslate(float x, float y, float z)
        {
            return new Matrix4x4(1, 0, 0, 0, 
                                     0, 1, 0, 0, 
                                     0, 0, 1, 0, 
                                     x, y, z, 1);
        }


        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix4x4 GetScale(float x, float y, float z)
        {
            return new Matrix4x4(x, 0, 0, 0, 
                                      0, y, 0, 0, 
                                      0, 0, z, 0, 
                                      0,0,0,1);
        }

        /// <summary>
        /// 按照Y轴旋转的矩阵
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix4x4 GetRotateY(float y)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.Identity();
            rm[0, 0] = (float)(System.Math.Cos(y));
            rm[0, 2] = (float)(-System.Math.Sin(y));


            rm[2, 0] = (float)(System.Math.Sin(y));
            rm[2, 2] = (float)(System.Math.Cos(y));

            return rm;
        }


        public static Matrix4x4 GetRotateX(float x)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.Identity();
            rm[1, 1] = (float)(System.Math.Cos(x));
            rm[1, 2] = (float)(System.Math.Sin(x));


            rm[2, 1] = (float)(-System.Math.Sin(x));
            rm[2, 2] = (float)(System.Math.Cos(x));
            return rm;
        }
        public static Matrix4x4 GetRotateZ(float z)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.Identity();

            rm[0, 0] = (float)(System.Math.Cos(z));
            rm[0, 1] = (float)(System.Math.Sin(z));
            //
            rm[1, 0] = (float)(-System.Math.Sin(z));
            rm[1, 1] = (float)(System.Math.Cos(z));

            return rm;
        }

        public static Matrix4x4 GetView(Vector3D pos, Vector3D lookAt, Vector3D up)
        {
            Vector3D dir = lookAt - pos;
            Vector3D right = Vector3D.Cross(up, dir);
            right.Nomalize();

            //平移部分
            Matrix4x4 t = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, -pos.x, -pos.y, -pos.z, 1);

            //旋转部分
            //TODO liyf 为什么是这个顺序
            Matrix4x4 r = new Matrix4x4(right.x, up.x, dir.x, 0, right.y, up.y, dir.y, 0, right.z, up.z, dir.z, 0, 0, 0, 0, 1);

            //TODO  为甚是平移矩阵乘上 旋转矩阵
            return t * r;

        }

        /// <summary>
        /// 裁剪矩阵，这是有公式，有定理的
        /// TODO 将来可以把这个用Shader入门精要里面的view矩阵试一下
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspect"></param>
        /// <param name="zn"></param>
        /// <param name="zf"></param>
        /// <returns></returns>
        public static Matrix4x4 GetProjection(float fov, float aspect, float zn, float zf)
        {
            Matrix4x4 p = new Matrix4x4();
            p.SetZero();

            p[0, 0] =(float) (1 / (System.Math.Tan(fov * 0.5f) * aspect));
            p[1, 1] = (float)(1 / (System.Math.Tan(fov * 0.5f) ));
            p[2, 2] = zf / (zf - zn);
            p[2, 3] = 1;
            p[3, 2] = (zn * zf) / (zn - zf);
            return p;
        }




        /// <summary>
        /// 线性差值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t)
        {
            if(t <= 0)
            {
                return a;
            }
            if(t >= 1)
            {
                return b;
            }
            return (b - a) * t + a;
        }


        /// <summary>
        /// 颜色插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Color Lerp(Color a, Color b, float t)
        {
            if (t <= 0)
            {
                return a;
            }
            else if (t >= 1)
            {
                return b;
            }
            else
            {
                return t * b + (1 - t) * a;
            }
        }
        public static int Range(int v, int min, int max)
        {
            if (v <= min)
            {
                return min;
            }
            if (v >= max)
            {
                return max;
            }
            return v;
        }

        public static float Range(float v, float min, float max)
        {
            if (v <= min)
            {
                return min;
            }
            if (v >= max)
            {
                return max;
            }
            return v;
        }

        public static void LerpVertex(ref Vertex v, Vertex v1, Vertex v2, float t)
        {

            v.onePerZ = Lerp(v1.onePerZ, v2.onePerZ, t);
            v.u = Lerp(v1.u, v2.u, t);
            v.v = Lerp(v1.v, v2.v, t);
            v.vColor = Lerp(v1.vColor, v2.vColor, t);
        }

    }
}
