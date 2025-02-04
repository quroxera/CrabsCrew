using System;
using System.Linq;
using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);

            spawner?.Component.Spawn();
        }

        public void Spawn(string id, float timeToDestroy)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);

            spawner?.Component.Spawn(timeToDestroy, gameObject);
        }

        public Vector3 GetSpawnPosition(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);

            return spawner == null ? Vector3.zero : spawner.Component.transform.position;
        }

        public void SetSpawnPosition(string id, Vector3 newPos)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);

            if (spawner == null)
                return;

            spawner.Component.transform.position = newPos;
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}