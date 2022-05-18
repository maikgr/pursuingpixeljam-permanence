
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StackableCard))]
    public class WorkerCard : MonoBehaviour
    {
        [SerializeField]
        private List<CardType> CanWorkOnTypes;
        private StackableCard card;

        private void Awake() {
            card = GetComponent<StackableCard>();
        }

        private void Start() {
            card.AddEventListener(StackableCardEvent.ON_STACKED, StartWorking);
            card.AddEventListener(StackableCardEvent.ON_REMOVED, StopWorking);
        }

        private void OnDestroy() {
            card.RemoveEventListener(StackableCardEvent.ON_STACKED, StartWorking);
            card.RemoveEventListener(StackableCardEvent.ON_REMOVED, StopWorking);
        }

        private void StartWorking(GameCard other) {
            if (other.cardType == CardType.Resource) {
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StartUseResource();
            }
        }

        private void StopWorking(GameCard other) {
            if (other.cardType == CardType.Resource) {
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StopUseResource();
            }
        }
    }
}