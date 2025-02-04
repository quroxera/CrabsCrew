using System;
using System.Collections;
using Scripts.Creatures.Weapons;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        private Coroutine _currentCoroutine;
        public int Stage { get; set; }

        [ContextMenu("Launch")]
        public void LaunchProjectile()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            if (Stage >= _settings.Length)
                yield break;

            var settings = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / settings.BurstCount;

            for (var i = 0; i < settings.BurstCount; i++)
            {
                for (var j = 0; j < settings.ItemsPerBurst; j++)
                {
                    var angle = sectorStep * i;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    
                    Vector3 spawnOffset = direction * (settings.SpawnSpacing * j);

                    var instance = SpawnUtils.Spawn(settings.Prefab, transform.position + spawnOffset);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(direction);
                }

                yield return new WaitForSeconds(settings.Delay);
            }
        }
            
        public void StopProjectile()
        {
            if (_currentCoroutine == null) 
                return;
            
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null; 
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;
        [SerializeField] private int _itemsPerBurst; 
        [SerializeField] private float _spawnSpacing;
        public GameObject Prefab => _prefab;
        public int BurstCount => _burstCount;
        public float Delay => _delay;
        public int ItemsPerBurst => _itemsPerBurst;
        public float SpawnSpacing => _spawnSpacing;
    }
}