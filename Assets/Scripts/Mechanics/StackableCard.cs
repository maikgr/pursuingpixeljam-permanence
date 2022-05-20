using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(SelectableCard))]
    public class StackableCard : EventBusBehaviour<GameCard>
    {
        [SerializeField]
        private List<CardType> canStackOnTypes;
        private Rigidbody2D cardBody;
        private Vector2 placementOffset;
        private SelectableCard selectableCard;
        private Collider2D bottomCardCollider;
        private GameCard stackedCard;

        protected override void Awake()
        {
            base.Awake();
            cardBody = GetComponent<Rigidbody2D>();
            selectableCard = GetComponent<SelectableCard>();
            placementOffset = new Vector2(0, -0.4f);
        }

        private void Start() {
            selectableCard.AddEventListener(SelectableCardEvent.ON_SELECTED, OnCardRemoved);
            selectableCard.AddEventListener(SelectableCardEvent.ON_DROPPED, OnCardDropped);
        }

        private void OnDestroy() {
            OnCardRemoved(selectableCard);
            selectableCard.RemoveEventListener(SelectableCardEvent.ON_SELECTED, OnCardRemoved);
            selectableCard.RemoveEventListener(SelectableCardEvent.ON_DROPPED, OnCardDropped);
        }

        private void OnCardDropped(SelectableCard card)
        {
            var results = new RaycastHit2D[10];
            var contacts = card.attachedCollider.Cast(Vector2.zero, results, Mathf.Infinity);
            var targetCollider = results.FirstOrDefault(cast => cast.collider != null).collider;
            if (targetCollider != null)
            {
                var targetCard = targetCollider.GetComponent<GameCard>();
                if (targetCard != null) {
                    stackedCard = targetCard;
                    if (canStackOnTypes.Any(allow => allow.Equals(targetCard.cardType))) {
                        DispatchEvent(StackableCardEvent.ON_STACKED, stackedCard);
                        targetCollider.enabled = false;
                        bottomCardCollider = targetCollider;
                        cardBody.position = new Vector2(
                            targetCard.transform.position.x + placementOffset.x,
                            targetCard.transform.position.y + placementOffset.y
                        );
                        transform.position = new Vector3(
                            targetCard.transform.position.x + placementOffset.x,
                            targetCard.transform.position.y + placementOffset.y,
                            targetCard.transform.position.z - 1
                        );
                        cardBody.bodyType = RigidbodyType2D.Static;
                    }
                    // Push this card away if not allowed
                    else
                    {
                        DispatchEvent(StackableCardEvent.ON_NOT_ALLOWED, stackedCard);
                        var magnitude = 1000f;
                        var pushDirection = transform.position - targetCard.transform.position;
                        cardBody.AddForce(pushDirection.normalized * magnitude);
                    }
                }
            }
        }

        private void OnCardRemoved(SelectableCard card)
        {
            if (bottomCardCollider != null)
            {
                DispatchEvent(StackableCardEvent.ON_REMOVED, stackedCard);
                bottomCardCollider.enabled = true;
                bottomCardCollider = null;
                cardBody.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    public static class StackableCardEvent
    {
        public const string ON_STACKED = "onStacked";
        public const string ON_REMOVED = "onRemoved";
        public const string ON_NOT_ALLOWED = "onNotAllowed";
    }
}
