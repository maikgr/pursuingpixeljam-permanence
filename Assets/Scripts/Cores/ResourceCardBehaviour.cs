using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Extensions;

namespace Permanence.Scripts.Cores
{
    public abstract class ResourceCardBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ResourceSpawnArea resourceSpawnArea;
        [SerializeField]
        private float lootTime;
        [SerializeField]
        private List<GameObject> loots;
        [SerializeField]
        private LootingBarController loadingBar;
        private float timeUntilNextLoot;
        private bool isLooting;
        private float timeModifier = 1f;

        protected virtual void Awake() {
            timeUntilNextLoot = lootTime;
            loadingBar.gameObject.SetActive(false);
        }

        protected virtual void Update() {
            if (isLooting)
            {
                timeUntilNextLoot -= Time.deltaTime;
                loadingBar.SetPercentage(timeUntilNextLoot / (lootTime * timeModifier));
                if (timeUntilNextLoot <= 0)
                {
                    loadingBar.ResetBar();
                    SpawnLoot(loots);
                    timeUntilNextLoot = lootTime * timeModifier;
                }
            }
        }

        public virtual void StartUseResource(float timeModifier = 1f)
        {
            this.timeModifier = timeModifier;
            timeUntilNextLoot = lootTime * timeModifier;
            loadingBar.gameObject.SetActive(true);
            isLooting = true;
        }

        public virtual void StopUseResource()
        {
            isLooting = false;
            loadingBar.gameObject.SetActive(false);
        }
        
        protected void SpawnLoot(List<GameObject> loots) {
            var loot = loots.GetRandom();
            var spawnPoint = GetRandomSpawnPoint(resourceSpawnArea.MinLocalPoint, resourceSpawnArea.MaxLocalPoint);
            var lootObj = Instantiate(loot, spawnPoint, Quaternion.identity);
        }
        
        private Vector2 GetRandomSpawnPoint(Vector2 minPoint, Vector2 maxPoint)
        {
            var hasObject = true;
            Vector2 randomPos = Vector2.zero;
            while (hasObject)
            {
                randomPos = new Vector2(
                    UnityEngine.Random.Range(minPoint.x, maxPoint.x),
                    UnityEngine.Random.Range(minPoint.y, maxPoint.y)
                );
                var hitInfo = Physics2D.Raycast(randomPos, Vector2.zero);
                hasObject = hitInfo.collider != null;
            }
            return randomPos;
        }
    }
}