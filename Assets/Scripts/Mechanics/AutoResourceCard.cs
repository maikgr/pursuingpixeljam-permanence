using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using System;
using System.Collections;

namespace Permanence.Scripts.Mechanics
{
    public class AutoResourceCard : ResourceCardBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DelayLootStart(1f);
        }

        protected override void Update() {
            timeUntilNextLoot += Time.deltaTime;
            cardProgressBar.Value = timeUntilNextLoot;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
            if (timeUntilNextLoot >= lootTime)
            {   
                SpawnLoot(loots);
                timeUntilNextLoot = 0;
            }
        }

        public override void StartUseResource(float speedModifier = 1f)
        {
            // Do nothing
        }

        public override void StopUseResource()
        {
            // Cannot be stopped
        }

        private IEnumerator DelayLootStart(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_START, cardProgressBar);
        }
    }
}