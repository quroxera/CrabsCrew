using UnityEngine;

namespace Scripts.Utils
{
    public static class SpawnUtils
    {
        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = ContainerName)
        {
            var container = GameObject.Find(containerName);
            if (!container)
                container = new GameObject(containerName);

            return Object.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }
    }
}