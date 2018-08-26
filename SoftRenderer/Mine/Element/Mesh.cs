using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer.Mine.Element
{
    class Mesh
    {
        public Vertex[] verts;

        
        public Mesh(Vector3D[] pointList, int[] indexs,Vector3D[] vectColors, Point2D[] uvs) 
        {
            verts = new Vertex[indexs.Length];
            for(int i = 0; i < indexs.Length; i++)
            {
                int pointIndex = indexs[i];
                var point = pointList[pointIndex];

                verts[i] = new Vertex(point,uvs[i].x,uvs[i].y, 
                    new Color(vectColors[i].x, vectColors[i].y, vectColors[i].z));
            }
        }

    }
}
