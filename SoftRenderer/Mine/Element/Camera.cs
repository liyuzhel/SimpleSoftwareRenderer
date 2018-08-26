using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Mine.Element
{
    class Camera
    {

        public Vector3D pos;
        public Vector3D lookAt;
        public Vector3D up;

        /// <summary>
        /// 视锥体的弧度
        /// </summary>
        public float fov;
        
        /// <summary>
        /// 长宽比
        /// </summary>
        public float aspect;
        
        /// <summary>
        /// 近裁剪面
        /// </summary>
        public float nearClip;

        /// <summary>
        /// 远裁剪面
        /// </summary>
        public float farClip;


        public Camera(Vector3D pos, Vector3D lookAt, Vector3D up, 
            float fov, float aspect, float nearClip, float farClip)
        {
            this.pos = pos;
            this.lookAt = lookAt;
            this.up = up;
            this.fov = fov;
            this.aspect = aspect;
            this.nearClip = nearClip;
            this.farClip = farClip;
        }


    }
}
