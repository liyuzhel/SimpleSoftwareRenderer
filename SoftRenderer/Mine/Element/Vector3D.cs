using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Mine.Element
{
    /// <summary>
    /// 行向量
    /// </summary>
    public struct Vector3D
    {

        public float x;
        public float y;
        public float z;
        public float w;

        public Vector3D(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            
        }

        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
        }


        public float Length
        {
            get
            {
                float length = x * x + y * y + z * z;
                return (float)System.Math.Sqrt(length);
            }
        }

        public void Nomalize()
        {
            float len = Length;

            if (len != 0)
            {
                float s = 1 / len;
                x *= s;
                y *= s;
                z *= s;
                w *= s;
            }
        }


        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            Vector3D v = new Vector3D();
            v.x = a.x - b.x;
            v.y = a.y - b.y;
            v.z = a.z - b.z;
            v.w = 0;

            return v;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            Vector3D v = new Vector3D();
            v.x = a.x + b.x;
            v.y = a.y + b.y;
            v.z = a.z + b.z;
            v.w = 0;

            return v;
        }

        
        /// <summary>
        /// 点乘， 就a 在b向量的投影
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Vector3D a, Vector3D b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector3D Cross(Vector3D a, Vector3D b)
        {

            Vector3D v = new Vector3D();
            v.x = a.y * b.z - a.z * b.y;
            v.y = a.z * b.x - a.x * b.z;
            v.z = a.x * b.y - a.y * b.x;
            return v;
        }

        public static Vector3D operator *(Vector3D lhs, Matrix4x4 rhs)
        {
            Vector3D v = new Vector3D();
            v.x = lhs.x * rhs[0, 0] + lhs.y * rhs[1, 0] + lhs.z * rhs[2, 0] + lhs.w * rhs[3, 0];
            v.y = lhs.x * rhs[0, 1] + lhs.y * rhs[1, 1] + lhs.z * rhs[2, 1] + lhs.w * rhs[3, 1];
            v.z = lhs.x * rhs[0, 2] + lhs.y * rhs[1, 2] + lhs.z * rhs[2, 2] + lhs.w * rhs[3, 2];
            v.w = lhs.x * rhs[0, 3] + lhs.y * rhs[1, 3] + lhs.z * rhs[2, 3] + lhs.w * rhs[3, 3];

            return v;
        }
    }

    public struct Point2D
    {
        public float x;
        public float y;
        public Point2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
