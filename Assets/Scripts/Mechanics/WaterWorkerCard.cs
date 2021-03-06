
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StackableCard))]
    public class WaterWorkerCard : EventBusBehaviour
    {
        private StackableCard card;
        private GameCard workplaceCard;

        protected override void Awake() {
            base.Awake();
            card = GetComponent<StackableCard>();
        }

        private void Start() {
            card.AddEventListener(StackableCardEvent.ON_STACKED, StartWorking);
            card.AddEventListener(StackableCardEvent.ON_REMOVED, StopWorking);
        }

        private void OnDestroy() {
            StopWorking(workplaceCard);
            card.RemoveEventListener(StackableCardEvent.ON_STACKED, StartWorking);
            card.RemoveEventListener(StackableCardEvent.ON_REMOVED, StopWorking);
        }

        private void StartWorking(IEnumerable<StackableCard> stacks) {
            var other = stacks.Last().GetComponent<GameCard>();
            if (other == null || !CardType.Fire.Equals(other.cardType)) return;
            workplaceCard = other;
            var blocker = other.gameObject.GetComponent<BlockerCard>();
            blocker.StartReduceHealth(() => EmptyWater(), 10f);
            DispatchEvent(WorkerCardEvent.ON_START_WORKING);
        }

        private void StopWorking(IEnumerable<StackableCard> stacks) {
            var other = stacks.Last().GetComponent<GameCard>();
            StopWorking(other);
        }

        private void StopWorking(GameCard other)
        {
            if (other == null || !CardType.Fire.Equals(other.cardType)) return;
            var blocker = other.gameObject.GetComponent<BlockerCard>();
            blocker.StopReduceHealth();
            DispatchEvent(WorkerCardEvent.ON_STOP_WORKING);
        }

        private void EmptyWater()
        {
            Destroy(gameObject, 0.5f);
        }
    }
}