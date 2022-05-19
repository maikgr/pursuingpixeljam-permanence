using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;
using UnityEngine.EventSystems;

namespace Permanence.Scripts.Mechanics {
    public class DetailsModalController : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;
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
        private RectTransform submitButton;
        [SerializeField]
        private Vector2 modalOffset;
        [SerializeField]
        private List<DetailsModalSlotController> materialSlots;
        [SerializeField]
        private GraphicRaycaster graphicRaycaster;
        private bool modalIsOpen;
        private Camera mainCamera;
        private GameCard selectedCard;
        private List<float> bgWidthPreset;

        private void Awake() {
            mainCamera = Camera.main;
            bgWidthPreset = new List<float> {
                350,
                460,
                550,
                640
            };
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
                        selectedCard = gameCard;
                        ShowModal(selectedCard, (BoxCollider2D)raycast.collider);
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
            AdjustModalDimension(gameCard, collider);

            // Calculate position on screen
            modal.position = collider.bounds.max + (Vector3)modalOffset;

            // Set texts
            cardName.text = gameCard.cardName;
            cardDescription.text = gameCard.cardDescription;
            cardInstruction.text = gameCard.cardInstruction;

            modal.gameObject.SetActive(true);
            modalIsOpen = true;
        }

        private void AdjustModalDimension(GameCard gameCard, BoxCollider2D collider)
        {
            // Disable all material slots
            materialSlots.ForEach(slot => {
                slot.ClearSlot();
                slot.gameObject.SetActive(false);
            });
            // Check if card requires materials
            var numberOfSlots = 0;
            var consumerCard = gameCard.gameObject.GetComponent<MaterialConsumerCard>();
            if (consumerCard != null)
            {
                numberOfSlots = Mathf.Min(materialSlots.Count, consumerCard.requiredMaterials.Count);
                for (var i = 0; i < materialSlots.Count; ++i)
                {
                    if (i < consumerCard.requiredMaterials.Count)
                    {
                        materialSlots[i].SetSlotRequirement(consumerCard.requiredMaterials[i].cardType);
                        materialSlots[i].gameObject.SetActive(true);
                    }
                }
            }

            background.sizeDelta = new Vector2(bgWidthPreset[numberOfSlots], background.sizeDelta.y);

            // Show submit button if this requires material
            if (numberOfSlots > 0)
            {
                submitButton.gameObject.SetActive(true);
            }
            else
            {
                submitButton.gameObject.SetActive(false);
            }
        }

        public void HideModal()
        {
            submitButton.gameObject.SetActive(false);
            modal.gameObject.SetActive(false);
            modalIsOpen = false;
        }

        public void onSubmit()
        {
            HideModal();
        }
    }
}
