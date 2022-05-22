using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using System;
using System.Collections;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StructureCard))]
    public class EvertreeBudController : ResourceCardBehaviour
    {
        private StructureCard structureCard;

        protected override void Awake()
        {
            base.Awake();
            structureCard = GetComponent<StructureCard>();
            DelayLootStart(1f);
        }

        protected override void Update() {
            if (structureCard.CurrentHealth < structureCard.MaxHealth) {
                timeUntilNextLoot -= Time.deltaTime;
                cardProgressBar.Value = lootTime - timeUntilNextLoot;
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