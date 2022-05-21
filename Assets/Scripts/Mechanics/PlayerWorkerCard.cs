
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StackableCard))]
    public class PlayerWorkerCard : EventBusBehaviour
    {
        [SerializeField]
        private List<CardType> CanWorkOnTypes;
        private StackableCard card;
        private CardType[] resources = new CardType[2] { CardType.River, CardType.Mineshaft };
        private CardType[] hazards = new CardType[2] { CardType.Bandit, CardType.Fire };
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
            if (other == null) return;
            workplaceCard = other;
            if (resources.Any(res => res.Equals(other.cardType))) {
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StartUseResource();
                DispatchEvent(WorkerCardEvent.ON_START_WORKING);
            }
            else if (CardType.Bandit.Equals(other.cardType))
            {
                var bandit = other.gameObject.GetComponent<BanditCard>();
                bandit.StartReduceHealth(() => StopWorking(other));
                DispatchEvent(WorkerCardEvent.ON_START_WORKING);
            }
            else if (CardType.Fire.Equals(other.cardType))
            {
                var blocker = other.gameObject.GetComponent<BlockerCard>();
                blocker.StartReduceHealth(() => StopWorking(other));
                DispatchEvent(WorkerCardEvent.ON_START_WORKING);
            }
        }

        private void StopWorking(GameCard other) {
            if (other == null) return;
            workplaceCard = null;
            if (resources.Any(res => res.Equals(other.cardType))) {
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StopUseResource();
                DispatchEvent(WorkerCardEvent.ON_STOP_WORKING);
            }
            else if (CardType.Bandit.Equals(other.cardType))
            {
                var bandit = other.gameObject.GetComponent<BanditCard>();
                bandit.StopReduceHealth();
                DispatchEvent(WorkerCardEvent.ON_STOP_WORKING);
            }
            else if (CardType.Fire.Equals(other.cardType))
            {
                var blocker = other.gameObject.GetComponent<BlockerCard>();
                blocker.StopReduceHealth();
                DispatchEvent(WorkerCardEvent.ON_STOP_WORKING);
            }
        }
    }

    public static class WorkerCardEvent
    {
        public const string ON_START_WORKING = "onStartWorking";
        public const string ON_STOP_WORKING = "onStopWorking";
    }
}