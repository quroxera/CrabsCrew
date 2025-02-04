using System.Collections;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class EdgeColliderProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _projectileCount;
        [SerializeField] private float _delay;
        [SerializeField] private EdgeCollider2D _edgeCollider;

        public readonly int StageIndex = 2;
        
        [ContextMenu("Launch")]
        public void LaunchProjectile()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var points = _edgeCollider.points;
            if (points == null || points.Length < 2)
                yield break;
            
            var worldPoints = new Vector2[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                worldPoints[i] = _edgeCollider.transform.TransformPoint(points[i]);
            }
            
            var totalLength = CalculateEdgeLength(worldPoints);
            
            var step = totalLength / _projectileCount;

            for (var i = 0; i <= _projectileCount; i++)
            {
                var spawnDistance = step * i;
                var spawnPosition = GetPointAlongEdge(worldPoints, spawnDistance);
                
                SpawnUtils.Spawn(_prefab, spawnPosition);
                
                yield return new WaitForSeconds(_delay);
            }
        }

        private static float CalculateEdgeLength(Vector2[] points)
        {
            var length = 0f;
            for (var i = 1; i < points.Length; i++)
            {
                length += Vector2.Distance(points[i - 1], points[i]);
            }

            return length;
        }

        private static Vector2 GetPointAlongEdge(Vector2[] points, float distance)
        {
            var currentLength = 0f;

            for (var i = 1; i < points.Length; i++)
            {
                var segmentLength = Vector2.Distance(points[i - 1], points[i]);

                if (currentLength + segmentLength >= distance)
                {
                    var t = (distance - currentLength) / segmentLength;
                    return Vector2.Lerp(points[i - 1], points[i], t);
                }

                currentLength += segmentLength;
            }
            
            return points[points.Length - 1];
        }
    }
}