using Permanence.Scripts.Cores;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Constants;


namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StructureCard))]
    public class MineshaftController : ResourceCardBehaviour
    {
        private StructureCard structure;

        protected override void Awake()
        {
            base.Awake();
            structure = GetComponent<StructureCard>();
        }

        public override void StartUseResource(float speedModifier = 1f)
        {
            if (structure.CurrentHealth <= 0) return;
            this.speedModifier = speedModifier;
            isLooting = true;
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_START, cardProgressBar);
        }
    }
}
