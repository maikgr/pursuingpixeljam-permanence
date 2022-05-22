using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Entities;
using UnityEngine.EventSystems;
using Permanence.Scripts.Mechanics;

namespace Permanence.Scripts.UI {
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
                380,
                475,
                565,
                665
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
            SfxController.instance.PlayAudio(GameSfxType.DetailsOpen, transform.position);
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
            var eventConsumerCard = gameCard.gameObject.GetComponent<MaterialConsumerCard<CardProgressBar>>();
            if (consumerCard != null)
            {
                numberOfSlots = Mathf.Min(materialSlots.Count, consumerCard.requiredMaterials.Count);
            
                for (var i = 0; i < materialSlots.Count; ++i)
                {
                    if (i < consumerCard.requiredMaterials.Count)
                    {
                        var requiredMaterial = consumerCard.requiredMaterials[i];
                        var materialSlot = materialSlots[i];
                        materialSlot.SetSlotRequirement(requiredMaterial.cardType, consumerCard, i);
                        materialSlot.gameObject.SetActive(true);
                    }
                }
            }
            else if (eventConsumerCard != null)
            {
                numberOfSlots = Mathf.Min(materialSlots.Count, eventConsumerCard.requiredMaterials.Count);
            
                for (var i = 0; i < materialSlots.Count; ++i)
                {
                    if (i < eventConsumerCard.requiredMaterials.Count)
                    {
                        var requiredMaterial = eventConsumerCard.requiredMaterials[i];
                        var materialSlot = materialSlots[i];
                        materialSlot.SetSlotRequirement(requiredMaterial.cardType, eventConsumerCard, i);
                        materialSlot.gameObject.SetActive(true);
                    }
                }
            }

            background.sizeDelta = new Vector2(bgWidthPreset[numberOfSlots], background.sizeDelta.y);

            // Show submit button if this requires material
            submitButton.gameObject.SetActive(numberOfSlots > 0);
        }

        public void HideModal()
        {
            SfxController.instance.PlayAudio(GameSfxType.ButtonClick, transform.position);
            submitButton.gameObject.SetActive(false);
            modal.gameObject.SetActive(false);
            modalIsOpen = false;
        }

        public void onSubmit()
        {
            SfxController.instance.PlayAudio(GameSfxType.ButtonClick, transform.position);
            var consumerCard = selectedCard.gameObject.GetComponent<MaterialConsumerCard>();
            var eventConsumerCard = selectedCard.gameObject.GetComponent<MaterialConsumerCard<CardProgressBar>>();
            if (consumerCard != null)
            {
                if (consumerCard.SubmitMaterial())
                {
                    HideModal();
                }
                else
                {
                    // Show warning
                }
            }
            else if (eventConsumerCard != null)
            {
                if (eventConsumerCard.SubmitMaterial())
                {
                    HideModal();
                }
                else
                {
                    // Show warning
                }
            }
            else
            {
                HideModal();
            }
        }
    }
}
