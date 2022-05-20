using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Permanence.Scripts.Entities;

namespace Permanence.Scripts.Mechanics
{
    public class EvertreeMaterialConsumerCard : MaterialConsumerCard
    {
        [SerializeField]
        private ResourceSpawnArea resourceSpawnArea;
        [SerializeField]
        private float workTime;
        [SerializeField]
        private List<GameObject> rewards;
        private float timeLeft;
        private bool isWorking;
        private CardProgressBar cardProgressBar;

        protected override void Awake() {
            base.Awake();
            cardProgressBar = new CardProgressBar();
        }
        
        protected virtual void Update() {
            if (isWorking)
            {
                timeLeft -= Time.deltaTime;
                cardProgressBar.Value = timeLeft/workTime;
                DispatchEvent(CardProgressBarEvent.ON_LOOTING_PROGRESS, cardProgressBar);
                if (timeLeft <= 0)
                {
                    isWorking = false;
                    SpawnLoot(rewards);
                }
            }
        }

        public override bool SubmitMaterial()
        {
            if (requiredMaterials.Any(mat => !mat.isFulfilled)) return false;
            requiredMaterials.Clear();
            isWorking = true;
            cardProgressBar.IsShow = true;
            timeLeft = workTime;
            DispatchEvent(CardProgressBarEvent.ON_LOOTING_START, cardProgressBar);
            return true;
        }

        protected void SpawnLoot(List<GameObject> loots) {
            cardProgressBar.IsShow = false;
            DispatchEvent(CardProgressBarEvent.ON_LOOTING_STOP, cardProgressBar);
            foreach(var loot in loots)
            {
                var spawnPoint = resourceSpawnArea.GetRandomSpawnPoint(transform.position);
                Instantiate(loot, spawnPoint, Quaternion.identity);
            }
        }
    }
}