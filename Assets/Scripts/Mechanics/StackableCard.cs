using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.UI;
using Sirenix.OdinInspector;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(GameCard))]
    public class StackableCard : EventBusBehaviour<IEnumerable<StackableCard>>
    {
        [SerializeField]
        private List<CardType> canStackOnTypes;
        private Vector2 placementOffset;
        private Vector3 mouseOffset;
        private Vector3 originalPos;
        private Camera mainCamera;
        private CanvasGameCard canvasGameCard;
        public StackableCard ParentCard { get; private set; }
        public StackableCard ChildCard { get; private set; }
        public Collider2D CardCollider
        {
            get
            {
                return GetComponent<Collider2D>();
            }
        }
        public IEnumerable<StackableCard> Stacks
        {
            get
            {
                var parentStack = new List<StackableCard>();
                var childStack = new List<StackableCard>() { this };
                TraverseParentCards((card, index) => parentStack.Add(card));
                TraverseChildCards((card, index) => childStack.Add(card));

                return new List<StackableCard>().Concat(parentStack).Concat(childStack);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            placementOffset = new Vector2(0, -0.65f);
            mainCamera = Camera.main;
        }

        private void Start()
        {
            canvasGameCard = GameObject.FindGameObjectWithTag(GameTags.CanvasGameCard).GetComponent<CanvasGameCard>();
        }

        private void OnDestroy()
        {
            DispatchEvent(StackableCardEvent.ON_REMOVED, Stacks);
        }

        public void SelectCard()
        {
            // Set position
            originalPos = transform.position;
            mouseOffset = transform.position - Camera.main.WorldMousePosition();
            transform.position = new Vector3(transform.position.x, transform.position.y, -20f);

            DetachFromParent();

            // Disable all colliders
            CardCollider.enabled = false;
            TraverseChildCards((card, index) => card.CardCollider.enabled = false);

            canvasGameCard.SetCard(this);
            SfxController.instance.PlayAudio(GameSfxType.CardPickup, transform.position);
        }

        public void MoveCard()
        {
            var position = (Vector2)(Camera.main.WorldMousePosition() + mouseOffset);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
        
        private StackableCard GetCardBelow()
        {
            var results = new RaycastHit2D[10];
            var contacts = CardCollider.Cast(Vector2.zero, results);
            var targetCollider = results.Where(res => res.collider != null && res.collider.GetComponent<StackableCard>() != null)
                .OrderBy(res => res.transform.position.z)
                .FirstOrDefault().collider;
            if (targetCollider != null)
            {
                return targetCollider.GetComponent<StackableCard>();
            }
            return null;
        }

        private void StackOnCard(StackableCard card)
        {
            // Get the last card stack
            var lastCard = card.Stacks.Last();

            // Set this card position
            transform.SetParent(lastCard.transform);
            transform.localPosition = new Vector3(
                placementOffset.x,
                placementOffset.y,
                -1f
            );

            // Add this card to stack
            lastCard.Attach(this);
            ParentCard = lastCard;

            // Dispatch event
            DispatchEvent(StackableCardEvent.ON_STACKED, Stacks);
        }

        public void DropCard()
        {
            // enable this card collider
            CardCollider.enabled = true;

            // Check for card below
            var targetCard = GetCardBelow();
            if (targetCard != null)
            {
                if (canStackOnTypes.Any(allow => allow.Equals(targetCard.GetComponent<GameCard>().cardType)))
                {
                    StackOnCard(targetCard);
                }
                // Push this card away if not allowed
                else
                {
                    DispatchEvent(StackableCardEvent.ON_NOT_ALLOWED, Stacks);
                    ResetPosition();
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }

            // Enable child colliders
            TraverseChildCards((card, index) =>
            {
                card.CardCollider.enabled = true;
            });
            SfxController.instance.PlayAudio(GameSfxType.CardPlaced, transform.position);
        }


        private void DetachFromParent()
        {
            if (ParentCard != null)
            {
                DispatchEvent(StackableCardEvent.ON_REMOVED, Stacks);
                ParentCard.Detach();
                ParentCard = null;
                transform.SetParent(null);
            }
        }

        private void TraverseChildCards(Action<StackableCard, int> action)
        {
            var childNode = ChildCard;
            var index = 0;
            while (childNode != null)
            {
                action.Invoke(childNode, index);
                childNode = childNode.ChildCard;
                index += 1;
            }
        }

        private void TraverseParentCards(Action<StackableCard, int> action)
        {
            var parentNode = ParentCard;
            var index = 0;
            while (parentNode != null)
            {
                action.Invoke(parentNode, index);
                parentNode = parentNode.ParentCard;
                index += 1;
            }
        }

        public void ResetPosition()
        {
            transform.position = originalPos;
            SfxController.instance.PlayAudio(GameSfxType.CardPlaced, transform.position);
            DropCard();
        }

        public void Attach(StackableCard card)
        {
            ChildCard = card;
        }

        public void Detach()
        {
            ChildCard = null;
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }


    public static class StackableCardEvent
    {
        public const string ON_STACKED = "onStacked";
        public const string ON_REMOVED = "onRemoved";
        public const string ON_NOT_ALLOWED = "onNotAllowed";
    }
}
