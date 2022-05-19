using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Extensions;

namespace Permanence.Scripts.Cores
{
    public abstract class ResourceCardBehaviour : EventBusBehaviour<CardProgressBar>
    {
        [SerializeField]
        private ResourceSpawnArea resourceSpawnArea;
        [SerializeField]
        private float lootTime;
        [SerializeField]
        private List<GameObject> loots;
        private float timeUntilNextLoot;
        private bool isLooting;
        private float speedModifier = 1f;
        private CardProgressBar cardProgressBar;

        #pragma warning disable 0114
        protected virtual void Awake() {
            base.Awake();
            timeUntilNextLoot = lootTime;
            cardProgressBar = new CardProgressBar();
        }
        #pragma warning restore 0114

        protected virtual void Update() {
            if (isLooting)
            {
                timeUntilNextLoot -= Time.deltaTime * speedModifier;
                cardProgressBar.Value = timeUntilNextLoot/lootTime;
                DispatchEvent(CardProgressBarEvent.ON_LOOTING_PROGRESS, cardProgressBar);
                if (timeUntilNextLoot <= 0)
                {
                    SpawnLoot(loots);
                    timeUntilNextLoot = lootTime;
                }
            }
        }

        public virtual void StartUseResource(float speedModifier = 1f)
        {
            this.speedModifier = speedModifier;
            isLooting = true;
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_LOOTING_START, cardProgressBar);
        }

        public virtual void StopUseResource()
        {
            isLooting = false;
            cardProgressBar.IsShow = false;
            DispatchEvent(CardProgressBarEvent.ON_LOOTING_STOP, cardProgressBar);
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