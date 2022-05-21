using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using System;
using System.Collections;

namespace Permanence.Scripts.Mechanics
{
    public class EvertreeBudController : ResourceCardBehaviour
    {
        private void Start() {
            StartCoroutine(DelayWorkStart(5f));
        }

        protected override void Update() {
            if (isLooting) {
                timeUntilNextLoot -= Time.deltaTime * speedModifier;
                cardProgressBar.Value = timeUntilNextLoot/lootTime;
                DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
                if (timeUntilNextLoot <= 0)
                {
                    SpawnLoot(loots);
                    timeUntilNextLoot = lootTime;
                }
            }
        }

        public override void StartUseResource(float speedModifier = 1f)
        {
            this.speedModifier = speedModifier * 5;
        }

        public override void StopUseResource()
        {
            this.speedModifier = speedModifier / 5;
        }

        private IEnumerator DelayWorkStart(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isLooting = true;
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_START, cardProgressBar);
        }
    }
}