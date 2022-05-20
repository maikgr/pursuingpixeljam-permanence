using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Cores
{
    public abstract class ResourceCardBehaviour : EventBusBehaviour<CardProgressBar>
    {
        [SerializeField]
        protected ResourceSpawnArea resourceSpawnArea;
        [SerializeField]
        protected float lootTime;
        [SerializeField]
        protected List<GameObject> loots;
        protected float timeUntilNextLoot;
        protected bool isLooting;
        protected float speedModifier = 1f;
        protected CardProgressBar cardProgressBar;

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
            var spawnPoint = resourceSpawnArea.GetRandomSpawnPoint(transform.position);
            var lootObj = Instantiate(loot, spawnPoint, Quaternion.identity);
            SfxController.instance.PlayAudio(GameSfxType.CardSpawn, transform.position);
        }
    }
}