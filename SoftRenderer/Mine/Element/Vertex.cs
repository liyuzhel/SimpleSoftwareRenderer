﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Mine.Element
{

    /// <summary>
    /// 颜色值(只存储0-1)
    /// </summary>
    public struct Color
    {
        private float _r;
        private float _b;
        private float _g;

        public float r
        {
            get { return MathUtil.Range(_r, 0, 1); }
            set { _r = MathUtil.Range(value, 0, 1); }
        }

        public float g
        {
            get { return MathUtil.Range(_g, 0, 1); }
            set { _g = value; }
        }

        public float b
        {
            get { return MathUtil.Range(_b, 0, 1); }
            set { _b = value; }
        }

        public Color(float r, float g, float b)
        {
            this._r = MathUtil.Range(r, 0, 1);
            this._g = MathUtil.Range(g, 0, 1);
            this._b = MathUtil.Range(b, 0, 1);
        }

        public Color(System.Drawing.Color c)
        {
            this._r = MathUtil.Range((float)c.R / 255, 0, 1);
            this._g = MathUtil.Range((float)c.G / 255, 0, 1);
            this._b = MathUtil.Range((float)c.B / 255, 0, 1);
        }
        /// <summary>
        /// 转换为系统的color
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color TransFormToSystemColor()
        {
            float r = this.r * 255;
            float g = this.g * 255;
            float b = this.b * 255;
            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
        }
        /// <summary>
        /// 颜色乘法，用于颜色混合，实际叫做Modulate（调制）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color operator *(Color a, Color b)
        {
            Color c = new Color();
            c.r = a.r * b.r;
            c.g = a.g * b.g;
            c.b = a.b * b.b;
            return c;
        }

        public static Color operator *(float a, Color b)
        {
            Color c = new Color();
            c.r = a * b.r;
            c.g = a * b.g;
            c.b = a * b.b;
            return c;
        }
        public static Color operator *(Color a, float b)
        {
            Color c = new Color();
            c.r = a.r * b;
            c.g = a.g * b;
            c.b = a.b * b;
            return c;
        }

        public static Color operator +(Color a, Color b)
        {
            Color c = new Color();
            c.r = a.r + b.r;
            c.g = a.g + b.g;
            c.b = a.b + b.b;
            return c;
        }
    }

    struct Vertex
    {
        public Vector3D point;

        public Vertex(Vector3D vec,float u, float v, Color color)
        {
            this.point = vec;
            this.u = u;
            this.v = v;
            this.onePerZ = 1;
            this.vColor = color;
        }

        public Color vColor;

        public float u;
        public float v;

        public float onePerZ;

    }
}
