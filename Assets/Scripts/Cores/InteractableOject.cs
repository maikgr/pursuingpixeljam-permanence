using System;
using UnityEngine;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Cores
{
    public class InteractableOject : MonoBehaviour
    {
        [SerializeField]
        private Vector2 placementOffset;
        private Collider2D objCollider;
        private Camera mainCamera;
        private Vector3 offset;

        private void Awake() {
            objCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
        }

        private void OnMouseDown() {
            offset = transform.position - WorldMousePosition;
            objCollider.enabled = false;
        }

        private void OnMouseDrag() {
            transform.position = (Vector2)(WorldMousePosition + offset);
        }

        private void OnMouseUp() {
            RaycastMouseToTag(GameTags.GameCard, (targetPos) => {
                var newPos = targetPos + (Vector3)placementOffset;
                transform.position = new Vector3(newPos.x, newPos.y, targetPos.z - 1);
            });
            objCollider.enabled = true;
        }

        private Vector3 WorldMousePosition {
            get {
                return mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        private void RaycastMouseToTag(string targetTag, Action<Vector3> onHit)
        {
            var rayOrigin = mainCamera.transform.position;
            RaycastHit2D hitInfo = Physics2D.Raycast(WorldMousePosition, Vector2.zero);
            if (hitInfo.collider != null && hitInfo.transform.CompareTag(targetTag))
            {
                onHit.Invoke(hitInfo.transform.position);
            }
        }
    }
}