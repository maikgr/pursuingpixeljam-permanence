using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Mechanics {
    public class DetailsModalController : EventBusBehaviour
    {
        [SerializeField]
        private RectTransform modal;
        [SerializeField]
        private RectTransform background;
        [SerializeField]
        private TMP_Text cardName;
        [SerializeField]
        private TMP_Text cardDescription;
        [SerializeField]
        private TMP_Text cardInstruction;
        [SerializeField]
        private RectTransform closeButton;
        [SerializeField]
        private Image slotPrefab;
        [SerializeField]
        private Vector2 modalPadding;
        [SerializeField]
        private float textMargin;
        [SerializeField]
        private Vector2 modalOffset;
        private bool modalIsOpen;
        private Camera mainCamera;

        private void Awake() {
            mainCamera = Camera.main;
        }

        private void Update() {
            if (!modalIsOpen && Input.GetButtonDown(InputKeyName.Fire2))
            {
                var raycast = Physics2D.Raycast(mainCamera.WorldMousePosition(), Vector2.zero);
                if (raycast.collider != null)
                {
                    var gameCard = raycast.collider.GetComponent<GameCard>();
                    if (gameCard != null)
                    {
                        ShowModal(gameCard, (BoxCollider2D)raycast.collider);
                    }
                }
            }
            else if (modalIsOpen && Input.GetButtonDown(InputKeyName.Fire2))
            {
                HideModal();
            }
        }

        public void ShowModal(GameCard gameCard, BoxCollider2D collider)
        {
            // Adjust card size
            var titleSize = cardName.GetPreferredValues(gameCard.cardName, cardName.rectTransform.rect.width, cardName.rectTransform.rect.height);
            var descriptionSize = cardDescription.GetPreferredValues(gameCard.cardDescription, cardDescription.rectTransform.rect.width, cardDescription.rectTransform.rect.height);
            var instructionSize = cardInstruction.GetPreferredValues(gameCard.cardInstruction, cardInstruction.rectTransform.rect.width, cardInstruction.rectTransform.rect.height);
            background.sizeDelta = new Vector2(
                cardDescription.rectTransform.rect.width + (modalPadding.x * 2),
                titleSize.y + descriptionSize.y + instructionSize.y + closeButton.rect.height + (textMargin * 3) + (modalPadding.y * 2)
            );

            // Adjust text position
            cardDescription.rectTransform.position = new Vector2(
                cardDescription.rectTransform.position.x,
                cardName.rectTransform.position.y - titleSize.y - textMargin
            );
            cardInstruction.rectTransform.position = new Vector2(
                cardInstruction.rectTransform.position.x,
                cardDescription.rectTransform.position.y - descriptionSize.y - textMargin
            );
            closeButton.position = new Vector2(
                closeButton.position.x,
                cardInstruction.rectTransform.position.y - instructionSize.y - textMargin
            );

            // Calculate position on screen
            var boundMax = mainCamera.WorldToScreenPoint(
                collider.bounds.max
            );
            modal.position = boundMax + (Vector3)modalOffset;

            // Set texts
            cardName.text = gameCard.cardName;
            cardDescription.text = gameCard.cardDescription;
            cardInstruction.text = gameCard.cardInstruction;

            // Dispatch event
            DispatchEvent(DetailsModalEvent.ON_SHOW);
            modal.gameObject.SetActive(true);
            modalIsOpen = true;
        }

        public void HideModal()
        {
            DispatchEvent(DetailsModalEvent.ON_HIDE);
            modal.gameObject.SetActive(false);
            modalIsOpen =  false;
        }
    }

    public static class DetailsModalEvent
    {
        public const string ON_SHOW = "onShow";
        public const string ON_HIDE = "onHide";
    }
}
