using UnityEngine;

namespace Scripts.Utils
{
    public class GizmosUtils
    {
        public static void DrawBounds(Bounds bounds, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            
            Gizmos.DrawLine(bounds.min, new Vector3(bounds.min.x, bounds.max.y));   
            Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y), bounds.max);
            Gizmos.DrawLine(bounds.max, new Vector3(bounds.max.x, bounds.min.y));
            Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y), bounds.min);
            
            Gizmos.color = prevColor;
        }
        
        public static void DrawCapsule(Vector3 start, Vector3 end, float radius, CapsuleDirection2D direction)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = Color.red;

            if (direction == CapsuleDirection2D.Vertical)
            {
                var up = Vector3.up * radius;
                Gizmos.DrawWireSphere(start + up, radius);
                Gizmos.DrawWireSphere(end - up, radius);
                Gizmos.DrawLine(start + Vector3.right * radius, end + Vector3.right * radius);
                Gizmos.DrawLine(start - Vector3.right * radius, end - Vector3.right * radius);
            }
            else
            {
                var right = Vector3.right * radius;
                Gizmos.DrawWireSphere(start + right, radius);
                Gizmos.DrawWireSphere(end - right, radius);
                Gizmos.DrawLine(start + Vector3.up * radius, end + Vector3.up * radius);
                Gizmos.DrawLine(start - Vector3.up * radius, end - Vector3.up * radius);
            }

            Gizmos.color = prevColor;
        }
    }
}