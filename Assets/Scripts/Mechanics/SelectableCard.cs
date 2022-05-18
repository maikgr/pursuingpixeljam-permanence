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
        private Rigidbody2D body2D;
        private Collider2D cardCollider;
        private Camera mainCamera;
        private Vector3 mouseOffset;
        public Vector2 CurrentPosition => body2D.position;
        public Collider2D attachedCollider => cardCollider;
        private bool isSelected;
        private DetailsModalController detailsModal;

        protected override void Awake() {
            base.Awake();
            cardCollider = GetComponent<Collider2D>();
            body2D = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            detailsModal = GameObject.FindGameObjectWithTag(GameTags.GameCanvas).GetComponent<DetailsModalController>();
        }

        private void Start() {
            detailsModal.AddEventListener(DetailsModalEvent.ON_SHOW, OnModalShow);
            detailsModal.AddEventListener(DetailsModalEvent.ON_HIDE, OnModalHide);
        }

        private void OnDestroy() {
            detailsModal.RemoveEventListener(DetailsModalEvent.ON_SHOW, OnModalShow);
            detailsModal.RemoveEventListener(DetailsModalEvent.ON_HIDE, OnModalHide);
        }

        private void OnMouseDown() {
            if (isSelected) return;
            mouseOffset = transform.position - mainCamera.WorldMousePosition();
            cardCollider.enabled = false;
            DispatchEvent(SelectableCardEvent.ON_SELECTED, this);
        }

        private void OnMouseDrag() {
            if (isSelected) return;
            body2D.position = (Vector2)(mainCamera.WorldMousePosition() + mouseOffset);
            DispatchEvent(SelectableCardEvent.ON_MOVING, this);
        }

        private void OnMouseUp() {
            if (isSelected) return;
            cardCollider.enabled = true;
            DispatchEvent(SelectableCardEvent.ON_DROPPED, this);
        }

        public void OnModalShow(GameCard gameCard)
        {
            if (gameCard.gameObject.Equals(gameObject))
            {
                Debug.Log("show", gameObject);
                isSelected = true;
            }
        }

        public void OnModalHide(GameCard gameCard)
        {
            if (gameCard.gameObject.Equals(gameObject))
            {
                Debug.Log("hide", gameObject);
                isSelected = false;
            }
        }
    }

    public static class SelectableCardEvent
    {
        public const string ON_SELECTED = "onSelected";
        public const string ON_MOVING = "onMoving";
        public const string ON_DROPPED = "onDropped";
    }
}