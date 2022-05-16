using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Cores
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(ICardEvent))]
    public class CardInteractionController : MonoBehaviour
    {
        public CardType cardType;
        [SerializeField]
        private List<CardType> disallowedCardTypes;
        private ICardEvent cardEvent;
        private Vector2 placementOffset;
        private Collider2D objCollider;
        private Camera mainCamera;
        private Vector3 mouseOffset;
        private bool isPickedUp;
        private CardInteractionController interactedObject;

        private void Awake() {
            objCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
            placementOffset = new Vector2(0, -0.4f);
            cardEvent = GetComponent<ICardEvent>();
        }

        private void OnMouseDown() {
            mouseOffset = transform.position - WorldMousePosition;
            objCollider.enabled = false;
            isPickedUp = true;
            if (interactedObject != null)
            {
                cardEvent.OnStopInteracting(interactedObject);
                interactedObject = null;
            }
        }

        private void OnMouseDrag() {
            transform.position = (Vector2)(WorldMousePosition + mouseOffset);
        }

        private void OnMouseUp() {
            objCollider.enabled = true;
            StartCoroutine(DisableStatusPickUp());
        }

        // Wait for a delay to disable to let collision check happen
        private IEnumerator DisableStatusPickUp()
        {
            yield return new WaitForFixedUpdate();
            isPickedUp = false;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (isPickedUp)
            {
                var otherObj = other.gameObject.GetComponent<CardInteractionController>();
                if (otherObj != null)
                {
                    // Push object away if not allowed
                    if (disallowedCardTypes.Any(disallow => disallow.Equals(otherObj.cardType)))
                    {
                        var pushOffset = otherObj.transform.position - WorldMousePosition;
                        transform.position = new Vector2(
                            other.transform.position.x + pushOffset.x,
                            other.transform.position.y + pushOffset.y
                        );
                    }
                    else
                    {
                        Debug.Log("CollisionEnter", this);
                        transform.position = new Vector3(
                            other.transform.position.x + placementOffset.x,
                            other.transform.position.y + placementOffset.y,
                            other.transform.position.z - 1
                        );
                        interactedObject = otherObj;
                        cardEvent.OnStartInteracting(otherObj);
                    }
                }
                isPickedUp = false;
            }
        }

        private Vector3 WorldMousePosition {
            get {
                return mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}