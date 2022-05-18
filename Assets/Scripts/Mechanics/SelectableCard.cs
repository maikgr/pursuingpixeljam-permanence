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

        protected override void Awake() {
            base.Awake();
            cardCollider = GetComponent<Collider2D>();
            body2D = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
        }

        private void OnMouseDown() {
            mouseOffset = transform.position - mainCamera.WorldMousePosition();
            cardCollider.enabled = false;
            spriteRenderer.sortingOrder = 11;
            DispatchEvent(SelectableCardEvent.ON_SELECTED, this);
        }

        private void OnMouseDrag() {
            var position = (Vector2)(mainCamera.WorldMousePosition() + mouseOffset);
            body2D.position = position;
            DispatchEvent(SelectableCardEvent.ON_MOVING, this);
        }

        private void OnMouseUp() {
            cardCollider.enabled = true;
            spriteRenderer.sortingOrder = 1;
            DispatchEvent(SelectableCardEvent.ON_DROPPED, this);
        }
    }

    public static class SelectableCardEvent
    {
        public const string ON_SELECTED = "onSelected";
        public const string ON_MOVING = "onMoving";
        public const string ON_DROPPED = "onDropped";
    }
}