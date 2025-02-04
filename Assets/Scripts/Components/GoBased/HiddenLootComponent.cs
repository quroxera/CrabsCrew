using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class HiddenLootComponent : MonoBehaviour
    {
        [System.Serializable]
        public class LootItem
        {
            public GameObject prefab;
            public int chance; 
        }

        [SerializeField] private Transform _target;
        [SerializeField] private int _totalCount;
        [SerializeField] private List<LootItem> _lootItems;

        public void DropLoot()
        {
            int spawnedCount = 0;

            while (spawnedCount < _totalCount)
            {
                LootItem selectedItem = SelectLootItem();

                if (selectedItem != null)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0.5f, 0f);
                    Instantiate(selectedItem.prefab, _target.position + randomOffset, Quaternion.identity);
                    spawnedCount++;
                }
            }
        }

        private LootItem SelectLootItem()
        {
            int totalChance = 0;

            foreach (LootItem lootItem in _lootItems)
            {
                totalChance += lootItem.chance;
            }

            int randomValue = Random.Range(0, totalChance);
            int currentChance = 0;

            foreach (LootItem lootItem in _lootItems)
            {
                currentChance += lootItem.chance;

                if (randomValue < currentChance)
                {
                    return lootItem;
                }
            }
            return null;
        }
    }
}