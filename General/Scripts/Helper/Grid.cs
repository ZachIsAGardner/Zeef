using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Zanitacharin
{
    public class Grid : MonoBehaviour
    {
        public Color Color = new Color(1, 1, 1, 0.5f);
        public float Size = 1.28f;
        public Vector2 Amount = new Vector2(10, 10);

        void OnDrawGizmos() 
        {
            Vector3 size = Vector3.one * Size;
            Vector3 offset = Vector3.one * Size * Amount / 2;
            
            Gizmos.color = Color;
            
            int i = 0;
            while(i < Amount.x)
            {
                int j = 0;

                while(j < Amount.y)
                {
                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                    {
                        Gizmos.DrawCube(
                            transform.position + new Vector3(i * Size, j * Size, 0) - offset, 
                            size
                        );
                    }

                    j++;
                }

                i++;
            }
        }
    }
}

