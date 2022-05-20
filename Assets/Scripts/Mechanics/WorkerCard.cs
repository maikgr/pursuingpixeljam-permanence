
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StackableCard))]
    public class WorkerCard : EventBusBehaviour
    {
        [SerializeField]
        private List<CardType> CanWorkOnTypes;
        private StackableCard card;
        private CardType[] resources = new CardType[2] { CardType.River, CardType.Mineshaft };
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

        private void StartWorking(GameCard other) {
            workplaceCard = other;
            if (resources.Any(res => res.Equals(other.cardType))) {
                DispatchEvent(WorkerCardEvent.ON_START_WORKING);
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StartUseResource();
            }
        }

        private void StopWorking(GameCard other) {
            workplaceCard = null;
            if (resources.Any(res => res.Equals(other.cardType))) {
                DispatchEvent(WorkerCardEvent.ON_STOP_WORKING);
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StopUseResource();
            }
        }
    }

    public static class WorkerCardEvent
    {
        public const string ON_START_WORKING = "onStartWorking";
        public const string ON_STOP_WORKING = "onStopWorking";
    }
}