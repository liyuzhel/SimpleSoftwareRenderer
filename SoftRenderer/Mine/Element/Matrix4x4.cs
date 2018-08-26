using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Mine.Element
{
    public class Matrix4x4
    {
        float[,] _m = new float[4, 4];


        public Matrix4x4()
        {

        }

        public Matrix4x4(float a1, float a2, float a3, float a4,
            float b1, float b2, float b3 , float b4,
            float c1, float c2, float c3, float c4,
            float d1, float d2, float d3, float d4)
        {
            _m[0, 0] = a1; _m[0, 1] = a2; _m[0, 2] = a3; _m[0, 3] = a4;
            _m[1, 0] = b1; _m[1, 1] = b2; _m[1, 2] = b3; _m[1, 3] = b4;
            _m[2, 0] = c1; _m[2, 1] = c2; _m[2, 2] = c3; _m[2, 3] = c4;
            _m[3, 0] = d1; _m[3, 1] = d2; _m[3, 2] = d3; _m[3, 3] = d4;
        }


        public void SetZero()
        {
            _m[0, 0] = 0; _m[0, 1] = 0; _m[0, 2] = 0; _m[0, 3] = 0;
            _m[1, 0] = 0; _m[1, 1] = 0; _m[1, 2] = 0; _m[1, 3] = 0;
            _m[2, 0] = 0; _m[2, 1] = 0; _m[2, 2] = 0; _m[2, 3] = 0;
            _m[3, 0] = 0; _m[3, 1] = 0; _m[3, 2] = 0; _m[3, 3] = 0;
        }


        public float this[int i, int j]
        {
            get
            {
                return _m[i, j];
            }

            set
            {
                _m[i, j] = value;
            }
        }


        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            Matrix4x4 rm = new Matrix4x4();
            rm.SetZero();

            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    for(int k = 0; k < 4; k++)
                    {
                        rm._m[i, j] += a._m[i, k] * b._m[k, j];
                    }
                }
            }
            return rm;
        }


        public void Identity()
        {
            _m[0, 0] = 1; _m[0, 1] = 0; _m[0, 2] = 0; _m[0, 3] = 0;
            _m[1, 0] = 0; _m[1, 1] = 1; _m[1, 2] = 0; _m[1, 3] = 0;
            _m[2, 0] = 0; _m[2, 1] = 0; _m[2, 2] = 1; _m[2, 3] = 0;
            _m[3, 0] = 0; _m[3, 1] = 0; _m[3, 2] = 0; _m[3, 3] = 1;
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <returns></returns>
        public void Transpose()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = i; j < 4; j++)
                {
                    float temp = _m[i, j];
                    _m[i, j] = _m[j, i];
                    _m[j, i] = temp;
                }
            }
        }

        /// <summary>
        /// 求行列式
        /// 3d 数学基础里面 有一个公式
        /// 这就是按照那个公式进行运算的
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private float Determinate(float[,] m, int n)
        {
            if (n == 1)
            {//递归出口
                return m[0, 0];
            }
            else
            {
                float result = 0;
                float[,] tempM = new float[n - 1, n - 1];
                for (int i = 0; i < n; i++)
                {
                    //求代数余子式
                    for (int j = 0; j < n - 1; j++)//行
                    {
                        for (int k = 0; k < n - 1; k++)//列
                        {
                            int x = j + 1;//原矩阵行
                            int y = k >= i ? k + 1 : k;//原矩阵列
                            tempM[j, k] = m[x, y];
                        }
                    }

                    result += (float)System.Math.Pow(-1, 1 + (1 + i)) * m[0, i] * Determinate(tempM, n - 1);
                }
                return result;
            }
        }


        /// <summary>
        /// 获取伴随矩阵
        /// 伴随矩阵就是，原矩阵的代数余子式矩阵的转置
        /// </summary>
        /// <returns></returns>
        public Matrix4x4 GetAdJoint()
        {
            int x, y;
            float[,] tempM = new float[3, 3];
            Matrix4x4 result = new Matrix4x4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int t = 0; t < 3; ++t)
                        {
                            x = k >= i ? k + 1 : k;
                            y = t >= j ? t + 1 : t;

                            tempM[k, t] = _m[x, y];
                        }
                    }
                    result._m[i, j] = (float)System.Math.Pow(-1, (1 + j) + (1 + i)) * Determinate(tempM, 3);
                }
            }
            result.Transpose();
            return result;
        }


    }

}
