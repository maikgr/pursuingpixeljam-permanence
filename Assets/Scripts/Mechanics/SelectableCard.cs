using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Constants;
using System;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class SelectableCard : EventBusBehaviour<SelectableCard>
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D body2D;
        private Collider2D cardCollider;
        private Camera mainCamera;
        private Vector3 mouseOffset;
        public Vector2 CurrentPosition => body2D.position;
        public Collider2D attachedCollider => cardCollider;
        public bool IsSelected { get; private set; }
        private Vector3 originalPos;
        private CanvasGameCard canvasGameCard;

        protected override void Awake() {
            base.Awake();
            cardCollider = GetComponent<Collider2D>();
            body2D = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            canvasGameCard = GameObject.FindGameObjectWithTag(GameTags.CanvasGameCard).GetComponent<CanvasGameCard>();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && !IsSelected)
            {
                var hit = Physics2D.Raycast(mainCamera.WorldMousePosition(), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.Equals(gameObject))
                {
                    IsSelected = true;
                    SelectCard();
                }
            }
            else if (Input.GetMouseButton(0) && IsSelected)
            {
                MoveCard();
            }
            else if (Input.GetMouseButtonUp(0) && IsSelected)
            {
                IsSelected = false;
                DropCard();
            }
        }

        private void SelectCard() {
            originalPos = transform.position;
            canvasGameCard.SetCard(this);
            mouseOffset = transform.position - mainCamera.WorldMousePosition();
            cardCollider.enabled = false;
            spriteRenderer.sortingOrder = 11;
            SfxController.instance.PlayAudio(GameSfxType.CardPickup, transform.position);
            DispatchEvent(SelectableCardEvent.ON_SELECTED, this);
        }

        private void MoveCard() {
            var position = (Vector2)(mainCamera.WorldMousePosition() + mouseOffset);
            body2D.position = position;
            DispatchEvent(SelectableCardEvent.ON_MOVING, this);
        }

        private void DropCard() {
            canvasGameCard.UnsetCard();
            cardCollider.enabled = true;
            spriteRenderer.sortingOrder = 1;
            SfxController.instance.PlayAudio(GameSfxType.CardPlaced, transform.position);
            DispatchEvent(SelectableCardEvent.ON_DROPPED, this);
        }

        public void ResetPosition() {
            SfxController.instance.PlayAudio(GameSfxType.CardPlaced, transform.position);
            transform.position = originalPos;
            body2D.position = originalPos;
            DropCard();
        }
    }

    public static class SelectableCardEvent
    {
        public const string ON_SELECTED = "onSelected";
        public const string ON_MOVING = "onMoving";
        public const string ON_DROPPED = "onDropped";
    }
}