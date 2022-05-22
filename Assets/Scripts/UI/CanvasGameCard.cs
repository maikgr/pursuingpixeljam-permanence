using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Permanence.Scripts.Mechanics;

namespace Permanence.Scripts.UI {
    public class CanvasGameCard : MonoBehaviour, IDragHandler, IDropHandler
    {
        public SelectableCard selectableCard;
        [SerializeField]
        private GraphicRaycaster graphicRaycaster;
        private Canvas canvas;
        private bool isDragging;

        private void Awake() {
            canvas = graphicRaycaster.GetComponent<Canvas>();
        }

        public void SetCard(SelectableCard card)
        {
            selectableCard = card;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 pos
            );
            transform.position = canvas.transform.TransformPoint(pos);
            isDragging = true;
        }

        public void UnsetCard()
        {
            selectableCard = null;
        }

        private void Update() {
            if (isDragging && Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Input.mousePosition,
                    canvas.worldCamera,
                    out Vector2 pos
                );
                transform.position = canvas.transform.TransformPoint(pos);
            }
            else if (isDragging && Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                CheckSlot(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnDrop(PointerEventData eventData)
        {
            CheckSlot(eventData);
        }

        private void CheckSlot(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);
            var slot = results.FirstOrDefault(res => res.gameObject.GetComponent<DetailsModalSlotController>() != null);
            if (slot.gameObject != null)
            {
                var modalSlot = slot.gameObject.GetComponent<DetailsModalSlotController>();
                var gameCard = selectableCard.GetComponent<GameCard>();
                if (modalSlot == null || !modalSlot.TrySetSlot(gameCard))
                {
                    selectableCard.ResetPosition();
                }
                else
                {
                    GameObject.Destroy(selectableCard.gameObject);
                }
            }
            else
            {
                UnsetCard();
            }
        }
    }
}